using UnityEngine;
using UnityEngine.AI;

public class WanderState : State
{
    private float wanderRadius;
    private NavMeshAgent agent;

    public float WanderRadius { get { return wanderRadius; } }


    public WanderState(NPC npc, float wanderRadius) : base(npc) 
    { 
        this.wanderRadius = wanderRadius;
        agent = npc.GetComponent<NavMeshAgent>();
    }

    public override void Enter()
    {
        // set destination to a random point in the NavMesh
        agent.SetDestination(RandomNavmeshLocation(wanderRadius));
    }

    public override void Execute()
    {
        // if the NPC is close to the destination, set a new destination
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            agent.SetDestination(RandomNavmeshLocation(wanderRadius));
        }
    }

    public override void Exit()
    {
        // stop the agent from moving when exiting the WanderState
        agent.isStopped = true;
    }

    private Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += npc.transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;            
        }
        return finalPosition;
    }
}
