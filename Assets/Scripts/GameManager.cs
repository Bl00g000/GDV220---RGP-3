using System.Collections;
using System.Collections.Generic;
using TMPro;
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

        // FOR TESTING
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            GameOver(false);
        }
        
        if (Input.GetKeyDown(KeyCode.Q)) 
        {
            GameOver(true);
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

    public void GameOver(bool _bWon)
    {
        bIsPaused = true;
        Time.timeScale = 0.0f;

        if (!_bWon)
        {
            gameOver.GetComponentInChildren<TextMeshProUGUI>().text = "GAME OVER...";
        }
        else
        {
            gameOver.GetComponentInChildren<TextMeshProUGUI>().text = "VICTORY!";
        }

        gameOver.SetActive(true);
    }
}
