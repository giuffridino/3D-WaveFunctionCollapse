using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForPlayerEnter : MonoBehaviour
{
    private bool _triggered = false; // Keep track of whether the trigger has been activated

    private void OnTriggerEnter(Collider other)
    {
        if (!_triggered && other.CompareTag("Player"))
        {
            var door = GameObject.Find("Door");
            if (door != null)
            {
                door.GetComponent<DoorInteraction>().CloseDoor();
                _triggered = true; // Mark the trigger as activated
            }
        }
    }
}
