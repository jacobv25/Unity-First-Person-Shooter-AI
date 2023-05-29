using UnityEngine;
using UnityEngine.AI;

public class WanderState : State
{
    private float wanderRadius;
    private NavMeshAgent agent;
    private float wanderTimer;

    public float WanderRadius { get { return wanderRadius; } }


    public WanderState(NPC npc, float wanderRadius) : base(npc) 
    { 
        this.wanderRadius = wanderRadius;
        agent = npc.GetComponent<NavMeshAgent>();
        wanderTimer = npc.WanderTime;
    }

    public override void Enter()
    {
        agent.isStopped = false; //allow the agent to move
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
        // Check for enemies.
        Vector3 npcPosition = npc.transform.position;
        Vector3 startDirection = Quaternion.Euler(0, -npc.VisionAngle / 2, 0) * npc.transform.forward;
        Vector3 leftBoundary = Quaternion.Euler(0, npc.VisionAngle, 0) * startDirection;
        Vector3 rightBoundary = startDirection;

        RaycastHit hit;
        for (int i = 0; i < npc.NumberOfRays; i++)
        {
            float interpolationFactor = (float)(i + 1) / (npc.NumberOfRays + 1);
            Vector3 interpolatedDirection = Vector3.Slerp(rightBoundary, leftBoundary, interpolationFactor);
            Ray visionRay = new Ray(npcPosition, interpolatedDirection);
            
            if (Physics.Raycast(visionRay, out hit, npc.VisionDistance))
            {
                // Check if the hit object is an NPC
                NPC potentialEnemy = hit.transform.GetComponent<NPC>();
                if (potentialEnemy != null && npc.IsEnemy(potentialEnemy))
                {
                    // We see an enemy! Attack!
                    npc.Target = potentialEnemy; // Set the target.
                    Debug.Log(npc.Target.ToString());
                    npc.ChangeState(new RangedAttackState(npc, npc.AttackRange, npc.AttackDamage)); // Change to attack state.
                    return;
                }
            }
        }

        wanderTimer -= Time.deltaTime;
        if (wanderTimer <= 0)
        {
            npc.ChangeState(new IdleState(npc));
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
