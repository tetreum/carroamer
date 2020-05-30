using System;
using UnityEngine;
using UnityEngine.UI;

namespace Peque.Inventory
{
    public class ItemData
    {
        public string id;
        public int quantity = 1;
        public string name {
            get {
                return id;
            }
        }
        private Item baseData {
            get {
                return null;
            }
        }
        public int price {
            get {
                return baseData.price;
            }
        }
        public float weight {
            get {
                return baseData.weight;
            }
        }
        public ItemType type {
            get {
                return (ItemType)Enum.Parse(typeof(ItemType), baseData.type.ToString());
            }
        }
    }

    public enum ItemType
    {
        Cloth = 1,
        Weapon = 2,
        Consumable = 3,
        Useless = 4,
        Container = 5
    }
}