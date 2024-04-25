using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    private bool rotatingDoor = false;
    private int direction = 1;
    private Quaternion initialRotation;
    private Quaternion targetRotation;
    private float rotationSpeed = 90f; // Adjust rotation speed as needed
    [SerializeField] private AudioSource openDoorSound;
    [SerializeField] private AudioSource closeDoorSound;

    private void Start()
    {
        initialRotation = transform.rotation;
    }

    private void Update()
    {
        if (rotatingDoor)
        {
            float step = rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);

            if (transform.rotation == targetRotation)
            {
                rotatingDoor = false;
            }
        }
    }

    public void OpenDoor()
    {
        rotationSpeed = 90f;
        targetRotation = initialRotation * Quaternion.Euler(0, 90 * direction, 0);
        rotatingDoor = true;
        openDoorSound.Play();
    }

    public void CloseDoor()
    {
        rotationSpeed = 360f;
        targetRotation = initialRotation;
        rotatingDoor = true;
        closeDoorSound.Play();
    }
}
