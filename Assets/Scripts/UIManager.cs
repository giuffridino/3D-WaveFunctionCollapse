using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI restartText;
    [SerializeField] private int secondsToFadeOutRestartText = 20;
    private float _time;
    private void Start()
    {
        SetTextToTopLeft();
        StartCoroutine(FadeOutRestartText());
    }

    public void UpdateTimer()
    {
        _time += Time.deltaTime;
        int minutes = Mathf.FloorToInt(_time / 60f);
        int seconds = Mathf.FloorToInt(_time % 60f);
        string formattedTime = $"{minutes:00}:{seconds:00}";
        timerText.text = formattedTime;
    }
    
    public void SetEndGameText()
    {
        Debug.Log("You Won in " + _time + " seconds! Press R to restart");
        
        restartText.gameObject.SetActive(false);
        
        int minutes = Mathf.FloorToInt(_time / 60f);
        int seconds = Mathf.FloorToInt(_time % 60f);
        timerText.text = $"You Won in\n{minutes:00}:{seconds:00} seconds!\nPress R to restart";
        timerText.rectTransform.sizeDelta = new Vector2(432, 50);
        timerText.fontSize = 24;
            
        SetTextToCenter();
    }

    private IEnumerator FadeOutRestartText()
    {
        for (float t = 0; t < 1; t += Time.deltaTime / secondsToFadeOutRestartText)
        {
            Color color = restartText.color;
            color.a = Mathf.Lerp(1, 0, t);
            restartText.color = color;
            yield return null;
        }
    }

    private void SetTextToCenter()
    {
        timerText.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        timerText.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        timerText.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        timerText.rectTransform.anchoredPosition = Vector2.zero;
    }
    
    private void SetTextToTopLeft()
    {
        timerText.rectTransform.anchorMin = new Vector2(0, 0.95f);
        timerText.rectTransform.anchorMax = new Vector2(0, 0.95f);
        timerText.rectTransform.pivot = new Vector2(0, 1);
        timerText.rectTransform.anchoredPosition = Vector2.zero;
        
        restartText.rectTransform.anchorMin = new Vector2(0.05f, 0.87f);
        restartText.rectTransform.anchorMax = new Vector2(0.05f, 0.87f);
        restartText.rectTransform.pivot = new Vector2(0, 1);
        restartText.rectTransform.anchoredPosition = Vector2.zero;
    }
}
