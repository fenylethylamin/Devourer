using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    public string mainMenuScene;


public void Resume()
{
    GameManager.instance.PauseUnpause();
}

public void MainMenu()
{
    SceneManager.LoadScene(mainMenuScene);

    Time.timeScale = 1f;
}

public void QuitGame()
{
    Application.Quit();
    Debug.Log("Quitting Game");
}

}