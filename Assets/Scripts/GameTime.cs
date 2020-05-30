using System;
using UnityEngine;

namespace Peque
{
    public class GameTime : MonoBehaviour
    {
        public static GameTime Instance;
        public float timeRatio = 0.08f; // 1 ingame day = 2 reallife hours
        public TimeSpan currentTime {
            get {
                return TimeSpan.FromSeconds(totalSeconds);
            }
        }
        public DateTime currentDate {
            get {
                return baseTime.Add(currentTime);
            }
        }
        private float totalSeconds = 0;
        private DateTime baseTime = new DateTime(2015, 01, 01);

        void Awake() {
            Instance = this;
        }

        public void Update() {
            totalSeconds += Time.deltaTime / timeRatio;
        }

        public void addMinutes (int minutes) {
            totalSeconds += minutes * 60;

            // if we artificially add time, update player screen too
            try {
                PlayerPanel.Instance.updateTime();
            } catch { }
        }

        public void addHours (int hours) {
            addMinutes(hours * 60);
        }

        public override string ToString() {
            return string.Format("{0} Days, {1:00}:{2:00}:{3:00}.{4:000}", currentTime.Days, currentTime.Hours, currentTime.Minutes, currentTime.Seconds, currentTime.Milliseconds);
        }
    }

}