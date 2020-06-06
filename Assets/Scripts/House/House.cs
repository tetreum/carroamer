using System.Collections.Generic;
using UnityEngine;

namespace Peque
{
    public class House : MonoBehaviour {

        public int id;
        public ObjectInteractor[] items;
        public Dictionary<string, ObjectInteractor> itemList = new Dictionary<string, ObjectInteractor>();

        public Person.Person owner;

        private Dictionary<int, List<int>> personsInHouse = new Dictionary<int, List<int>>();

        public enum Item
        {
            Fridge = 1,
            MainDoor = 2,
            Bed = 3,
        }

        private void Awake()
        {
            foreach (ObjectInteractor item in items) {
                itemList.Add(item.name, item);
            }
        }

        public void addGuest (Person.Person person, int roomId) {
            if (!personsInHouse.ContainsKey(person.id)) {
                personsInHouse.Add(person.id, new List<int>());
            }
            personsInHouse[person.id].Add(roomId);

            if (person.isPlayer && personsInHouse[person.id].Count == 1) {
                SoundManager.Instance.lowerExternalAmbient();
            }
        }

        public void removeGuest(Person.Person person, int roomId) {
            if (!personsInHouse.ContainsKey(person.id)) {
                return;
            }
            personsInHouse[person.id].Remove(roomId);

            if (person.isPlayer && !isIn(person)) {
                SoundManager.Instance.increaseExternalAmbient();
            }
        }

        public bool isIn (Person.Person person) {
            return isIn(person.id);
        }

        public bool isIn(int personId) {
            return personsInHouse.ContainsKey(personId) && personsInHouse[personId].Count > 0;
        }

        private GameObject find (string name) {
            return GameObject.Find("House-" + id + "/Items/" + name);
        }

	    public ObjectInteractor getItem (Item name)
        {
            string stringName = name.ToString();

            if (itemList.ContainsKey(stringName)) {
                return itemList[stringName];
            }

            GameObject obj = find(stringName);

            if (obj == null) {
                Debug.Log("House-" + id + " - Item " + stringName + " not found");
                return null;
            }

            return obj.GetComponent<ObjectInteractor>();
        }

        public static House getById (int id) {
            return GameObject.Find("House-" + id).GetComponent<House>();
        }
    }
}
