using UnityEngine;
using Peque;

public class ESCPanel : MonoBehaviour {

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (!Player.Instance.isFrozen) {
            Player.Instance.freeze(Player.FreezeReason.ESCMenu);
        }
    }

    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (Player.Instance.freezeReason != Player.FreezeReason.Driving) {
            Player.Instance.unFreeze();
        }
    }

    public void resume() {
        Menu.Instance.showPanel("PlayerPanel");
    }

    public void exitButton () {
        Application.Quit();
    }
}
