using System.Collections.Generic;
using UnityEngine;

namespace Peque.NPC
{
    public abstract class Task
    {
        public NPCController npc;
        public string taskName;
        public Task.Priority priority = Task.Priority.Low;
        public List<Subtask> subTasks = new List<Subtask>();
        public Subtask currentSubtask;

        public abstract void start();
        public virtual void OnEachFrame() {
            currentSubtask.OnEachFrame();
        }

        public virtual void OnInterrumpt(Task task) {
            Debug.Log(taskName);
            currentSubtask.OnInterrumpt(task);
        }

        public void startNextSubTask(bool? finishStatus)
        {
            if (subTasks.Count == 0) {
                npc.startNextTask(finishStatus);
                return;
            }
            currentSubtask = subTasks[0];
            subTasks.RemoveAt(0);

            currentSubtask.parentTask = this;
            currentSubtask.npc = npc;
            currentSubtask.start();
        }

        public enum Priority
        {
            Critical = 10,
            High = 8,
            Medium = 5,
            Low = 1,
        }
    }
}