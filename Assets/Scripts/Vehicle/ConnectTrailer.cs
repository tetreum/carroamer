﻿using Peque;
using UnityEngine;

public class ConnectTrailer : SimpleInteractiveItem
{
    private void OnMouseDown() {
        if (!isNear) {
            return;
        }
        // its not linked
        HingeJoint hinge = gameObject.GetComponent<HingeJoint>();

        if (hinge == null) {
            return;
        }

        Destroy(hinge);
        toggleTrigger();
    }
    private void OnTriggerEnter(Collider other) {
        // its already linked or its an unexpected collision
        if (gameObject.GetComponent<HingeJoint>() != null || Player.Instance.currentVehicle == null) {
            return;
        }

        transform.position = Player.Instance.currentVehicle.transform.Find("TrailerConnectorPosition").transform.position;
        HingeJoint hinge = gameObject.AddComponent<HingeJoint>();

        hinge.limits = new JointLimits() {
            min = -90,
            max = 90
        };
        hinge.anchor = new Vector3(0, 0.5f, 0);
        hinge.axis = new Vector3(0, 1, 0);
        hinge.connectedBody = Player.Instance.currentVehicle.GetComponent<Rigidbody>();
        hinge.useLimits = true;

        toggleTrigger();
    }

    void toggleTrigger () {
        foreach (BoxCollider col in gameObject.GetComponents<BoxCollider>()) {
            if (col.isTrigger) {
                col.enabled = !col.enabled;
                break;
            }
        }
        SoundManager.Instance.playEffect(SoundManager.Effect.TrailerConnected, this.gameObject);
    }
}
