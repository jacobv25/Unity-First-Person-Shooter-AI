using UnityEngine;

public class RangedAttackState : State
{
    private float attackRange;
    private int attackDamage;

    public RangedAttackState(NPC npc, float attackRange, int attackDamage) : base(npc)
    {
        this.npc = npc;
        this.attackRange = attackRange;
        this.attackDamage = attackDamage;
    }

    public override void Enter()
    {
        // Do something when entering this state, like playing an attack animation.
    }

    public override void Execute()
    {
        if (npc.Target == null)
        {
            Debug.Log("Target is null!");
            // Switch to idle or wander state if there is no target
            npc.ChangeState(new IdleState(npc));
        }
        else
        {
            // Face the target
            Vector3 targetDirection = npc.Target.transform.position - npc.transform.position;
            float singleStep = npc.RotationSpeed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(npc.transform.forward, targetDirection, singleStep, 0.0f);
            newDirection.y = 0;  // Keep the NPC upright. Assumes NPC moves in the XZ plane.
            npc.transform.rotation = Quaternion.LookRotation(newDirection);

            // Check if the target is in attack range
            if (Vector3.Distance(npc.transform.position, npc.Target.transform.position) <= attackRange)
            {
                // Shoot the target
                npc.FireGun();
            }
            else
            {
                // Switch back to idle or wander state if the target is out of attack range
                npc.ChangeState(new IdleState(npc));
            }
        }
    }

    public override void Exit()
    {
        // Do something when exiting this state, like stopping an attack animation.
    }
}
