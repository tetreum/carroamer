using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Peque.NPC;

namespace Peque.Person
{
    public class Person : MonoBehaviour
    {
        protected Data data;
        public Transform face;
        public Animator animator {
            get {
                if (_animator == null) {
                    _animator = GetComponent<Animator>();
                }
                return _animator;
            }
        }
        private Animator _animator;

        public int id {
            get {
                return data.id;
            }
        }
        public bool isNPC {
            get {
                return (data.id != 1);
            }
        }
        public bool isPlayer {
            get {
                return (data.id == 1);
            }
        }
        public string firstName {
            get {
                return data.firstName;
            }
        }
        public string surnames {
            get {
                return data.surnames;
            }
        }
        public string fullName {
            get {
                return firstName + " " + surnames;
            }
        }
        public int age {
            get {
                return data.age;
            }
        }
        virtual public int health {
            get {
                return data.health;
            }
            set {
                data.health = Utils.sanitizeStatValue(value);
            }
        }
        public bool isDead {
            get {
                return (health < 1);
            }
        }
        virtual public int hungry {
            get {
                return data.hungry;
            }
            set {
                data.hungry = Utils.sanitizeStatValue(value);
            }
        }
        public int strength {
            get {
                return data.strength;
            }
            set {
                data.strength = Utils.sanitizeStatValue(value);
            }
        }
        public int stamina {
            get {
                return data.stamina;
            }
            set {
                data.stamina = Utils.sanitizeStatValue(value);

                if (isNPC) {
                    if (data.stamina == 0) { // sleep no matter where it is

                    } else if (data.stamina < 10) { // go to sleep
                        getNPCController().addTask(new NPC.Tasks.Sleep());
                    }
                }
            }
        }
        public int intelligence {
            get {
                return data.intelligence;
            }
            set {
                data.intelligence = Utils.sanitizeStatValue(value);
            }
        }

        public Weapons.Weapon weapon {
            get {
                return data.weapon;
            }
            set {
                data.weapon = value;
            }
        }

        private House _house;
        public House house {
            get {
                if (_house == null) {
                    _house = House.getById(data.houseId);
                }
                return _house;
            }
        }

        public bool hasHouse {
            get {
                return (data.houseId != 0);
            }
        }
        public bool isTalking {
            get {
                return (talked != null);
            }
        }
        public Person talked;
        public List<Person> nearPersons;

        public Data getData() {
            return data;
        }

        // should be fired ONLY ONCE, on start
        public void setData(Data npcData) {
            data = npcData;

            //House.getById(data.houseId).owner = this;
            InvokeRepeating("UpdatePersonStats", 60, 60);
        }

        public void addNearPerson (Person person) {
            try {
                nearPersons.Add(person);
            } catch { }
        }

        public void removeNearPerson(Person person) {
            try {
                nearPersons.Remove(person);
            } catch { }
        }

        public bool isNear(Person person) {
            return nearPersons.Contains(person);
        }

        public Feelings getFeelingsFor (Person person) {
            if (!data.feelings.ContainsKey(person.id)) {
                data.feelings.Add(person.id, new Feelings());
            }

            return data.feelings[person.id];
        }

        public bool isInContacts (Person person) {
            return data.contacts.ContainsKey(person.id);
        }

        public NPCController getNPCController () {
            return GetComponent<NPCController>();
        }

        // return if this npc knows npcId (spoke with him before)
        public bool knows(int npcId)  {
            return data.spokenDialogs.ContainsKey(npcId) && (data.spokenDialogs[npcId].asked.Count > 0 || data.spokenDialogs[npcId].replied.Count > 0);
        }

        public bool knows(Person person) {
            return knows(person.id);
        }

        public bool isInHisHouse () {
            if (!hasHouse) {
                return false;
            }
            return house.isIn(this);
        }

        public void setSpokenDialog(int npcId, string dialog, string reply)
        {
            if (!data.spokenDialogs.ContainsKey(npcId)) {
                data.spokenDialogs.Add(npcId, new DialogData(id, npcId));
            }

            data.spokenDialogs[npcId].asked.Add(dialog, reply);
        }

        public void getHit (Weapons.Weapon weapon, Person attacker) {
            health -= weapon.getDamage();
        }

        void UpdatePersonStats() {
            //ModManager.personBehaviour.UpdatePersonStats(this);
        }
    }
}


