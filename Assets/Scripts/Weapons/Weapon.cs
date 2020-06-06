using UnityEngine;

namespace Peque.Weapons
{
    public class Weapon : MonoBehaviour
    {
        public Type type;
        public int minDamage;
        public int maxDamage;
        public int maxDistance;
        public float hitsPerSecond;
        public bool animationFinished = true;
        private float timeSinceLastAttack;

        public enum Type
        {
            Hand = 1,
            Knife = 2,
            Gun = 3,
        }

        public Weapon (Type type, int minDamage, int maxDamage, int maxDistance, int hitsPerSecond = 4) {
            this.type = type;
            this.minDamage = minDamage;
            this.maxDamage = maxDamage;
            this.maxDistance = maxDistance;
            this.hitsPerSecond = hitsPerSecond;
        }

        public int getDamage () {
            return Random.Range(minDamage, maxDamage);
        }

        public bool hit(Person.Person attacker, Person.Person target) {

            if (Vector3.Distance(attacker.transform.position, target.transform.position) > maxDistance || target.isDead || !animationFinished) {
                return false;
            }

            timeSinceLastAttack += Time.deltaTime;

            if (timeSinceLastAttack < (1.0 / hitsPerSecond)) {
                return false;
            }

            if (type == Type.Hand && attacker.isNPC) {
                if (!attacker.getNPCController().isFighting) {
                    attacker.getNPCController().isFighting = true;
                }
                
                attacker.animator.SetTrigger("handHit");
            }

            target.getHit(this, attacker);
            timeSinceLastAttack = 0;

            return true;
        }
    }
}

