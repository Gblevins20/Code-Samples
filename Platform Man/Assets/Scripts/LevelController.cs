/*
 * Team Platform Men (Gregory Blevins)
 * Project 2
 * Controls which level is loaded
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    //This array of scenes of all the levels
    public string[] levels = {"MainMenu", "SampleScene", "Tutorial1", "Tutorial2" , "Tutorial3" , "Tutorial4", "Level1", "Level2", "Brandt Level 1"};

    public Canvas pauseMenu;

    public GameOverController gameOver;

    void Start()
    {
        gameOver = FindObjectOfType<GameOverController>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pauseMenu.gameObject.activeInHierarchy)
            {
                pauseMenu.gameObject.SetActive(true);
                
            }
            else if (pauseMenu.gameObject.activeInHierarchy)
            {
                ResumeLevel();
            }
        }
    }

    public void ResumeLevel()
    {
        pauseMenu.gameObject.SetActive(false);
    }

    //Restarts the current level
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    //ChangeLevel takes the interger that points to what level is in the above array
    public void ChangeLevel(int level)
    {
        SceneManager.LoadScene(levels[level]);
    }

    //Exits out of the game
    //public void Quit()
    //{
    //    Application.Quit();
    //}
}
