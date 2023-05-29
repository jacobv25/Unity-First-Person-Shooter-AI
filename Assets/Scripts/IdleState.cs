using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    private float idleTimer;

    public IdleState(NPC npc) : base(npc)
    {
        this.npc = npc;
        idleTimer = npc.IdleTime;
    }

    public override void Enter()
    {
        // Set idle animation here. It depends on your animation system.
    }

    public override void Execute()
    {
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

        idleTimer -= Time.deltaTime;
        if (idleTimer <= 0)
        {
            npc.ChangeState(new WanderState(npc, npc.WanderRadius));
        }
    }


    public override void Exit()
    {
        // Clear up or reset anything before state switches.
    }
}

