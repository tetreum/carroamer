using UnityEngine;
using UnityEngine.AI;

namespace Peque.NPC.Subtasks
{
    public class GoToPosition : Subtask
    {
        private float speed = 3;
        
        private Transform target;
        private Vector3? lastRequest;
        private NavMeshAgent agent;

        public GoToPosition(Transform target)
        {
            taskName = GetType().Name;
            this.target = target;
        }

        public override void start()
        {
            agent = npc.gameObject.GetComponent<NavMeshAgent>();
            agent.speed = speed;

            // target is an obstacle, increase the keepDistance (has obstacles have margin)
            /*if (target.gameObject.GetComponent<NavMeshObstacle>() != null) {
                keepDistance = 1.5f;
            }*/

            if ((agent.transform.position - target.position).magnitude < agent.stoppingDistance)
            {
                parentTask.startNextSubTask(true);
                return;
            }

            go();
        }

        public override void OnEachFrame() {
            go();
        }

        public override void OnInterrumpt(Task task)
        {
            if (lastRequest != null && agent.gameObject.activeSelf) {
                agent.ResetPath();
            }
            lastRequest = null;
        }

        void go()
        {
            var pos = target.position;

            if (lastRequest != pos)
            {
                if (!agent.SetDestination(pos))
                {
                    parentTask.startNextSubTask(false);
                    return;
                }
            }

            lastRequest = pos;

            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                agent.isStopped = true;
                agent.ResetPath();
                agent.isStopped = false;
                parentTask.startNextSubTask(true);
            }
        }
    }

	
}
