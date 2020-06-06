using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Notifies nearset NPCs that a sound has been triggered
namespace Peque.NPC {

    [RequireComponent(typeof(BoxCollider))]
    public class SoundNotificator : MonoBehaviour {

        private bool inited = false;

        private Vector3 originPosition;
        private string soundName;

        private List<int> notifiedPersons = new List<int>();

        public void init (string name, float duration, Vector3 position, int distance) {
            soundName = name;
            originPosition = position;

            transform.localScale.Set(distance, 2, distance);
            inited = true;
            StartCoroutine(selfDestroy(duration));
        }

        private void OnTriggerStay(Collider other) {
            if (!inited) {
                return;
            }

            var person = other.GetComponent<Person.Person>();

            if (!person || !person.isNPC || notifiedPersons.Contains(person.id)) {
                return;
            }

            notifiedPersons.Add(person.id);
            person.getNPCController().OnHearSound(soundName, originPosition);
        }

        IEnumerator selfDestroy (float duration) {
            yield return new WaitForSeconds(duration);
            Destroy(gameObject);
        }
    }
}