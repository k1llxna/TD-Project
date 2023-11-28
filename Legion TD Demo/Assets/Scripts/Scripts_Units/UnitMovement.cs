using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SphereCollider))]
public class UnitMovement : MonoBehaviour
{
    public enum MovementState { InActive, Move, Seek, Targeting, Attacking }
    public MovementState currentState = MovementState.InActive;

    public Transform[] waypoints;
    private int currentWaypointIndex;

    // default stats impletmented
    public float moveSpeed = 5f;
    float startSpeed;
    public float attackRange = 5f;
    public float attackDamage = 10f;
    public LayerMask targetLayer;
    public float attackCooldown = 1.5f;

    public float hp;
    public float starthp;
    public Image hpBar;
    public Transform canvasTarget;

    [SerializeField]
    private Transform target;
    private float attackTimer = 0f;
    

    void Start()
    {
        starthp = hp;
        startSpeed = moveSpeed;
        this.gameObject.GetComponent<SphereCollider>().radius = attackRange;

        currentWaypointIndex = 0;
        GameObject[] waypointObjects = GameObject.FindGameObjectsWithTag("Waypoint");
        waypoints = new Transform[waypointObjects.Length];

        for (int i = 0; i < waypointObjects.Length; i++)
        {
            waypoints[i] = waypointObjects[i].transform;
        }
       // currentState = MovementState.Move;
        StartCoroutine(MovementRoutine());
    }

    IEnumerator MovementRoutine()
    {
        while (true)
        {
            switch (currentState)
            {
                case MovementState.InActive:
                    // Do idle behavior or transition to another state                
                    break;

                case MovementState.Move:
                    Move();
                    break;

                case MovementState.Seek:
                    FindTarget();
                    break;

                case MovementState.Targeting:
                    TargetAndAttack();
                    break;

                case MovementState.Attacking:
                    AttackTarget();
                    break;
            }
            yield return null;
        }
    }

    #region Combat
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");

        if (target == null)
            FindTarget();
    }

    void FindTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange, targetLayer);
        if (colliders.Length > 0)
        {
            currentState = MovementState.Targeting;
            target = colliders[0].transform; // Target the first unit found within range
        }
    }

    void TargetAndAttack()
    {
        if (target != null)
        {
            currentState = MovementState.Attacking;
        }
        else
        { // attacker and defender logic
            currentState = MovementState.InActive;

            if (this.gameObject.layer.ToString() == "Attacker")
            {
                currentState = MovementState.Move;
            }
        }
    }

    void AttackTarget()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown)
        {
            if (target != null && Vector3.Distance(transform.position, target.position) <= attackRange - target.transform.localScale.x)
            {
                // Rotate towards the target
                transform.LookAt(target);

                // Attack the target *add animation logic here and sync*
                UnitMovement targetUnit = target.GetComponent<UnitMovement>();
                if (targetUnit != null)
                {
                    targetUnit.TakeDamage(attackDamage); 
                    attackTimer = 0f;
                }
            }
            else
            {
                //if (this.gameObject.layer.ToString() == "Attacker")
                {
                    currentState = MovementState.Move;
                }
                target = null;            
            }
        }
    }
    #endregion

    #region Movement
    void Move()
    {
        if (currentWaypointIndex < waypoints.Length)
        {
            Transform targetWaypoint = waypoints[currentWaypointIndex];
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);

            if (transform.position == targetWaypoint.position)
            {
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Length)
                {
                    currentState = MovementState.InActive; // Reached the last waypoint, set to idle
                }
            }
        }
    }
    public void StartMoving()
    {
        currentWaypointIndex = 0;
        currentState = MovementState.Move;
    }
    public void StopMoving()
    {
        currentState = MovementState.InActive;
    }

    void MoveBackward()
    {
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
    }

    public void StartMovingForward()
    {
        currentState = MovementState.Move;
    }

    public void StartMovingBackward()
    {
        currentState = MovementState.Seek;
    }

    public void StopMovement()
    {
        currentState = MovementState.InActive;
    }

    public void Slow(float amount)
    {
        moveSpeed = startSpeed * (1f - amount);
    }
    #endregion

    public void TakeDamage(float damage)
    {
        // Reduce health or add your damage logic here
        //Debug.Log(name + " took " + damage + " damage!");
        
        hp -= damage;
        hpBar.fillAmount = hp/ starthp;
        if (hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
