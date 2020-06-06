namespace Peque.NPC.Tasks
{
    public class Eat : Task
    {
        public Eat() {
            taskName = GetType().Name;
            priority = Task.Priority.Low;
        }

        public override void start ()
        {
            if (!npc.hasHouse) {
                startNextSubTask(null);
                return;
            }
            ObjectInteractor fridge = npc.house.getItem(House.Item.Fridge);

            subTasks.Add(new Subtasks.GoToPosition(fridge.startPosition));
            subTasks.Add(new Subtasks.InteractWithItem(fridge));
            subTasks.Add(new Subtasks.StopItemInteraction(fridge));
            //subTasks.Add(grabItem(hambuga));
            //subTasks.Add(interactWithItem(fryingPan));
            //subTasks.Add(eat(fryingPan));

            startNextSubTask(null);
        }
    }
}
