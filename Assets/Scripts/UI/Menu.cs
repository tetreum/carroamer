using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Peque
{
    public class Menu : MonoBehaviour
    {
        public List<GameObject> Menus;

        public static Menu _instance;
        public static Menu Instance
        {
            get
            {
                return _instance;
            }
        }

        void Awake()
        {
            _instance = this;
            SceneManager.sceneLoaded += OnLevelFinishedLoading;

            DontDestroyOnLoad(transform.gameObject);
        }

        public void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            switch (scene.name)
            {
                case "Main":
                    Cursor.lockState = CursorLockMode.None;
                    showPanel("MainPanel");

                    break;
                case "Map":
                case "Test":
                    showPanel("PlayerPanel");

                    Cursor.lockState = CursorLockMode.Locked;
                    Cursors.setCursor(Cursors.CType.Normal);

                    break;
                default:
                    hidePanel("MainPanel");
                    break;
            }
        }

        public GameObject getPanel(string name)
        {
            foreach (GameObject panel in Menus)
            {
                if (panel.name == name)
                {
                    return panel;
                }
            }
            throw new UnityException("UI Panel " + name + " not found");
        }

        public void togglePanel(string name)
        {
            GameObject panel = getPanel(name);

            panel.SetActive(!panel.activeSelf);
        }

        public GameObject showPanel(string name, bool hidePanels = true)
        {
            if (hidePanels)
            {
                hideAllPanels();
            }

            GameObject panel = this.getPanel(name);
            panel.SetActive(true);

            return panel;
        }

        public void hidePanel(string name)
        {
            foreach (GameObject panel in Menus)
            {
                try
                {
                    if (panel.name == name)
                    {
                        panel.SetActive(false);
                    }
                } catch { }
            }
        }

        public void hideAllPanels()
        {
            foreach (GameObject panel in Menus)
            {
                panel.SetActive(false);
            }
        }

        /*
         * We place this here since SmartphonePanel gets disabled when taking a screenshot
         */
        public void afterScreenshot(string file)
        {
            StartCoroutine(showUI(file));
        }

        IEnumerator showUI(string file)
        {
            yield return new WaitForSeconds(0.2f);

            Debug.Log("Screenshot saved as " + file);

            Menu.Instance.showPanel("SmartphonePanel", false);
            Menu.Instance.showPanel("PlayerPanel", false);
        }
    }
}