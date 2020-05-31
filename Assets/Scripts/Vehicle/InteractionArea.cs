
using UnityEngine;

namespace Peque.Vehicles
{
    public class InteractionArea : SimpleInteractiveItem
    {
        public Type type;
        protected override float holdDistance {
            get { return 2f; }
        }
        protected override float maxDistanceGrab {
            get { return 3f; }
        }

        public enum Type
        {
            Enter = 1,
            Exit = 2
        };

        void OnMouseDown() {
            if (!enabled || !isNear) {
                return;
            }

            var hinge = GetComponent<HingeJoint>();

            // close door
            if (Player.Instance.currentVehicle == null && !hinge.useSpring) {
                SoundManager.Instance.playEffect(SoundManager.Effect.LockCarDoor, gameObject);
                hinge.useSpring = true;
                return;
            }

            hinge.useSpring = false;

            if (Player.Instance.currentVehicle == null) {
                SoundManager.Instance.playEffect(SoundManager.Effect.OpenCarDoor, gameObject);
                isHoldingIt = true;
            } else {
                Player.Instance.stopDriving();
            }
        }

        private void Update() {
            if (!isHoldingIt) {
                return;
            }
            if (Input.GetMouseButtonUp(0)) {
                isHoldingIt = false;
            } else {
                holdItem();
            }
        }
    }
}


