namespace Peque.NPC.Subtasks
{
    public class ShowDialog : Subtask
    {
        string dialog;
        int npcId;

        public ShowDialog(string dialog, int npcId)
        {
            taskName = GetType().Name;
            this.dialog = dialog;
            this.npcId = npcId;
        }

        public override void start()
        {
            DialogPanel.Instance.showDialog(dialog, npc.id, npcId);
            parentTask.startNextSubTask(true);
        }

        public override void OnInterrumpt(Task task) {
            DialogPanel.Instance.close();
        }
    }
    
}
