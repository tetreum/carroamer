using UnityEngine;
using Peque;

public class ESCPanel : MonoBehaviour {

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Player.Instance.freeze(Player.FreezeReason.ESCMenu);
    }

    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Player.Instance.unFreeze();
    }

    public void resume() {
        Menu.Instance.showPanel("PlayerPanel");
    }

    public void exitButton () {
        Application.Quit();
    }
}
