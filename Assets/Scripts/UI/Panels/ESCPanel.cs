﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Peque;

public class ESCPanel : MonoBehaviour {

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Player.Instance.freeze(Player.FreezeReason.ESCMenu);
    }

    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Player.Instance.unFreeze();
    }

    public void resume() {
        Menu.Instance.showPanel("PlayerPanel");
    }

    public void exitButton () {
        Application.Quit();
    }
}
