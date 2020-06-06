using Peque.Person;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Peque.NPC {

    /**
    
    Vital Selector
    | - isHungry
    | - isHurt
    | - isSleepy

    Activity Selector
    | - isWorkingDay -> | - isWorkHour -> Go to work
		                | - !isWorkHour -> Make plans
    | - isFestiveDay -> | - Make plans

    Make plans
    | - isExcited -> | - HasCouple -> ...
		             | - !HasCouple -> Try to meet someone
    | - else ->
    */
    [RequireComponent(typeof(NavMeshAgent))]
    public class NPCController : Person.Person {
        
        override public int hungry
        {
            get {
                return data.hungry;
            }
            set {
                data.hungry = value;

                if (data.hungry < 10)
                {
                    Tasks.Eat task = new Tasks.Eat();
                    task.priority = Task.Priority.Critical;

                    addTask(task);
                } else if (data.hungry < 50) {
                    addTask(new Tasks.Eat());
                }
            }
        }
        public bool isFighting {
            get {
                return animator.GetBool("fighting");
            }
            set {
                animator.SetBool("fighting", value);
            }
        }

        public List<Task> pendingTasks = new List<Task>();
        public Task previousTask;
        public Task currentTask;
        
        [HideInInspector]
        public NavMeshAgent agent;

        private bool requestedTasks = false;

        private void Awake() {
            agent = GetComponent<NavMeshAgent>();
        }

        void Start () {
            // TMP @ToDo refactor/remove this
            //Transform spawn = GameObject.Find("Spawn1").transform;
            //GameObject npc = Instantiate(Instance.npcPrefabs[0], spawn.position, spawn.rotation);
            NPCController controller = GameObject.Find("Martin").GetComponent<NPCController>();
            Data data = new Data();
            data.id = 2;
            data.firstName = "Martin";
            data.health = 100;
            data.strength = 60;

            controller.setData(data);
            NPCManager.Instance.personList.Add(data.id, controller);

            NPCManager.Instance.personList.Add(1, Player.Instance);

            if (data == null) {
                Debug.LogError("NPC without data set");
            }

            animator.SetBool("Grounded", true);
            // temporal
            hungry = 30;
        }
	
	    void Update () {
            if (data == null) { return; }

            animator.SetFloat("VelocityZ", agent.velocity.magnitude);

		    if (currentTask != null) {
                currentTask.OnEachFrame();
            } else if (!requestedTasks) { // avoid calling OnIdle multiple frames while it's already deciding whats next
                requestedTasks = true;
                OnIdle();
                requestedTasks = false;
            }
	    }

        private void OnMouseDown() {
            addTask(new Tasks.Chat(Player.Instance.id));
        }

        public void OnIdle() {
            //ModManager.personBehaviour.OnIdle(this);
        }

        public void OnHearSound (string soundName, Vector3 position) {
            //ModManager.personBehaviour.OnHearSound(this, soundName, position);
        }

        public void OnSeeAPerson(Person.Person person) {
            //ModManager.personBehaviour.OnSeeAPerson(this, person);
        }

        public void addTask (Task task)
        {
            task.npc = this;

            if (currentTask == null || task.priority > currentTask.priority)
            {
                if (currentTask != null) {
                    currentTask.OnInterrumpt(task);
                    pendingTasks.Add(currentTask); // readd the interrumpted task to queue
                }

                // prepend task
                pendingTasks.Insert(0, task);
                startNextTask(null);
            } else {
                pendingTasks.Add(task);
            }
        }

        public void startNextTask(bool? finishStatus)
        {
            previousTask = currentTask;

            if (pendingTasks.Count == 0) {
                currentTask = null;
                return;
            }

            currentTask = pendingTasks[0];

            pendingTasks.RemoveAt(0);
            currentTask.start();
        }

        public bool hit(Person.Person person) {
            return weapon.hit(this, person);
        }
    }
}
