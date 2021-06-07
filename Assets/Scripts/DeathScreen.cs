using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    public CanvasGroup Curtain;

    public Button RestartButton;

    private PlayerDeath PlayerDeath;
    private bool playerDied;

    private void Awake()
    {
        RestartButton.onClick.AddListener(RestartGame);

        PlayerDeath = FindObjectOfType<PlayerDeath>();

        PlayerDeath.Happened += OnPlayerDied;
    }

    private void Update()
    {
        if (playerDied)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                RestartGame();
            }
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
        Curtain.alpha = 0;
        StartCoroutine(FadeOut());
    }

    public void Hide()
    {
        StartCoroutine(FadeIn());
    }

    private void OnPlayerDied()
    {
        playerDied = true;
        Show();
    }

    private IEnumerator FadeIn()
    {
        //yield return new WaitForSeconds(1f);
        while (Curtain.alpha > 0)
        {
            Curtain.alpha -= 0.03f;
            yield return new WaitForSeconds(0.03f);
        }
      
        gameObject.SetActive(false);
    }
    
    private IEnumerator FadeOut()
    {
        //yield return new WaitForSeconds(1f);
        while (Curtain.alpha < 1)
        {
            Curtain.alpha += 0.03f;
            yield return new WaitForSeconds(0.03f);
        }
        
    }

    private void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
