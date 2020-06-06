namespace Peque.NPC.Tasks
{
    public class Patrol : Task
    {
        Patrol () {
            taskName = GetType().Name;
            priority = Task.Priority.Low;
        }

        public override void start()
        {
            
            //subTasks.Add(grabItem(hambuga));
            //subTasks.Add(interactWithItem(fryingPan));
            //subTasks.Add(eat(fryingPan));

            startNextSubTask(null);
        }
    }
}
