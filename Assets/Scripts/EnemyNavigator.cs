using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigator : MonoBehaviour
{
    Vector3 startingPoint;
    [SerializeField] float wanderRange;
    public NavMeshAgent navMesh { get; private set; }
    Rigidbody navRigidbody;
    // Start is called before the first frame update
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        navRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3 GetStartingPoint()
    {
        return new Vector3(transform.position.x,0,transform.position.y);
    }


    Vector3 GetRandomPointInRange(Vector3 startingPoint)
    {
        Vector3 offset = new Vector3(Random.Range(-wanderRange, wanderRange), 0, Random.Range(-wanderRange, wanderRange));

        NavMeshHit hit;

        bool gotPoint = NavMesh.SamplePosition(startingPoint + offset, out hit, 1, NavMesh.AllAreas);

        if (gotPoint)
        {
            return hit.position;
        }

        return Vector3.zero;
    }

    void RoamToPoint(Vector3 desiredPosition)
    {
        navMesh.SetDestination(desiredPosition);
    }
    [ContextMenu("Move")]
    public void MoveAgent()
    {
        RoamToPoint(GetRandomPointInRange(GetStartingPoint()));
    }

    public bool EndOfPath()
    {
        if (navMesh.remainingDistance < 0.1)
        {
            return true;
        }
        return false;
    }
    public void ChasePlayer(PlayerDetection player)
    {
        navMesh.SetDestination(player.transform.position);
    }
    public void AttackPlayer(PlayerDetection player)
    {
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        navRigidbody.AddForce((player.transform.position - transform.position).normalized * 15, ForceMode.Impulse);
    }
}
