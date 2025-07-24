using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class RestartScene : MonoBehaviour
{
    // This function will be called when the button is pressed
    public void RestartCurrentScene()
    {
        // Get the index of the currently active scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Load the scene again using its index
        SceneManager.LoadScene(currentSceneIndex);

        // Optional: If you have time-based elements or physics, you might want to
        // reset the time scale or physics simulation if it was paused or modified.
        // Time.timeScale = 1f;
        // Physics.autoSimulation = true; // For older versions or specific setups
    }
}