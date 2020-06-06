namespace Peque.NPC
{
    public abstract class Subtask
    {
        public NPCController npc;
        public string taskName;
        public Task parentTask;

        public abstract void start();
        public virtual void OnEachFrame() {}
        public abstract void OnInterrumpt(Task task);
    }
}