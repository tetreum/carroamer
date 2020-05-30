using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Peque.Inventory;

namespace Peque
{
    public class Equipment : MonoBehaviour
    {
        const int MAX_WEIGHT_NAKED = 1;

        [HideInInspector]
        public float totalWeight = 0;

        public float maxWeight {
            get {
                if (bag == null) {
                    return MAX_WEIGHT_NAKED;
                }
                return bag.weight;
            }
        }
        public bool isOverweight {
            get {
                return totalWeight > maxWeight;
            }
        }
        public ItemData bag;
        public Dictionary<string, ItemData> items = new Dictionary<string, ItemData>();
        public CallBack onWeightChange;

        public delegate void CallBack();

        public void addItem (ItemData item) {
            if (items.ContainsKey(item.id)) {
                items[item.id].quantity += item.quantity;
            } else {
                items.Add(item.id, item);
            }
            updateTotalWeight();
        }

        public void delItem (string id, int quantity = 1) {
            if (!items.ContainsKey(id)) {
                return;
            }

            if (items[id].quantity == quantity) {
                items.Remove(id);
            } else {
                items[id].quantity -= quantity;
            }
            updateTotalWeight();
        }

        public ItemData getItem (string id) {
            if (!hasItem(id)) {
                return null;
            }
            return items[id];
        }

        public bool hasItem (string id) {
            return items.ContainsKey(id);
        }

        public void updateTotalWeight () {
            float total = 0;

            foreach (ItemData item in items.Values) {
                total += item.quantity * item.weight;
            }
            totalWeight = total;

            bool alreadyApplied = Player.Instance.firstPersonController.setOverWeightSpeed(isOverweight);

            if (alreadyApplied) {
                return;
            }

            PlayerPanel.Instance.overweightIcon.SetActive(isOverweight);
            if (onWeightChange != null) {
                onWeightChange();
            }
        }
    }
}