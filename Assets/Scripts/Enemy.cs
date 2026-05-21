using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform player;
    private Vector3 directionToPlayer;
    public float viewAngle = 120f;
    public float viewRange = 5f;
    public LayerMask playerLayer;
    public float detectionRadius = 0.5f;
    private NavMeshAgent agent;
    private bool patrolling = true;
    public Transform[] waypoints;
    private Transform targetWayPoint;
    private int wayPointIndex = 0;
    private bool playerFound = false;
    public float alertDuration = 20f;
    private float timeSinceAlerted = 0;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (GameManager.Instance.crawlersStarted)
        {
            targetWayPoint = waypoints[wayPointIndex];
            directionToPlayer = (player.position - transform.position).normalized;
            Quaternion rot = Quaternion.LookRotation(directionToPlayer);
            if (patrolling)
            {
                EnemyPatrolling();

            }
            if (PlayerDetected())
            {
                playerFound = true;
                patrolling = false;
                agent.SetDestination(player.position);
            }
            if (playerFound && !PlayerDetected())
            {
                if (timeSinceAlerted < alertDuration)
                {
                    agent.SetDestination(player.position);
                    timeSinceAlerted += Time.deltaTime;
                }
                else
                {
                    playerFound = false;
                    timeSinceAlerted = 0;
                    patrolling = true;
                }
            }
        }

    }

    private void EnemyPatrolling()
    {
        agent.SetDestination(targetWayPoint.position);
        float dist = Vector3.Distance(transform.position, targetWayPoint.position);
        float buffer = 0.3f;
        if (dist <= buffer)
        {
            wayPointIndex++;
            if (wayPointIndex >= waypoints.Length)
            {
                wayPointIndex = 0;
            }
        }
      
    }
    private bool PlayerDetected()
    {
        bool result = false;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        if (angle < viewAngle / 2)
        {
            if (Physics.Raycast(transform.position, directionToPlayer, viewRange, playerLayer))
            {
                result = true;
            }
        }

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= detectionRadius)
        {
            result = true;
        }
        return result;
    }
    
}
