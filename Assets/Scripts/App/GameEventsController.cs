using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine;
using DPUtils.Systems.DateTime;
using DPUtils.Systems.SaveSystem;
using TMPro;

public class GameEventsController : MonoBehaviour
{
    public RandomNPCSpawner randomNPCSpawning;
    public SaveGameScript saveGameScript;
    private InGameMenuScript inGameMenuScript;

    public TextMeshProUGUI BarStatus; // Reference to the TMP object
    private bool isSpawningActive = false;

    private bool isGamePaused = false;

    private TimeManagerScript timeManager;

    private void Start()
    {
        string saveFilePath = Application.persistentDataPath + "/savedGameFile.dat";

        if (File.Exists(saveFilePath))
        {
            saveGameScript.LoadGame();
        }
        // Find the TimeManagerScript in the scene
        timeManager = FindObjectOfType<TimeManagerScript>();
        inGameMenuScript = FindObjectOfType<InGameMenuScript>();

        if (timeManager == null)
        {
            Debug.LogError("TimeManagerScript not found in the scene.");
        }
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        if (!isGamePaused)
        {
            // Access the TimeInfo from TimeManagerScript
            TimeInfo currentTimeInfo = timeManager.CurrentTimeInfo;

            // Now you can use currentTimeInfo in this script
            Debug.Log($"Current hour: {currentTimeInfo.Hour}, AM: {currentTimeInfo.IsAM}");

            // Check for the specific time conditions
            CheckTimeConditions(currentTimeInfo.Hour);

            AutoSaveDaily(currentTimeInfo.Hour, currentTimeInfo.Minutes);

            // Update the bar status
            UpdateBarStatus(isSpawningActive);
        }
    }

    // Check time conditions and handle NPC spawning/stopping
    void CheckTimeConditions(int currentHour)
    {
        if (currentHour >= 7 && currentHour < 17 && !isSpawningActive)
        {
            // Start spawning if it's morning and spawning is not already active
            StartSpawning();
        }
        else if (currentHour > 17 && isSpawningActive)
        {
            StopSpawning();
        }
    }

    void AutoSaveDaily(int currentHour, int currentMinutes)
    {
        if (currentHour == 23 && currentMinutes == 00)
        {
            saveGameScript.SaveGame();
            Debug.Log("Daily auto-save completed");
        }
    }

    // Start spawning NPCs
    void StartSpawning()
    {
        if (randomNPCSpawning)
        {
            randomNPCSpawning.StartSpawning();
            isSpawningActive = true;
        }

    }

    // Stop spawning NPCs
    void StopSpawning()
    {
        randomNPCSpawning.StopSpawning();
        isSpawningActive = false;
    }

    // Update bar status based on spawning activity
    void UpdateBarStatus(bool barOpen)
    {
        if (BarStatus != null)
        {
            if (barOpen)
            {
                BarStatus.text = "Bar: Open ";
                BarStatus.color = Color.green;
            }
            else
            {
                BarStatus.text = "Bar: Closed ";
                BarStatus.color = Color.red;
            }
        }
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void ExitToMenu()
    {

        inGameMenuScript.OnButtonClick(); // NEED TO UNPAUSE GAME BEFORE TRANSITION (OTHERWISE ALL SCENES LOAD IN PAUSE MODE)
        SceneManager.LoadSceneAsync("Main Menu");


    }

    public void PauseGame()
    {
        // Pause the game
        isGamePaused = true;

        // Halt the current DateTime in the TimeManagerScript
        timeManager.PauseDateTime();

        // Freeze all characters and NPCs
        // (You might need to implement logic in each script to handle pausing/unpausing)
        // For example, you can iterate through GameObjects and call Pause/Unpause methods on specific scripts.

        // Halts NPC spawning
        StopSpawning();
    }

    public void UnpauseGame()
    {
        // Unpause the game
        isGamePaused = false;

        // Resume the current DateTime in the TimeManagerScript
        timeManager.ResumeDateTime();

        // Unfreeze all characters and NPCs
        // (You might need to implement logic in each script to handle pausing/unpausing)

        // Resumes NPC spawning if needed
    }
}
