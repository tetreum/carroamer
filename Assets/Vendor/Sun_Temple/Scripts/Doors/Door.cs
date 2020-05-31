using Peque;
using System.Collections;
using UnityEngine;

namespace SunTemple
{
    public class Door : SimpleInteractiveItem
    {
		public bool IsLocked = false;
        public bool DoorClosed = true;
        public float OpenRotationAmount = 90;
        public float RotationSpeed = 1f;
        public float MaxDistance = 3.0f;
		private Collider DoorCollider;

        Vector3 StartRotation;
        float StartAngle = 0;
        float EndAngle = 0;
        float LerpTime = 1f;
        float CurrentLerpTime = 0;
        bool Rotating;

		void Start(){
            cursorType = Cursors.CType.OpenDoor;
            StartRotation = transform.localEulerAngles;
			DoorCollider = GetComponent<BoxCollider>();

			if (!DoorCollider) {
				Debug.LogWarning (this.GetType ().Name + ".cs on " + gameObject.name + "door has no collider", gameObject);
				return;
			}

			if (!Player.Instance) {
				Debug.LogWarning (this.GetType ().Name + ".cs on " + this.name + ", Player not found in Scene", gameObject);
				return;
			}
        }

		private void OnMouseDown() {
			if (!enabled || !isNear) {
				return;
			}

			if (IsLocked == false) {
				Activate();
			}
		}

		IEnumerator rotateDoor () {
			while (Rotating) {
				Rotate();
				yield return new WaitForEndOfFrame();
			}
		}

        public void Activate()
        {
			StopAllCoroutines();

            if (DoorClosed)
                Open();
            else
                Close();
        }

        void Rotate()
        {
            CurrentLerpTime += Time.deltaTime * RotationSpeed;
            if (CurrentLerpTime > LerpTime)
            {
                CurrentLerpTime = LerpTime;
            }

            float _Perc = CurrentLerpTime / LerpTime;

            float _Angle = CircularLerp.Clerp(StartAngle, EndAngle, _Perc);
            transform.localEulerAngles = new Vector3(transform.eulerAngles.x, _Angle, transform.eulerAngles.z);

			if (CurrentLerpTime == LerpTime) {
				Rotating = false;
				DoorCollider.enabled = true;
			}
        }

        void Open()
        {
            SoundManager.Effect[] effects = new SoundManager.Effect[] {
                SoundManager.Effect.OpenDoor1,
                SoundManager.Effect.OpenDoor2,
                SoundManager.Effect.OpenDoor3,
            };

            SoundManager.Instance.playEffect(effects[Random.Range(0, effects.Length)], this.gameObject);
            DoorCollider.enabled = false;
            DoorClosed = false;
            StartAngle = transform.localEulerAngles.y;
            EndAngle =  StartRotation.y + OpenRotationAmount;
            CurrentLerpTime = 0;
            Rotating = true;
			StartCoroutine(rotateDoor());
        }

        void Close()
        {
            SoundManager.Effect[] effects = new SoundManager.Effect[] {
                SoundManager.Effect.CloseDoor1,
                SoundManager.Effect.CloseDoor2,
                SoundManager.Effect.CloseDoor3,
            };

            SoundManager.Instance.playEffect(effects[Random.Range(0, effects.Length)], this.gameObject);
            DoorCollider.enabled = false;
            DoorClosed = true;
            StartAngle = transform.localEulerAngles.y;
            EndAngle = transform.localEulerAngles.y - OpenRotationAmount;
            CurrentLerpTime = 0;
            Rotating = true;
			StartCoroutine(rotateDoor());
		}
    }
}