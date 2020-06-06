using UnityEngine;
using UnityEngine.AI;

namespace Peque.NPC.Tasks
{
    public class Fight : Task
    {
        private float speed = 3;
        private NavMeshAgent agent;
        private Person.Person target;
        private Vector3? lastRequest;
        private float originalDistance;

        public Fight(Person.Person target) {
            taskName = GetType().Name;
            priority = Task.Priority.Critical;
            this.target = target;
        }

        public override void start() {
            agent = npc.gameObject.GetComponent<NavMeshAgent>();
            agent.speed = speed;
        }

        public override void OnEachFrame() {
            if (target.isDead) {
                npc.isFighting = false;
                startNextSubTask(true);
                return;
            }

            if (!npc.isFighting) {
                getNear();
            } else {
                npc.hit(target);
            }
        }

        public override void OnInterrumpt(Task task) {
            if (lastRequest != null && agent.gameObject.activeSelf) {
                agent.ResetPath();
            }
            lastRequest = null;
        }

        void getNear() {
            var pos = target.transform.position;

            if (lastRequest != pos) {
                if (agent.SetDestination(pos)) {
                    originalDistance = agent.remainingDistance;
                } else {
                    startNextSubTask(false);
                    return;
                }
            }

            lastRequest = pos;

            if (agent.remainingDistance <= agent.stoppingDistance) {
                npc.isFighting = true;
                agent.isStopped = true;
                agent.ResetPath();
                agent.isStopped = false;
            } else if (agent.remainingDistance < (originalDistance / 2)) {
                npc.isFighting = true;
            } else {
                npc.isFighting = false;
            }
        }
    }
}
