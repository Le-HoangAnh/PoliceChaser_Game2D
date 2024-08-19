using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _openHelpPanel, _settingsPanel, _storePanel, _carPrefab;

    [SerializeField]
    private Image _settingsMusic, _settingsSound, _settingsShake;

    private void Awake()
    {
        _openHelpPanel.SetActive(false);
        _settingsPanel.SetActive(false);
        _storePanel.SetActive(false);
        SpawnAndSetCars();
    }

    private void SpawnAndSetCars()
    {
        for (int i = 0; i < 16; i++)
        {
            SpawnCar();
        }
    }

    private void SpawnCar()
    {

    }

    #region BUTTON_FUNCTION
    public void OpenFacebook()
    {
        Application.OpenURL("");
    }

    public void OpenInstagram()
    {
        Application.OpenURL("");
    }

    public void OpenHelpPanel()
    {
        _openHelpPanel.SetActive(true);
    }

    public void CloseHelpPanel()
    {
        _openHelpPanel.SetActive(false);
    }

    #region SETTINGS_BUTTON
    public void OpenSettingsPanel()
    {
        _settingsPanel.SetActive(true);
        SetSettingsButton(_settingsMusic, Constants.Settings.SETTINGS_MUSIC);
        SetSettingsButton(_settingsSound, Constants.Settings.SETTINGS_SOUND);
        SetSettingsButton(_settingsShake, Constants.Settings.SETTINGS_SHAKE);
    }

    public void ClickSwitchMusic()
    {
        SwitchSettingsButton(_settingsMusic, Constants.Settings.SETTINGS_MUSIC);
    }

    public void ClickSwitchSound()
    {
        SwitchSettingsButton(_settingsSound, Constants.Settings.SETTINGS_SOUND);
    }

    public void ClickSwitchShake()
    {
        SwitchSettingsButton(_settingsShake, Constants.Settings.SETTINGS_SHAKE);
    }

    public void CloseSettingsPanel()
    {
        _settingsPanel.SetActive(false);
    }
    #endregion

    public void OpenStorePanel()
    {
        _storePanel.SetActive(true);
    }

    public void CloseStorePanel()
    {
        _storePanel.SetActive(false);
    }

    public void PlayGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GamePlay");
    }

    #endregion

    #region HELPER_FUNCTION
    void SetSettingsButton(Image currentImage, string key)
    {
        int result;
        if (PlayerPrefs.HasKey(key))
        {
            result = PlayerPrefs.GetInt(key);
        }
        else
        {
            result = 1;
        }
        PlayerPrefs.SetInt(key, result);
        currentImage.color = result == 1 ? Color.white : Color.gray;
    }

    void SwitchSettingsButton(Image currentImage, string key)
    {
        int result;
        if (PlayerPrefs.HasKey(key))
        {
            result = PlayerPrefs.GetInt(key);
        }
        else
        {
            result = 1;
        }
        result = result == 1 ? 0 : 1;
        PlayerPrefs.SetInt(key, result);
        currentImage.color = result == 1 ? Color.white : Color.gray;
    }

    #endregion
}
