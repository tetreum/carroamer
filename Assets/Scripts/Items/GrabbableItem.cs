using UnityEngine;

namespace Peque {

    [RequireComponent(typeof(Rigidbody))]
    public class GrabbableItem : SimpleInteractiveItem
    {
        protected override float holdDistance {
            get { return 2f; }
        }
        void OnMouseDown() {
            if (!enabled || !isNear) {
                return;
            }

            Rigidbody rigid = GetComponent<Rigidbody>();
            rigid.useGravity = false;

            isHoldingIt = true;
        }
        private void Update() {
            if (!isHoldingIt) {
                return;
            }
            if (Input.GetMouseButtonUp(0)) {
                isHoldingIt = false;
                onRelease();
                return;
            } 
            if (Input.GetAxis("Mouse ScrollWheel") > 0) {
                transform.Rotate(Vector3.up * 0.5f, Space.Self);
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0) {
                transform.Rotate(Vector3.down * 0.5f, Space.Self);
            }
            holdItem();
        }

        private void onRelease() {
            Rigidbody rigid = GetComponent<Rigidbody>();
            rigid.useGravity = true;
        }
    }
}
