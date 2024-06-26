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
    [SerializeField] private TextMeshProUGUI bonusText;
    [SerializeField] private TextMeshProUGUI restartText;
    [SerializeField] private TextMeshProUGUI runText;
	[SerializeField] private GameObject loadingText;
	[SerializeField] private TextMeshProUGUI percentageText;
	[SerializeField] private GameObject menu;
    [SerializeField] private int secondsToFadeOutText = 20;
	[SerializeField] private WFC wfc;
	[SerializeField] private Slider X;
	[SerializeField] private Slider Y;
	[SerializeField] private Slider Z;

    private float _time;
    private float bonusTimer = 0f;
    private float _bonus = 0f;
	private float animationStartTime;
    
    private void Start()
    {
		WFC.OnCollapsed += ShowUI;
    }

    public void UpdateTimer()
    {
        _time += Time.deltaTime;
        bonusTimer -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(_time / 60f);
        int seconds = Mathf.FloorToInt(_time % 60f);
        string formattedTime = $"{minutes:00}:{seconds:00}";
        timerText.text = formattedTime;

        minutes = Mathf.FloorToInt(_bonus / 60f);
        seconds = Mathf.FloorToInt(_bonus % 60f);
        formattedTime = $"-{minutes:00}:{seconds:00}";
        bonusText.text = formattedTime;
		float lerp = Mathf.PingPong((animationStartTime - Time.time) * 2f, 1f);
        Color lerpedColor = Color.Lerp(Color.green, Color.white, lerp);
        bonusText.color = lerpedColor;
            
        if (bonusTimer <= 0f)
        {
            _bonus = 0f;
            bonusText.text = "";
        }
    }

	private void ShowUI()
	{
		loadingText.SetActive(false);
		restartText.text = "Press R to\nrestart!";
		runText.text = "Hold shift to run";
        StartCoroutine(FadeOutTexts());
		WFC.OnCollapsed -= ShowUI;
	}


	public void UpdateLoadingProgress(int percentage)
	{	
		percentageText.text = percentage.ToString() + "%";
	}


    public void RemoveTime(float timeToRemove)
    {
        _time -= timeToRemove;
        _bonus += timeToRemove;
        if (bonusTimer < 0f)
		{
            bonusTimer = 0f;
			animationStartTime = Time.time;
		}
        bonusTimer = 2.0f;
		
        
        if (_time < 0)
        {
            _time = 0;
        }
    }
    
    public void SetEndGameText()
    {
        
        restartText.gameObject.SetActive(false);
		bonusText.gameObject.SetActive(false);
		runText.gameObject.SetActive(false);
        
        int minutes = Mathf.FloorToInt(_time / 60f);
        int seconds = Mathf.FloorToInt(_time % 60f);
        timerText.text = $"You Won in\n{minutes:00}:{seconds:00} seconds!\nPress R to restart!";
        timerText.rectTransform.sizeDelta = new Vector2(432, 50);
        timerText.fontSize = 24;
            
        SetTextToCenter();
    }

    private IEnumerator FadeOutTexts()
    {
        for (float t = 0; t < 1; t += Time.deltaTime / secondsToFadeOutText)
        {
            Color color = restartText.color;
            color.a = Mathf.Lerp(1, 0, t);
            restartText.color = color;
            runText.color = color;
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

	public void StartGame()
	{
		menu.SetActive(false);
		loadingText.SetActive(true);
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		wfc.dimX = (int)X.value;
		wfc.dimY = (int)Y.value;
		wfc.dimZ = (int)Z.value;
		
	}

}
