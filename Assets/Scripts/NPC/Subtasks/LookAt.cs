using UnityEngine;

namespace Peque.NPC.Subtasks
{
    public class LookAt : Subtask
    {
        new public string taskName = "LookAt";

        private Transform target;

        public LookAt(Transform target)
        {
            taskName = GetType().Name;
            this.target = target;
        }

        public override void start()
        {
            doLook();
        }

        public override void OnEachFrame()
        {
            doLook();
        }

        public override void OnInterrumpt(Task task)
        {
        }

        void doLook()
        {
            var lookPos = target.position;
            lookPos.y = npc.transform.position.y;
            npc.transform.LookAt(lookPos);

            parentTask.startNextSubTask(true);
        }
    }


}
