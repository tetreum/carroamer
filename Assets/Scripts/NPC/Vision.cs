using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Peque.Person;

namespace Peque.NPC
{
    public class Vision : MonoBehaviour
    {
        public Transform face;

        private Person.Person npc;
        private List<Person.Person> seenPersons = new List<Person.Person>();

        private void Awake() {
            npc = GetComponentInParent<Person.Person>();
        }

        private void OnTriggerStay(Collider coll) {
            var seenGuy = coll.GetComponent<Person.Person>();

            if (seenGuy && !seenPersons.Contains(seenGuy)) {
                seenPersons.Add(seenGuy);
                npc.getNPCController().OnSeeAPerson(seenGuy);
            }

            // this part is not working (hasDirectView doesnt see player even if they are in front)
            /*
            Debug.Log("Got a collission " + (seenGuy != null) + hasDirectView(coll.transform));
            if (!hasDirectView(coll.transform)) {
                return;
            }

            var seenGuy = coll.GetComponent<Person.Person>();

            // has seen another npc
            if (seenGuy) {
                Debug.Log("Seen " + seenGuy.id);
                if (!npc.knows(seenGuy)) {

                }
            }*/
        }

        private void OnTriggerExit(Collider coll) {
            var seenGuy = coll.GetComponent<Person.Person>();

            if (seenGuy) {
                seenPersons.Remove(seenGuy);
            }
        }

        private bool hasDirectView (Transform obj) {
            Vector3 direction = (obj.position - face.position);
            RaycastHit hit;

            bool gotHit = (Physics.Raycast(face.position, direction.normalized, out hit));
            if (!gotHit) {
                return false;
            }
            Debug.DrawRay(face.position, direction, Color.blue, 60);
            Debug.Log(hit.collider.gameObject.name);
            return (hit.collider.gameObject.name == obj.name);
            /*
            bool gotHit = (Physics.Raycast(transform.position, (obj.position - transform.position), out hit));
            if (!gotHit) {
                return true;
            }
            Debug.DrawRay(transform.position, (obj.position - transform.position), Color.blue, 60);
            Debug.Log(hit.collider.gameObject.name);
            return (hit.collider.gameObject.name == obj.name);
            /*Ray ray = Camera.main.ScreenPointToRay(transform.position);

            RaycastHit hit;
            return obj.Raycast(ray, out hit, 100.0F);*/
        }
    }
}

