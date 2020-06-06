namespace Peque.NPC.Tasks
{
    public class Chat : Task
    {
        int npcId;
        public Chat(int npcId) {
            taskName = GetType().Name;
            priority = Task.Priority.High;
            this.npcId = npcId;
        }

        public override void start() {
            subTasks.Add(new Subtasks.ShowDialog("WhatDoYouWant", npcId));
        }
    }
}
