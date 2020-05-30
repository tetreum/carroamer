using UnityEngine;

namespace Peque {
    public class SimpleInteractiveItem : MonoBehaviour
    {
        [HideInInspector]
        public new Collider collider;
        public Cursors.CType cursorType = Cursors.CType.Handle;

        public bool isNear
        {
            get
            {
                Ray playerAim = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                RaycastHit hit;

                if (Physics.Raycast(playerAim, out hit, 2f) && hit.transform.GetInstanceID() == this.transform.GetInstanceID()) {
                    return true;
                }
                return false;
            }
        }

        void Awake() {
            collider = gameObject.GetComponent<Collider>();
        }

        public void OnMouseOver()
        {
            if (!enabled || collider.isTrigger) { return; }

            if (isNear) {
                Cursors.setCursor(cursorType);
            } else {
                Cursors.setCursor(Cursors.CType.Normal);
            }
        }

        public void OnMouseExit()
        {
            if (!enabled || collider.isTrigger) { return; }
            Cursors.setCursor(Cursors.CType.Normal);
        }
    }

}