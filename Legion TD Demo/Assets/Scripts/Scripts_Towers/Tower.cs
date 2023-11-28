using System.Collections;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float health;

    [SerializeField]
    private Transform target;
    private UnitMovement targetEnemy;

    [Header("Attributes")]
    public float range = 15f;

    [Header("Bullets")]
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    [Header("Unity Fields")]
    public string enemyTag = "Enemy";

    public Transform rotator;
    public float turnSpeed = 10f;

    public GameObject bulletPrefab;
    public Transform firePoint;

    public int bulletsPerShot;

    [Header("Laser Components")]
    public int dmgOverTime = 20;
    public bool useLaser = false;
    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public Light impactLight;
    public float slowRatio = .5f;

    // temps to hold base stats
    private float baseRate;
    private float baseRange;
    private int baseDmg;

    public float buffTimer = 3.5f;
    public GameObject buffEffect;
    public bool isBuffed = false;

    public bool targetFirst = true;

    [SerializeField]
    GameObject[] enemies;

    
    
    // Start is called before the first frame update
    void Start()
    {
        // per x sec
        InvokeRepeating("UpdateTarget", 0f, 0.5f);

        baseRate = fireRate;
        baseDmg = dmgOverTime;
        baseRange = range;
        buffEffect.transform.position = transform.position;
        isBuffed = false;

        targetFirst = true;
    }

    void UpdateTarget()
    {
        enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        
        // store closest enemy found
        float maxDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        GameObject futhestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < maxDistance)
            {
                maxDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && maxDistance <= range)
        {
            target = nearestEnemy.transform;
            targetEnemy = nearestEnemy.GetComponent<UnitMovement>();
        } else
        {
            target = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            if (useLaser)
            {
                if (lineRenderer.enabled)
                {
                    lineRenderer.enabled = false;
                    impactEffect.Stop();
                    impactLight.enabled = false;
                }
            }
            return;
        }

        LockOnTarget();
        if (useLaser)
        {
            Laser();
        } else
        {
            if (fireCountdown <= 0)
            {
                Shoot();
                fireCountdown = 1f / fireRate;
            }
            fireCountdown -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer.ToString() == "Enemy")
        {
            
        }
    }

    void Laser()
    {
        targetEnemy.TakeDamage(dmgOverTime * Time.deltaTime);
        targetEnemy.Slow(slowRatio);
        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impactEffect.Play();
            impactLight.enabled = true;
        }
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);

        Vector3 dir = firePoint.position - target.position;
        impactEffect.transform.position = target.position + dir.normalized;
        impactEffect.transform.rotation = Quaternion.LookRotation(dir);
    }

    void LockOnTarget()
    {
        // look at target
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(rotator.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles; // XYZ
        rotator.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void Shoot()
    {
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Tower_Bullet_Default bullet = bulletGO.GetComponent<Tower_Bullet_Default>();

        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }

    public void DealDamage(float damage)
    {
        health -= damage;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public void BuffTower(float newFireRate, float newRange, int dmg)
    {   
        StartCoroutine(RecieveBuffs(newFireRate, newRange, dmg));
    }

    void RevertBuffs()
    {
        fireRate = baseRate;
        range = baseRange;
        dmgOverTime = baseDmg;
        isBuffed = false;
    }

    IEnumerator RecieveBuffs(float newFireRate, float newRange, int dmg)
    {
        isBuffed = true;
        fireRate = newFireRate;
        range = newRange;
        dmgOverTime = dmg;
       // Debug.Log(range);
        yield return new WaitForSeconds(buffTimer);
        RevertBuffs();
    }

}

