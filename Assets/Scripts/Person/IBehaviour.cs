using UnityEngine;

namespace Peque.Person
{
    public abstract class IBehaviour {

        public abstract void UpdatePersonStats(Person person);

        public abstract void OnIdle(Person person);

        public abstract void OnHearSound(Person person, string soundName, Vector3 position);

        public abstract void OnSeeAPerson(Person person, Person seenPerson);
    }
}
