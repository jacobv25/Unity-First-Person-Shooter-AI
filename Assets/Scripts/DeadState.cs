public class DeadState : State
{
    public DeadState(NPC npc) : base(npc) { }

    public override void Enter()
    {
        // NPC just died. Here you can perform any setup needed when the NPC dies.
        // For example, you can play a death animation or sound effect.
        
        npc.gameObject.SetActive(false); // For simplicity, we're just deactivating the NPC.
    }

    public override void Execute()
    {
        // NPC is dead, so there's nothing to update.
    }

    public override void Exit()
    {
        // NPC is dead, so it can't exit this state.
    }
}

