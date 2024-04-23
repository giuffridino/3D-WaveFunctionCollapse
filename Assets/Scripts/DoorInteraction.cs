using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public void OpenDoor()
    {
        StartCoroutine(RotateDoor());
    }
    
    public void CloseDoor()
    {
        Debug.Log("Door closed!");
    }

    IEnumerator RotateDoor()
    {
        var doorRotation = transform.rotation.eulerAngles;
        
        yield return new WaitForSeconds(0.05f);
    }
}
