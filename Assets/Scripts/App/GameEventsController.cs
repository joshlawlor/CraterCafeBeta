using System.Collections;
using UnityEngine;
using DPUtils.Systems.DateTime;
using TMPro;

public class GameEventsController : MonoBehaviour
{
    public RandomNPCSpawner randomNPCSpawning;
    public TextMeshProUGUI BarStatus; // Reference to the TMP object
    private bool isSpawningActive = false;

    private bool isGamePaused = false;

    private TimeManagerScript timeManager;

    private void Start()
    {
        // Find the TimeManagerScript in the scene
        timeManager = FindObjectOfType<TimeManagerScript>();

        if (timeManager == null)
        {
            Debug.LogError("TimeManagerScript not found in the scene.");
        }
    }

    // Update is called once per frame
    public void Update()
    {
        if (!isGamePaused)
        {
            // Access the TimeInfo from TimeManagerScript
            TimeInfo currentTimeInfo = timeManager.CurrentTimeInfo;

            // Now you can use currentTimeInfo in this script
            Debug.Log($"Current hour: {currentTimeInfo.Hour}, AM: {currentTimeInfo.IsAM}");

            // Check for the specific time conditions
            CheckTimeConditions(currentTimeInfo.Hour, currentTimeInfo.Minutes, currentTimeInfo.IsAM);

            // Update the bar status
            UpdateBarStatus(isSpawningActive);
        }
    }

    // Check time conditions and handle NPC spawning/stopping
    void CheckTimeConditions(int currentHour, int currentMinutes, bool isAM)
    {
        if (isAM && currentHour >= 7 && currentMinutes == 0 && !isSpawningActive)
        {
            // Start spawning if it's morning and spawning is not already active
            StartSpawning();
        }
        else if (!isAM && currentHour >= 17 && currentMinutes == 0 && isSpawningActive)
        {
            // Stop spawning if it's 4 PM and spawning is currently active
            StopSpawning();
        }
    }

    // Start spawning NPCs
    void StartSpawning()
    {
        randomNPCSpawning.StartSpawning();
        isSpawningActive = true;
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
