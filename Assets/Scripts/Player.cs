using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace Peque { 

    public class Player : Person.Person
    {
        public static Player Instance;
        public enum FreezeReason
        {
            Driving = 1,
            Dialog = 2,
            ESCMenu = 3,
            ViewingInventory = 4,
            Looting = 5,
        }

        public Equipment equipment;

        [HideInInspector]
		public MSVehicleControllerFree currentVehicle;
        [HideInInspector]
        public FirstPersonController firstPersonController;

        private Camera cam;

        public bool isFrozen {
            get {
                return (freezeReason != null);
            }
        }
        public FreezeReason? freezeReason;

        private void Awake() {
            Instance = this;
            firstPersonController = GetComponent<FirstPersonController>();
            cam = GetComponentInChildren<Camera>();

            data = new Person.Data();
            data.id = 1;
            data.firstName = "Player";
            data.health = 100;
            data.strength = 10;
        }

		private void OnEnable() {
			Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
		}

		private void Update() {
			if (Input.GetKeyDown(KeyCode.Escape)) {
                Menu.Instance.showPanel("ESCPanel");
            }
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Tab)) {
                if(Cursor.visible) {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                } else {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            }
#endif
        }

        public void enterVehicle (MSVehicleControllerFree vehicle) {
            currentVehicle = vehicle;
            currentVehicle.EnterInVehicle();
            freeze(FreezeReason.Driving);
            cam.tag = "Untagged";
        }

        public void stopDriving () {
            currentVehicle.ExitTheVehicle();
            unFreeze();
            cam.tag = "MainCamera";
            if (currentVehicle.doorPosition[0].transform.position != currentVehicle.transform.position) {
                transform.position = currentVehicle.doorPosition[0].transform.position;
            } else {
                transform.position = currentVehicle.doorPosition[0].transform.position + Vector3.up * 3.0f;
            }
            currentVehicle = null;
        }

        public void freeze(FreezeReason reason) {
            freezeReason = reason;
            firstPersonController.enabled = false;

            switch (reason) {
                case FreezeReason.Driving:
                    cam.gameObject.SetActive(false);

                    // move character outside the map to avoid colliding against it
                    Vector3 currentPos = transform.position;
                    currentPos.y -= 100;
                    transform.position = currentPos;
                    break;
            }
        }

        public void unFreeze(FreezeReason reason) {
            unFreeze();
        }

        public void unFreeze() {
            freezeReason = null;
            try {
                firstPersonController.enabled = true;
                cam.gameObject.SetActive(true);
            } catch { }
        }
    }
}