using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Peque.UI
{
    public class DialogItem : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        public string id;
        Text text;
        Color highlightedColor;
        Color normalColor;

        private void Awake() {
            text = GetComponentInChildren<Text>();
            highlightedColor = GetComponent<Button>().colors.highlightedColor;
            normalColor = GetComponent<Button>().colors.normalColor;
        }

        public void OnSelect(BaseEventData eventData) {
            text.color = highlightedColor;

            DialogPanel.Instance.possibleReplyId = id;
        }

        public void OnDeselect(BaseEventData data) {
            text.color = normalColor;
        }
    }
}
