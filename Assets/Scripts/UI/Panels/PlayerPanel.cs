using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Peque;

public class PlayerPanel : MonoBehaviour {

    public static PlayerPanel Instance;

    public RawImage bloodEffect;
    public Text timeLabel;
    public GameObject overweightIcon;

    void Awake () {
        Instance = this;
        InvokeRepeating("updateTime", 1, 60 * 12);
    }

    public void setHealth (int health) {
        Color bloodColor = bloodEffect.color;
        bloodColor.a = ((100 - health) / 100f);

        bloodEffect.color = bloodColor;
    }

    public void updateTime () {
        timeLabel.text = GameTime.Instance.currentDate.DayOfWeek.ToString() + " " + GameTime.Instance.currentDate.Day;
    }
}
