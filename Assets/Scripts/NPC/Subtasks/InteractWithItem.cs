namespace Peque.NPC.Subtasks
{
    public class InteractWithItem : Subtask
    {
        public ObjectInteractor item;

        public InteractWithItem (ObjectInteractor item) {
            taskName = GetType().Name;
            this.item = item;
        }

        public override void start()
        {
            lookAt();
        }

        public override void OnInterrumpt(Task task) {
        }

        void lookAt ()
        {
            var lookPos = item.transform.position;
            lookPos.y = npc.transform.position.y;
            npc.transform.LookAt(lookPos);
        }
    }


}
