using UnityEngine;

public class Tower_Bullet_Default : MonoBehaviour
{
    // target to persue
    private Transform target;
    public float speed = 50f;
    public float explosionRadius = 0f;
    public int damage = 50;
    public GameObject impactEffect;

    public void Seek(Transform target_)
    {
        target = target_;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return; // incase it takes long
        }

        // bullet orientation
        Vector3 dir = target.position - transform.position;
        float frameDistance = speed * Time.deltaTime;

        if (dir.magnitude <= frameDistance) // prevent overshoots
        {
            HitTarget();
            return;
        }
        transform.Translate(dir.normalized * frameDistance, Space.World);
        transform.LookAt(target);
    }

    void Damage(Transform enemy)
    {
        UnitMovement e = enemy.GetComponent<UnitMovement>();
        if (e != null)
        {
            e.TakeDamage(damage);
        }
    }
    void  HitTarget()
    {
        GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 1.5f);

        if (explosionRadius > 0f)
        {
            Explode();
        }
        else
        {
            Damage(target);
        }

        Destroy(gameObject);
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Enemy")
            {
                Damage(collider.transform);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
