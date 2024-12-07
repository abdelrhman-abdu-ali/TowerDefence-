using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonActions : MonoBehaviour
{
    public void OnPlayButton()
    {
        //UIManager.Instance.LoadScene("Main");
        SceneManager.LoadScene("Main");
    }

    public void OnPauseButton()
    {
        UIManager.Instance.SwitchCanvas(UIManager.Instance.pauseMenuCanvas);
    }

    public void OnResumeButton()
    {
        UIManager.Instance.SwitchCanvas(UIManager.Instance.gameplayCanvas);
    }

    public void OnMainMenuButton()
    {
        UIManager.Instance.SwitchCanvas(UIManager.Instance.mainMenuCanvas);
        UIManager.Instance.LoadScene("MainMenuScene");
    }

    public void OnQuitButton()
    {
        UIManager.Instance.QuitGame();
    }
}

