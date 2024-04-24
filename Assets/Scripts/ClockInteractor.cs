using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockInteractor : MonoBehaviour
{

    private AudioSource clockSound;
    private AudioSource pingSound;

    private PlayerMovement playerMovement;
    private UIManager ui;

    void Start()
    {
        ui = GameObject.Find("UIManager").GetComponent<UIManager>();
        clockSound = GameObject.Find("Clock").GetComponent<AudioSource>();
        pingSound = GameObject.Find("Ping").GetComponent<AudioSource>();
        playerMovement = GameObject.Find("PlayerController").GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!playerMovement.gameEnded)
        {
            ui.RemoveTime(2.0f);

            Debug.Log("Clock triggered!");
            clockSound.Play();
            pingSound.Play();
            Destroy(gameObject);
        }
    }
}
