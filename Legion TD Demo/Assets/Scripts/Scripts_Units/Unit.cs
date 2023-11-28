using UnityEngine;

public class Unit : MonoBehaviour
{
    public float attackRange = 5f;
    public float attackDamage = 10f;
    public LayerMask targetLayer;
    public float attackCooldown = 1.5f;

    private float attackTimer;
    private Transform target;

    void Update()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown)
        {
            FindTarget();
            if (target != null)
            {
                AttackTarget();
                attackTimer = 0f;
            }
        }
    }

    void FindTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange, targetLayer);
        if (colliders.Length > 0)
        {
            target = colliders[0].transform; // Target the first unit found within range
        }
        else
        {
            target = null;
        }
    }

    void AttackTarget()
    {
        if (target != null)
        {
            // Rotate towards the target
            transform.LookAt(target);

            // Attack the target
            Unit targetUnit = target.GetComponent<Unit>();
            if (targetUnit != null)
            {
                targetUnit.TakeDamage(attackDamage);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        // Reduce health or add your damage logic here
        Debug.Log(name + " took " + damage + " damage!");
    }
}
