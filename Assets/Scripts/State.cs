public abstract class State 
{
    protected NPC npc;

    public State(NPC npc) 
    {
        this.npc = npc;
    }

    public abstract void Enter();
    public abstract void Execute();
    public abstract void Exit();
}
