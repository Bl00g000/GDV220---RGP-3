using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] GameObject pauseMenu;
    [HideInInspector] public bool bIsPaused;

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

        bIsPaused = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PlayGame()
    {
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
}
