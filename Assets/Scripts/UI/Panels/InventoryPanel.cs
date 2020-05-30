using UnityEngine;
using UnityEngine.UI;
using Peque;
using Peque.Inventory;

public class InventoryPanel : MonoBehaviour {

    public ContainerPanel containerPanel;
    public Text weight;

    private void OnEnable() {
        ItemData[] items = new ItemData[Player.Instance.equipment.items.Count];
        int i = 0;

        foreach (ItemData item in Player.Instance.equipment.items.Values) {
            items[i] = item;
            i++;
        }

        containerPanel.init(ContainerPanel.Mode.Inventory, items);
        containerPanel.gameObject.SetActive(true);

        Player.Instance.equipment.onWeightChange = OnWeightChange;
        OnWeightChange();
    }

    private void OnDisable() {
        containerPanel.gameObject.SetActive(false);
        Player.Instance.equipment.onWeightChange = null;
    }

    void OnWeightChange () {
        float totalWeight = Player.Instance.equipment.totalWeight;
        float maxWeight = Player.Instance.equipment.maxWeight;
        
        weight.text = totalWeight + "/" + maxWeight;

        if (totalWeight > maxWeight) {
            weight.text = "<color=red>" + weight.text + "</color>";
        }
    }
}
