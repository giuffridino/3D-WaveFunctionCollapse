using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineController : MonoBehaviour
{
    private static CoroutineController _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public new static void StopAllCoroutines()
    {
        if (_instance != null)
        {
            _instance.StopAllCoroutinesInternal();
        }
    }

    private void StopAllCoroutinesInternal()
    {
        StopAllCoroutines();
    }
}