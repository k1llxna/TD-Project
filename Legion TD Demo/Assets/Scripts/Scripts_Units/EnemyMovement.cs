using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
    [Header("Variabes")]
    public float speed = 7f;
    public float maxSpeed = 10f;
    public float mass = 1;
    public float maxVel = 10f;
    public float maxForce = 10;
    public float maxAccel = 6f;
    public float targetRadius;
    public float slowRadius = 2f;
    public float timeToTarget = 0.1f;

    private Transform target;
    private int wavepointIndex = 0;

    private Enemy enemy;

    [SerializeField]
    private Vector3 vel;

    void Start()
    {
        enemy = GetComponent<Enemy>();
        target = Waypoints.waypoints[0];
        vel = Vector3.zero;
    }

    void Update()
    {
        Vector3 dir = target.position - transform.position; // direction to target
        Vector3 steering = dir - vel;
        steering = Vector3.ClampMagnitude(steering, maxForce);
        steering /= mass;

        vel = Vector3.ClampMagnitude(vel + steering, maxVel);   // velocity cliped to a max speed
        transform.position += vel * Time.deltaTime;             // position += velocity * time

        transform.forward = transform.forward.normalized;
        transform.forward *= maxAccel;

        if (Vector3.Distance(transform.position, target.position) <= 0.8f)
        {
            GetNextWaypoint();
        }
        enemy.speed = enemy.startSpeed;
    }

    void GetNextWaypoint()
    {
        if (wavepointIndex >= Waypoints.waypoints.Length - 1)
        {
            EndPath();
        }
        wavepointIndex++;
        target = Waypoints.waypoints[wavepointIndex];
    }

    void EndPath()
    {
        PlayerStats.Lives--;
        WaveSpawner.Instance.EnemiesAlive--;
        Destroy(gameObject);
    }
}
