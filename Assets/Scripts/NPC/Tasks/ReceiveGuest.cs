namespace Peque.NPC.Tasks
{
    public class ReceiveGuest : Task
    {
        int guestId;

        public ReceiveGuest(int guestId)
        {
            taskName = GetType().Name;
            priority = Task.Priority.High;
            this.guestId = guestId;
        }

        public override void start ()
        {
            ObjectInteractor door = npc.house.getItem(House.Item.MainDoor);

            subTasks.Add(new Subtasks.GoToPosition(door.endPosition));
            subTasks.Add(new Subtasks.InteractWithItem(door));

            string dialog;

            if (npc.knows(guestId)) {
                dialog = "OpenDoorUnexpected";
            } else {
                dialog = "OpenDoorUnkownVisitor";
            }
            
            subTasks.Add(new Subtasks.ShowDialog(dialog, guestId));
            subTasks.Add(new Subtasks.StopItemInteraction(door));


            startNextSubTask(null);
        }
    }
}
