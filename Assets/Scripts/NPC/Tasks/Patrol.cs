using UnityEngine;

namespace Peque.NPC.Tasks
{
    public class Patrol : Task
    {
        Transform[] waypoints;

        Patrol (Transform[] waypoints) {
            taskName = GetType().Name;
            priority = Task.Priority.Low;
            this.waypoints = waypoints;
        }

        public override void start()
        {
            foreach (Transform point in waypoints) {
                subTasks.Add(new Subtasks.GoToPosition(point));
            }

            startNextSubTask(null);
        }
    }
}
