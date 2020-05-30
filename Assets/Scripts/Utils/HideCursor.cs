using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Shitty script used to record background menu videos
*/

public class HideCursor : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
