using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DPUtils.Systems.SaveSystem;


public class MainMenuScript : MonoBehaviour
{
    public SaveGameScript saveGameScript;

    public void PlayGame()
    {
        // This will load a scene based on the next scene available in the build index
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ContinueGame()
    {
        saveGameScript.ContinueGame();
    }
    public void QuitGame()
    {
        Application.Quit();
    }

}
