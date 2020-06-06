using UnityEngine;

namespace Peque {

    public class ObjectInteractor : MonoBehaviour {

        public Transform startPosition;
        public Transform endPosition;
        public delegate void FinishCallback();

        private FinishCallback finishCallback;

        public void stopInteraction () {

        }

        void OnFinishObjectInteraction () {
            finishCallback();
        }
    }
}