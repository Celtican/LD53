using System;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public static PauseController instance;
    public GameObject pauseMenu;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space)) TogglePause();
    }

    public bool IsPaused()
    {
        return Time.timeScale == 0;
    }

    public void TogglePause()
    {
        if (IsPaused())
        {
            if (HeartController.instance.IsAlive()) Resume();
        }
        else Pause();
    }

    public void Pause()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    private void OnDestroy()
    {
        if (instance == this) instance = null;
    }
}