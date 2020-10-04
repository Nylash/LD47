using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance;

    public Canvas pauseCanvas = null;
    public Canvas victoryCanvas = null;

    public bool bLevelPaused = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void Pause()
    {
        bLevelPaused = !bLevelPaused;
        pauseCanvas.enabled = !pauseCanvas.enabled;
        if (!bLevelPaused)
            EventSystem.current.SetSelectedGameObject(null);
    }

    public void Victory()
    {
        bLevelPaused = true;
        victoryCanvas.enabled = true;
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
