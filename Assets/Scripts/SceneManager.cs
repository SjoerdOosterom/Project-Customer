using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneManagerScript : MonoBehaviour
{
    public GameObject loadingScreen;
    public GameObject pauseMenu;

    //public PlayerController playerController;  // Reference to PlayerController

    private bool isPaused = false;

    void Update()
    {
        // Check if the Escape key is pressed to toggle the pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        // Ensure cursor is locked when game is not paused
        /*if (!isPaused)
        {
            playerController.LockCursor();  // Lock cursor when game is resumed
        }
        else
        {
            playerController.UnlockCursor();  // Unlock cursor when game is paused
        }*/
    }

    public void StartGame()
    {
        LoadScene("SampleScene");
    }

    public void OpenOptions()
    {
        Debug.Log("Options menu opened.");
    }

    public void RestartCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        LoadScene(currentSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            Debug.Log("Loading progress: " + (progress * 100) + "%");

            yield return null;
        }

        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);   // Show pause menu
        Time.timeScale = 0f;         // Pause the game
        isPaused = true;             // Set the paused state
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);  // Hide pause menu
        Time.timeScale = 1f;         // Resume the game
        isPaused = false;            // Set the paused state
    }
}
