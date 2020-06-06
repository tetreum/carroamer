using UnityEngine;

namespace Peque { 

    public class HouseSpaceController : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other) {
            var person = other.GetComponent<Person.Person>();
            if (person == null) {
                return;
            }

            transform.parent.gameObject.GetComponentInParent<House>().addGuest(person, gameObject.GetInstanceID());
        }

        private void OnTriggerExit(Collider other) {
            var person = other.GetComponent<Person.Person>();
            if (person == null) {
                return;
            }

            transform.parent.gameObject.GetComponentInParent<House>().removeGuest(person, gameObject.GetInstanceID());
        }
    }
}