using UnityEngine;

namespace Peque.NPC
{
    public class InteractionTrigger : MonoBehaviour
    {
        private Person.Person person;
        private void Awake() {
            person = GetComponentInParent<Person.Person>();
        }
        private void OnTriggerEnter(Collider other) {
            var nearPerson = other.GetComponent<Person.Person>();

            if (nearPerson == null) {
                return;
            }

            nearPerson.addNearPerson(person);
            person.addNearPerson(nearPerson);
        }
    }
}

