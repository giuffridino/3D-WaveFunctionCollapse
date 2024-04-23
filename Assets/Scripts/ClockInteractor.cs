using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockInteractor : MonoBehaviour
{
    
    private PlayerMovement player;

    void Start()
    {
        player = GameObject.Find("PlayerController").GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        player.time -= 5.0f;
        gameObject.SetActive(false);
    }
}
