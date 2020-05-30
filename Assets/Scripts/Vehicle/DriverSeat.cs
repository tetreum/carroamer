using Peque;
using UnityEngine;

public class DriverSeat : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.transform.root.tag != "Player") {
            return;
        }
        Player.Instance.enterVehicle(transform.root.GetComponent<MSVehicleControllerFree>());
    }
}
