using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using DPUtils.Systems.SaveSystem;


public class MainMenuScript : MonoBehaviour
{
    public SaveGameScript saveGameScript;

    public GameObject PlayButton;
    public GameObject ContinueButton;


    void Update()
    {
        string saveFilePath = "Assets/SaveFiles/saveData.dat";

        if (File.Exists(saveFilePath))
        {
            ContinueButton.SetActive(true);
            PlayButton.SetActive(false);
        }
        else{
            ContinueButton.SetActive(false);
            PlayButton.SetActive(true);
        }
    }

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
