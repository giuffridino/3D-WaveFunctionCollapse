using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public void OpenDoor()
    {
        Debug.Log("Door opened!");
    }
    
    public void CloseDoor()
    {
        Debug.Log("Door closed!");
    }

    IEnumerator RotatingDoor()
    {
        yield return new WaitForSeconds(0.05f);
    }
}
