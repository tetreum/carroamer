namespace Peque.NPC.Tasks
{
    public class Sleep : Task
    {
        public Sleep() {
            taskName = GetType().Name;
            priority = Task.Priority.Critical;
        }

        public override void start()
        {
            ObjectInteractor bed = npc.house.getItem(House.Item.Bed);

            subTasks.Add(new Subtasks.GoToPosition(bed.startPosition));
            subTasks.Add(new Subtasks.InteractWithItem(bed));
            
            startNextSubTask(null);
        }
    }
}
