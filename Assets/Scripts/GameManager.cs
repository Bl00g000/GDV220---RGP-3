using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] GameObject pauseMenu;
    [HideInInspector] public bool bIsPaused;

    [SerializeField] GameObject gameOver;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }

        if (Input.GetKeyDown(KeyCode.E)) 
        {
            GameOver();
        }
    }

    public void PlayGame()
    {
        bIsPaused = false;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ReturnToMenu()
    {
        PauseGame();
        SceneManager.LoadScene(0);
    }

    public void PauseGame()
    {
        if (!bIsPaused)
        {
            bIsPaused = true;
            Time.timeScale = 0.0f;
            pauseMenu.SetActive(true);
        }
        else
        {
            bIsPaused = false;
            Time.timeScale = 1.0f;
            pauseMenu.SetActive(false);
        }
    }

    public void GameOver()
    {
        bIsPaused = true;
        Time.timeScale = 0.0f;
        gameOver.SetActive(true);
    }
}
