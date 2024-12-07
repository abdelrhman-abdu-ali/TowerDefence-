
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] public Canvas mainMenuCanvas;
    [SerializeField] public Canvas gameplayCanvas;
    [SerializeField] public Canvas pauseMenuCanvas;

    private Canvas currentCanvas;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SwitchCanvas(mainMenuCanvas);
    }

    public void SwitchCanvas(Canvas targetCanvas)
    {
        if (currentCanvas != null)
            currentCanvas.enabled = false;

        currentCanvas = targetCanvas;
        currentCanvas.enabled = true;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }
}
