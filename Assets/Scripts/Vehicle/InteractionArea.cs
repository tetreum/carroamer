
using UnityEngine;

namespace Peque.Vehicles
{
    public class InteractionArea : SimpleInteractiveItem
    {
        public Type type;

        private float distance = 2f;
        private float maxDistanceGrab = 3f;
        private bool holdingDoor = false;

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
                holdingDoor = true;
            } else {
                Player.Instance.stopDriving();
            }
        }

        private void Update() {
            if (holdingDoor) {

                if (Input.GetMouseButtonUp(0)) {
                    holdingDoor = false;
                }

                holdItem();
            }
        }

        private void holdItem() {
            Ray playerAim = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            Vector3 nextPos = Camera.main.transform.position + playerAim.direction * distance;
            Vector3 currPos = transform.position;

            GetComponent<Rigidbody>().velocity = (nextPos - currPos) * 10;

            if (Vector3.Distance(transform.position, Camera.main.transform.position) > maxDistanceGrab) {
                holdingDoor = false;
            }
        }
    }
}


