using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _scoreText, _endScoreText, _totalScoreText, _scorePrefab, _coinsText;

    [SerializeField] private Slider _revengeSlider;

    [SerializeField] private List<Image> _boostHolders;

    [SerializeField] private List<Image> _healthHolders;

    [SerializeField] private GameObject _boostHolder;

    private float _scoreMultiplier;
    private float _score;
    private bool _hasGameFinished;

    private void Awake()
    {
        _score = 0;
        _scoreMultiplier = 32;
        _hasGameFinished = false;
        _revengeSlider.gameObject.SetActive(false);

        int tempScore = (int)_score;
        _scoreText.text = tempScore.ToString() + " PTS";
    }

    private void OnEnable()
    {
        EventManager.StartListening(Constants.EventNames.GAME_START, StartGame);
        EventManager.StartListening(Constants.EventNames.GAME_OVER, GameOver);
        EventManager.StartListening(Constants.EventNames.UPDATE_SCORE, UpdateScore);
        EventManager.StartListening(Constants.EventNames.UPDATE_COIN, UpdateCoin);
        EventManager.StartListening(Constants.EventNames.REFRESH_SCORE, RefreshScore);
        EventManager.StartListening(Constants.EventNames.REFRESH_COIN, RefreshCoin);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Constants.EventNames.GAME_START, StartGame);
        EventManager.StopListening(Constants.EventNames.GAME_OVER, GameOver);
        EventManager.StopListening(Constants.EventNames.UPDATE_SCORE, UpdateScore);
        EventManager.StopListening(Constants.EventNames.UPDATE_COIN, UpdateCoin);
        EventManager.StopListening(Constants.EventNames.REFRESH_SCORE, RefreshScore);
        EventManager.StopListening(Constants.EventNames.REFRESH_COIN, RefreshCoin);
    }

    private void StartGame(Dictionary<string, object> message)
    {
        StartCoroutine(StartCounting());
    }

    IEnumerator StartCounting()
    {
        while (!_hasGameFinished)
        {
            _score += _scoreMultiplier * Time.deltaTime;
            int tempScore = (int) _score;
            _scoreText.text = tempScore.ToString() + " PTS";
            yield return null;
        }
    }

    private void GameOver(Dictionary<string, object> message)
    {
        _hasGameFinished = true;
        int tempScore = (int)_score;
        _endScoreText.text = "SCORE " + tempScore.ToString() + " PTS";
        int totalScore = PlayerPrefs.HasKey(Constants.Data.SCORE) ? PlayerPrefs.GetInt(Constants.Data.SCORE) : 0;
        totalScore += tempScore;
        PlayerPrefs.SetInt(Constants.Data.SCORE, totalScore);
        _totalScoreText.text = totalScore.ToString() + " PTS";
    }

    private void UpdateScore(Dictionary<string, object> message)
    {
        var scoreType = message[Constants.ScoreMessage.TYPE].ToString();

        switch (scoreType)
        {
            case Constants.ScoreMessage.NORMAL_COLLISION:
                _score += 40;
                int tempScore = (int)_score;
                _scoreText.text = tempScore.ToString() + " PTS";

                Vector3 spawnPos = (Vector3)message[Constants.ScoreMessage.POSITION];
                var tempScoreText = Instantiate(_scorePrefab, spawnPos, Quaternion.identity);
                tempScoreText.text = "+40";
                Destroy(tempScoreText, 3f);
                break;

            default:
                break;
        }
    }

    private void UpdateCoin(Dictionary<string, object> message)
    {
        int coins = PlayerPrefs.HasKey(Constants.Data.COIN) ? PlayerPrefs.GetInt(Constants.Data.COIN) : 0;
        coins++;
        PlayerPrefs.SetInt(Constants.Data.COIN, coins);
        _coinsText.text = coins.ToString();
    }

    private void RefreshScore(Dictionary<string, object> message)
    {
        int score = PlayerPrefs.HasKey(Constants.Data.SCORE) ? PlayerPrefs.GetInt(Constants.Data.SCORE) : 0;
        _totalScoreText.text = score.ToString() + " PTS";
    }

    private void RefreshCoin(Dictionary<string, object> message)
    {
        int coins = PlayerPrefs.HasKey(Constants.Data.COIN) ? PlayerPrefs.GetInt(Constants.Data.COIN) : 0;
        _coinsText.text = coins.ToString();
    }
}
