using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Peque
{
    public class Game : MonoBehaviour
    {
        public static Game Instance;

        private void Awake() {
            Instance = this;
        }

        private void Update() {
            if (Input.GetKey(KeyCode.Escape)) {
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}