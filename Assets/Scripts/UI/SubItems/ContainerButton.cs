using UnityEngine.UI;

namespace Peque.UI
{
    public class ContainerButton
    {
        public string key;
        public bool hasLongPress {
            get {
                return !(onLongPress == null);
            }
        }
        public bool hasShortPress {
            get {
                return !(onShortPress == null);
            }
        }
        public bool isPressed = false;
        public float t = 0.0f;

        public Button button;
        public Text buttonText;
        public Image loader;

        public CheckCallBack fillsRequirements;
        public CallBack onLongPress;
        public CallBack onShortPress;

        public delegate void CallBack();
        public delegate bool CheckCallBack();
    }
}

