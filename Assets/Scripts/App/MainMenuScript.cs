using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using DPUtils.Systems.SaveSystem;


public class MainMenuScript : MonoBehaviour
{
    public SaveGameScript saveGameScript;

    public GameObject NewGameButton;
    public GameObject PlayButton;

    public GameObject ContinueButton;


    void Update()
    {
        string saveFilePath = Application.persistentDataPath + "/savedGameFile.dat";
        if (File.Exists(saveFilePath))
        {
            ContinueButton.SetActive(true);
            NewGameButton.SetActive(true);
            PlayButton.SetActive(false);
        }
        else
        {
            ContinueButton.SetActive(false);
            NewGameButton.SetActive(false);
            PlayButton.SetActive(true);

        }
    }

    public void PlayGame()
    {
        // This will load a scene based on the next scene available in the build index
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
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
