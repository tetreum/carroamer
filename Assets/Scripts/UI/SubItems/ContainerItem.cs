using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Peque.Inventory;

namespace Peque.UI
{
    public class ContainerItem : MonoBehaviour, ISelectHandler
    {
        public Text itemName;
        public Text price;
        public Text quantity;
        public Text weight;
        public RawImage icon;
        public ItemData data;

        private int index;

        public void setData (ItemData data, int index) {
            itemName.text = data.name;
            price.text = data.price.ToString();
            quantity.text = data.quantity.ToString();
            weight.text = data.weight.ToString();
            this.index = index;
            this.data = data;

            //Player.Instance.StartCoroutine(loadItemImage(data.id, icon));
        }

        /*
        private IEnumerator loadItemImage(string id, RawImage icon) {
            /*
            var localFile = new WWW("file://" + System.IO.Path.Combine(Application.streamingAssetsPath, "Items/Textures/" + id + ".png"));

            yield return localFile;

            icon.texture = localFile.texture;
            
        }
        */

        public void OnSelect(BaseEventData eventData) {
            setSelected();
        }

        public void setSelected () {
            ContainerPanel.Instance.selectedIndex = index;
        }
    }
}


