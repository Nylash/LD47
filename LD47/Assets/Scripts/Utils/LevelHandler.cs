using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelHandler : MonoBehaviour
{
    public Canvas mainCanvas;
    public Canvas levelSelectionCanvas;

    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }

    public void SwitchCanvas()
    {
        mainCanvas.enabled = !mainCanvas.enabled;
        levelSelectionCanvas.enabled = !levelSelectionCanvas.enabled;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
