using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public CanvasGroup settingsPage;
    public GameObject loadGame;
    public AudioSource audioSource;
    private void Start()
    {
        Time.timeScale = 1.0f;
    }
    private void Update()
    {
        if(settingsPage.alpha == 1 && Keyboard.current.escapeKey.isPressed) settingsPage.alpha = 0;
    }
    public void NewGame()
    {
        settingsPage.alpha = 0;
        SaveManager.Instance.currentMode = SaveManager.GameMode.NewGame;
        SceneManager.LoadScene("SampleScene");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void OnSettings()
    {
        if (settingsPage.alpha == 1)
        {
            settingsPage.alpha = 0;
        }
        else {

            settingsPage.alpha = 1;
        }
    }

    public void OnLoadGame()
    {
        settingsPage.alpha = 0;
        audioSource.Pause();
        this.gameObject.SetActive(false);
        loadGame.SetActive(true);
    }

    private void OnEnable()
    {
        audioSource.UnPause();
    }

}
