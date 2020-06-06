using UnityEngine;

namespace Peque.NPC
{
    public class NPCBehaviour : Person.IBehaviour
    {
        /*
         Fired every minute for every person
         */
        public override void UpdatePersonStats (Person.Person person)
        {

        }

        /*
         Fired when an NPC doesnt have any task in queue
         */
        public override void OnIdle (Person.Person person)
        {

        }

        /*
         Fired when an NPC hears something
        */
        public override void OnHearSound(Person.Person person, string soundName, Vector3 position) {

            switch (soundName) {
                case "BreakGlass":
                    Debug.Log("Someone broke a glass inspect the area");
                    break;
            }
        }

        /*
         Fired when an NPC sees another person
        */
        public override void OnSeeAPerson(Person.Person person, Person.Person seenPerson) {

            // its just viewing the person who which is having the conversation
            if (person.isTalking && person.talked == seenPerson) {
                return;
            }

            // robber
            if (person.hasHouse && person.isInHisHouse() && person.house.isIn(seenPerson) && !person.knows(seenPerson)) {
                // is strong, fight the robber
                if (person.strength > 50) {    
                    person.getNPCController().addTask(new Tasks.Fight(seenPerson));
                } else {
                    // call the police
                }
            }
        }
    }
}


