using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private GameObject _pauseButton, _pausePanel, _gameActiveUI, _startButton,
        _gameEndUI, _storePanel, _inputPanel;

    public GameObject _player;

    public bool hasGameStarted;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        hasGameStarted = false;
        _startButton.SetActive(true);
        _gameActiveUI.SetActive(true);
        _gameEndUI.SetActive(false);
        _pausePanel.SetActive(false);
        _inputPanel.SetActive(false);
        _storePanel.SetActive(false);
    }

    public void StartGame()
    {
        hasGameStarted = true;
        _startButton.SetActive(false);
        _inputPanel.SetActive(true);
        EventManager.TriggerEvent(Constants.EventNames.GAME_START, null);
    }

    public void OpenPausePanel()
    {
        _inputPanel.SetActive(false);
        _pausePanel.SetActive(true);
        _pauseButton.SetActive(false);
        Time.timeScale = 0f;
    }

    public void ClosePausePanel()
    {
        _inputPanel.SetActive(true);
        _pausePanel.SetActive(false);
        _pauseButton.SetActive(true);
        Time.timeScale = 1f;
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("GamePlay");
    }
}
