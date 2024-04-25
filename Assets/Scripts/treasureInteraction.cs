using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class treasureInteraction : MonoBehaviour
{
    [SerializeField] private AudioSource openTreasureSound;
    private bool rotatingChest = false;
    private int direction = 1;
    private Transform topChest;
    private Quaternion initialRotation;
    private Quaternion targetRotation;
    private float rotationSpeed = 90f; // Adjust rotation speed as needed
    private bool _triggered = false;

    private void Start()
    {
        topChest = transform.Find("chest_top");
        initialRotation = topChest.transform.rotation;
    }

    private void Update()
    {
        if (rotatingChest)
        {
            float step = rotationSpeed * Time.deltaTime;
            topChest.transform.rotation = Quaternion.RotateTowards(topChest.transform.rotation, targetRotation, step);

            if (topChest.transform.rotation == targetRotation)
            {
                rotatingChest = false;
            }
        }
    }

    private void OpenChest()
    {
        targetRotation = initialRotation * Quaternion.Euler(-90 * direction, 0, 0);
        rotatingChest = true;
        openTreasureSound.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag + " " + other.name);
        if (!_triggered && other.CompareTag("Player"))
        {
            _triggered = true;
            OpenChest();
            other.GetComponent<PlayerMovement>().gameEnded = true;
        }
    }
}

