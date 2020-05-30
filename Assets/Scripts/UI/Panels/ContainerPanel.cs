using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Peque;
using Peque.Inventory;
using Peque.UI;

public class ContainerPanel : MonoBehaviour {

    public GameObject prefab;
    public Transform listContainer;

    public Button button1;
    public Text button1Text;
    public Image button1Loader;
    public Image button1KeyImage;

    public Button button2;
    public Text button2Text;
    public Image button2Loader;
    public Image button2KeyImage;

    public Sprite interactKey;
    public Sprite dropKey;

    public enum Mode
    {
        Container = 1,
        Inventory = 2,
        Stash = 3,
    }

    private Mode currentMode;
    Player.FreezeReason freezeReason;

    [HideInInspector]
    public int selectedIndex {
        get {
            return _selectedIndex;
        }
        set {
            _selectedIndex = value;

            if (currentMode == Mode.Inventory) {
                setMode(currentMode); // refresh shown buttons according to the selected item
            }
        }
    }
    private int _selectedIndex;
    public static ContainerPanel Instance;

    public ItemData selectedItem {
        get {
            return spawnedItems[selectedIndex].data;
        }
    }

    private List<ContainerItem> spawnedItems = new List<ContainerItem>();
    private Dictionary<string, ContainerButton> buttonList = new Dictionary<string, ContainerButton>();

    private void OnEnable() {
        Instance = this;
        Player.Instance.freeze(freezeReason);
        Cursors.setFree();

        StartCoroutine(selectFirstItem());
    }

    // Unity hack as Select() method doesn't work in OnEnable
    IEnumerator selectFirstItem () {
        yield return new WaitForSeconds(0.1f);
        
        try {
            spawnedItems[0].GetComponent<Button>().Select();
        } catch (Exception) { } // maybe the container its empty
    }

    private void OnDisable() {
        Player.Instance.unFreeze(freezeReason);
        Cursors.setLocked();
    }

    private void Update() {
        try {
            foreach (ContainerButton button in buttonList.Values) {
                if (!button.hasLongPress || !button.isPressed) {
                    continue;
                }
                if (button.loader.fillAmount < 1) {
                    button.loader.fillAmount = Mathf.Lerp(0, 100, button.t);
                    button.t += 0.01f * Time.deltaTime;
                } else if (button.fillsRequirements()) {
                    // reset fill values or onLongPress will be executed multiple times
                    button.loader.fillAmount = 0;
                    button.t = 0.0f;

                    button.onLongPress();
                    break;
                }
            }
        } catch (Exception) {}
    }

    public void setPressingButton (string name, bool isPressing) {
        try {
            if (!buttonList.ContainsKey(name) || buttonList[name].isPressed == isPressing) {
                return;
            }

            if (!isPressing) {
                // detect if player made a fast key down & key up
                if (buttonList[name].loader.fillAmount < 0.15f && buttonList[name].hasShortPress) {
                    buttonList[name].onShortPress();
                }
                buttonList[name].loader.fillAmount = 0;
                buttonList[name].t = 0.0f;
            }

            buttonList[name].isPressed = isPressing;
        } catch (Exception) {}
    }

    public void init(Mode mode, ItemData[] items) {
        setMode(mode);
        setItems(items);
    }
    
    private void setMode (Mode mode) {
        currentMode = mode;

        button1.onClick.RemoveAllListeners();
        button2.onClick.RemoveAllListeners();
        buttonList.Clear();

        button1.gameObject.SetActive(true);
        button2.gameObject.SetActive(true);
        button1KeyImage.sprite = interactKey;
        button2KeyImage.sprite = interactKey;

        ContainerButton button;

        switch (mode) {
            case Mode.Inventory:
                freezeReason = Player.FreezeReason.ViewingInventory;

                if (spawnedItems.Count == 0) {
                    button1Text.text = "Error";
                    button2Text.text = "Error";
                    return;
                }

                // refresh shown buttons according to the selected item
                switch (selectedItem.type) {
                    case ItemType.Consumable:
                        button1Text.text = "Eat";
                        button1Loader.transform.parent.gameObject.SetActive(true);

                        button = new ContainerButton();
                        button.key = "Interact";
                        button.loader = button1Loader;
                        button.onLongPress = () => {
                            eatSelectedItem();
                        };
                        button.fillsRequirements = () => {
                            return true;
                        };
                        buttonList.Add(button.key, button);
                        break;
                    case ItemType.Weapon:
                    case ItemType.Cloth:
                        button1Text.text = "Equip";
                        button1Loader.transform.parent.gameObject.SetActive(false);

                        button = new ContainerButton();
                        button.loader = button1Loader;
                        button.key = "Interact";
                        button.onShortPress = () => {
                            equipSelectedItem();
                        };
                        buttonList.Add(button.key, button);
                        break;
                    case ItemType.Useless:
                    default:
                        button1.gameObject.SetActive(false);
                        break;
                }

                button2Text.text = "Drop";
                button2KeyImage.sprite = dropKey;
                button2Loader.transform.parent.gameObject.SetActive(true);

                button = new ContainerButton();
                button.key = "Drop";
                button.loader = button2Loader;
                button.onLongPress = () => {
                    dropSelectedItem();
                };
                button.fillsRequirements = () => {
                    return true;
                };
                buttonList.Add(button.key, button);

                break;
            case Mode.Container:
                freezeReason = Player.FreezeReason.Looting;

                button1.onClick.AddListener(() => {
                    stealSelectedItem();
                });
                button1Text.text = "Steal";
                button1Loader.transform.parent.gameObject.SetActive(false);

                button2.onClick.AddListener(() => {
                    stealAll();
                });
                button2Text.text = "Steal all";
                button2Loader.transform.parent.gameObject.SetActive(true);

                button = new ContainerButton();
                button.key = "Interact";
                button.loader = button2Loader;
                button.onShortPress = () => {
                    stealSelectedItem();
                };
                button.onLongPress = () => {
                    stealAll();
                };
                button.fillsRequirements = () => {
                    return spawnedItems.Count > 0;
                };
                buttonList.Add(button.key, button);
                break;
        }
    }

    public void setItems (ItemData[] items) {
        clearList();

        foreach (ItemData data in items) {
            var entry = (Instantiate(prefab, listContainer)).GetComponent<ContainerItem>();
            spawnedItems.Add(entry);

            entry.setData(data, spawnedItems.Count - 1);
            entry.gameObject.SetActive(true);
        }
    }

    public void orderByPrice () {
        Debug.Log("orderByPrice");
    }

    public void equipSelectedItem () {
        if (spawnedItems[selectedIndex] == null) {
            return;
        }

        Debug.Log("equipSelectedItem");

        removeSelectedItem();
    }

    public void dropSelectedItem() {
        if (spawnedItems[selectedIndex] == null) {
            return;
        }

        Player.Instance.equipment.delItem(selectedItem.id, Player.Instance.equipment.getItem(selectedItem.id).quantity);

        removeSelectedItem();
    }

    public void eatSelectedItem () {
        if (spawnedItems[selectedIndex] == null) {
            return;
        }
        
        Player.Instance.equipment.delItem(selectedItem.id, Player.Instance.equipment.getItem(selectedItem.id).quantity);
        SoundManager.Instance.playEffect(SoundManager.Effect.Eat, gameObject);

        removeSelectedItem();
    }

    public void stealSelectedItem () {
        if (spawnedItems[selectedIndex] == null) {
            return;
        }

        Player.Instance.equipment.addItem(selectedItem);

        removeSelectedItem();
    }

    private void removeSelectedItem () {
        int index = selectedIndex;

        // Select the next item in list
        Button button = spawnedItems[index].GetComponent<Button>();
        Selectable selectable = button.FindSelectableOnDown();

        if (selectable.name.IndexOf("Prefab") == -1) {
            selectable = button.FindSelectableOnUp();

            if (selectable.name.IndexOf("Prefab") != -1) {
                selectable.Select();
            }
        } else {
            selectable.Select();
        }

        Destroy(spawnedItems[index].gameObject);
        
        if ((listContainer.childCount - 1) == 0) {
            Menu.Instance.showPanel("PlayerPanel");
        }
    }

    public void stealAll() {
        foreach (ContainerItem item in spawnedItems) {
            Player.Instance.equipment.addItem(item.data);
        }

        clearList();
        Menu.Instance.showPanel("PlayerPanel");
    }

    private void clearList () {
        if (spawnedItems == null || spawnedItems.Count == 0) {
            return;
        }

        foreach (ContainerItem item in spawnedItems) {
            try {
                Destroy(item.gameObject);
            } catch (Exception) {}
        }
        spawnedItems.Clear();
    }
}
