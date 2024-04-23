using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockInteractor : MonoBehaviour
{
    
    private UIManager ui;

    void Start()
    {
        ui = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        ui.RemoveTime(5.0f);
        Destroy(gameObject);
    }
}
