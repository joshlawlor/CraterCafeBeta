using UnityEngine;

public class InGameMenuScript : MonoBehaviour
{
    public GameObject gameMenu;
    private bool isPaused = false;

    public void OnButtonClick()
    {
        ToggleGameMenu();
                    Debug.Log("Pause button clicked");

    }

    public void ToggleGameMenu()
    {
        if (gameMenu != null)
        {
            bool newMenuState = !gameMenu.activeSelf;

            // Toggle the game menu
            gameMenu.SetActive(newMenuState);

            // Pause or unpause the game based on the menu state
            SetGamePaused(newMenuState);
        }
        else{
            Debug.Log("GameMenu is not active");
        }
    }

    void SetGamePaused(bool paused)
    {
        isPaused = paused;

        // Pause or unpause the game based on the 'paused' parameter
        Time.timeScale = paused ? 0 : 1;

        // You may need to implement additional logic to handle pausing specific components or scripts
        // For example, you might iterate through GameObjects and call Pause/Unpause methods on specific scripts.
        // Keep in mind that this approach may not pause animations or physics-based operations directly tied to Time.deltaTime.
    }
}
