using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StateScreens : MonoBehaviour
{
    public PlayerController playerController;
    public DetectionZone winZone;
    public TMP_Text pauseText;
    public TMP_Text gameOverText;
    public TMP_Text survivedText;
    public Button retryButton;

    private bool gamePaused = false;

    private void Awake()
    {
        // Ensure these references are assigned in the inspector
        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();
        }
    
        pauseText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        survivedText.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);

        retryButton.onClick.AddListener(OnRetry);
    }

    private void Update()
    {
        if (winZone.detectedColliders.Count > 0)
        {
            TriggerWin();
        }

        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            TogglePauseGame();
        }
    }

    private void OnEnable()
    {
        if (playerController != null)
        {
            playerController.OnPlayerDeath.AddListener(OnPlayerDeath);
            playerController.OnPlayerWon.AddListener(OnPlayerWon);
        }
    }

    private void OnDisable()
    {
        if (playerController != null)
        {
            playerController.OnPlayerDeath.RemoveListener(OnPlayerDeath);
            playerController.OnPlayerWon.RemoveListener(OnPlayerWon);
        }
    }

    private void OnPlayerDeath()
    {
        gameOverText.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);
    }

    private void OnPlayerWon()
    {
        survivedText.gameObject.SetActive(true);
        playerController.PausePlayerActions();
        Time.timeScale = 0f;
    }

    private void OnRetry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void TriggerWin()
    {
        if (playerController.OnPlayerWon != null)
        {
            playerController.OnPlayerWon.Invoke();
        }
    }
    
    private void TogglePauseGame()
    {
        gamePaused = !gamePaused;
        Time.timeScale = gamePaused ? 0f : 1f; // Toggle between freezing and resuming the game

        if (gamePaused)
        {
            pauseText.gameObject.SetActive(true);
            playerController.PausePlayerActions();
        }
        else
        {
            pauseText.gameObject.SetActive(false);
            playerController.ResumePlayerActions();
        }
    }
}