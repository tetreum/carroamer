namespace Peque.NPC.Subtasks
{
    public class StopItemInteraction : Subtask
    {
        private ObjectInteractor item;

        public StopItemInteraction(ObjectInteractor item) {
            taskName = GetType().Name;
            this.item = item;
        }

        public override void start() {
            item.stopInteraction();
            parentTask.startNextSubTask(true);
        }

        public override void OnInterrumpt(Task task) {
            item.stopInteraction();
        }
    }
}
