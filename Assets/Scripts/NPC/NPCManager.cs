using System.Collections.Generic;
using UnityEngine;

namespace Peque.NPC {
    public class NPCManager : MonoBehaviour
    {
        public static NPCManager Instance;
        public Dictionary<int, Person.Person> personList = new Dictionary<int, Person.Person>();

        private void Awake() {
            Instance = this;
        }

        public Person.Person getPerson(int id) {
            if (!personList.ContainsKey(id)) {
                Debug.LogError("NPCManager - Requested person " + id + " is not listed");
            }
            return personList[id];
        }

        public Person.Data getPersonData(int id) {
            if (!personList.ContainsKey(id)) {
                Debug.LogError("NPCManager - Requested person " + id + " is not listed");
            }
            return getPerson(id).getData();
        }
    }
}
