using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenuScene")
        {  // UIManager.Instance.SwitchCanvas(UIManager.Instance.mainMenuCanvas);
        }
        else if (scene.name == "Main")

        {// UIManager.Instance.SwitchCanvas(UIManager.Instance.gameplayCanvas);
        }
    }
}