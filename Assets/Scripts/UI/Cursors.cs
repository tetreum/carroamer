using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Peque;

public class Cursors : MonoBehaviour
{
    public RawImage currentCursor;
    public Texture2D[] cursors;
    public static Dictionary<string, Texture2D> indexedList = new Dictionary<string, Texture2D>();

    public enum CType
    {
        Handle,
        View,
        Normal,
        Kick,
        None,
        BreakGlass,
        Eat,
        Flash,
        Oiler,
        Plug,
        Refill,
        Shovel,
        Tape,
        OpenDoor,
        Drawer
    }

    public static Cursors Instance;

    public static bool isMovingMouse {
        get {
            return !(Input.GetAxis("Mouse X") == 0 && Input.GetAxis("Mouse Y") == 0);
        }
    }

    public void Awake () {
        if (Instance) {
            return;
        }

        foreach (Texture2D cursor in cursors) {
            indexedList.Add(cursor.name, cursor);
        }
        cursors = new Texture2D[0];

        Instance = this;
    }

    public static void setCursor (CType type)
    {
        if (type == CType.None) {
            Cursors.Instance.currentCursor.gameObject.SetActive(false);
            return;
        }

        Cursors.Instance.currentCursor.gameObject.SetActive(true);

        Cursors.Instance.currentCursor.texture = indexedList[type.ToString()];
    }

    public static void setFree(bool visible = true) {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = visible;
    }

    public static void setLocked() {
        Cursor.lockState = CursorLockMode.Locked;
        try {
            Player.Instance.StartCoroutine(disableCursor());
        } catch (System.Exception) {}
    }

    public static IEnumerator disableCursor() {
        yield return new WaitForEndOfFrame();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
