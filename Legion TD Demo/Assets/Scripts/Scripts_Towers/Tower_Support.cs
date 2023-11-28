using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_Support : MonoBehaviour
{
    public float health;

    [SerializeField]
    private Transform target;
    private Tower targetTower;

    [Header("Attributes")]
    public float range = 15f;

    [Header("Bullets")]
    public float fireRate = 1f;
    private float fireCountdown = 0f;
    public GameObject bulletPrefab;

    [Header("Heal Rate (Per Sec)")]
    public float healRate = 1f;

    [Header("Unity Fields")]
    public string allyTag = "Tower";
    public Transform rotator;
    public float turnSpeed = 10f;
    public Transform firePoint;

    [Header("Laser Components")]
    public int dmgOverTime = 20;
    public bool useLaser = false;
    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public float slowRatio = .5f;

    [Header("Buffs")]
    public int dmgBuff;
    public float rangeBuff;
    public float fireRateBuff;
    public float buffer = 2f;

    // Start is called before the first frame update
    void Start()
    {
        // per x sec
        InvokeRepeating("UpdateTowerTarget", 0f, 0.5f);
    }

    void UpdateTowerTarget()
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag(allyTag);
        // store closest tower found
        float shortestDistance = Mathf.Infinity;
        GameObject nearestTower = null;

        foreach (GameObject tower in towers)
        {
            float distanceToTower = Vector3.Distance(transform.position, tower.transform.position);
            if (distanceToTower < shortestDistance)
            {
                shortestDistance = distanceToTower;
                nearestTower = tower;
            }
        }

        if (nearestTower != null && shortestDistance <= range)
        {
            target = nearestTower.transform;
            targetTower = nearestTower.GetComponent<Tower>();
        }
        else
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
                }
            }
            return;
        }

        LockOnTarget();
        if (useLaser)
        {
            Laser();
        }
        else
        {
            if (fireCountdown <= 0)
            {
                Shoot();
                fireCountdown = 1f / fireRate;
            }
            fireCountdown -= Time.deltaTime;
        }
    }

    void Laser()
    {
        if (targetTower.isBuffed == false)
        {
            targetTower.BuffTower(fireRateBuff, rangeBuff, dmgBuff);
        }
        else
        {
            return;
        }
        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impactEffect.Play();
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
}