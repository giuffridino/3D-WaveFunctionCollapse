using System;
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
        float rotationSpeed = 90f;
        float rotationDuration = 3f;
        Quaternion targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 90, 0);
        float stepDuration = rotationDuration / rotationSpeed;
        int steps = Mathf.RoundToInt(rotationDuration / stepDuration);
        for (int i = 0; i < steps; i++)
        {
            float stepRotation = Quaternion.Lerp(transform.rotation, targetRotation, (float)i / steps).eulerAngles.y;
            transform.rotation = Quaternion.Euler(0, stepRotation, 0);
            yield return new WaitForSeconds(stepDuration);
        }
        transform.rotation = targetRotation;
    }
}
