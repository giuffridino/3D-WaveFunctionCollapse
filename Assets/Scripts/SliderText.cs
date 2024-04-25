using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderText : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI tm;
    void Update()
    {
        tm.text = gameObject.name + ": " + ((int)slider.value).ToString();
    }
}
