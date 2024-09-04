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
        EventManager.TriggerEvent(Constants.EventNames.REFRESH_COIN, null);
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

    public void GameOver()
    {
        _gameActiveUI.SetActive(false);
        _inputPanel.SetActive(false);
        _gameEndUI.SetActive(true);
        EventManager.TriggerEvent(Constants.EventNames.GAME_OVER, null);
        EventManager.TriggerEvent(Constants.EventNames.UPDATE_ITEM, null);
    }

    public void AddCoins()
    {
        PlayerPrefs.SetInt(Constants.Data.COIN, 10000);
        EventManager.TriggerEvent(Constants.EventNames.REFRESH_COIN, null);
    }

    public void AddScore()
    {
        PlayerPrefs.SetInt(Constants.Data.SCORE, 10000);
        EventManager.TriggerEvent(Constants.EventNames.REFRESH_SCORE, null);
    }

    public void OpenStore()
    {
        _storePanel.SetActive(true);
    }

    public void CloseStore()
    {
        _storePanel.SetActive(false);
    }
}

[System.Serializable]
public struct UpgradeData
{
    public string type;
    public int level;
    public int cost;
    public float value;
}
