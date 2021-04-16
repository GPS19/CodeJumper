using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * Pablo Yamamoto, Santiago Kohn, Gianluca Beltran
 *
 * Script in charge of completing a level
 */

public class LevelComplete : MonoBehaviour
{
    [SerializeField] private Text level;
    [SerializeField] private Text time;
    [SerializeField] private Text deaths;
    [SerializeField] private Text commands;
    [SerializeField] private GameObject levelCompleteCanvas;
    
    private float timer = 0f;
    private float minutes = 0f;
    private float seconds = 0f;
    public string formatedTime;
    public CommandExecuter commandExecuter;
    public string levelName;
    private wwwFormGameData gameData;

    private void Start()
    {
        gameData = GetComponent<wwwFormGameData>();
        commandExecuter = FindObjectOfType<CommandExecuter>();
        levelName = SceneManager.GetActiveScene().name.Replace(" ", "/"); // Get the name of the scene and replace the space with a /
        level.text = levelName;
    }

    private void Update() // on every frame, update the time elpased in the level
    {
        if (!commandExecuter.gameOver)
        {
            timer += Time.deltaTime;
        }
    }

    public void LoadNextLevel() // called when pressing next level on level complete menu, loads next level/scene
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1; // reset timescale to 1 in order to unpause the game
    }

    public void LoadMainMenu() // called when prssing main menu on level complete menu, loads main menu
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }

    public void LoadLevelSelect() // called when pressing levelselect on level complete menu, loads level select menu
    {
        SceneManager.LoadScene("LevelSelect");
        Time.timeScale = 1;
    }

    private void TimerFormat() // Function to format timer in minutes and seconds
    {
        minutes = Mathf.FloorToInt(timer / 60); // get timers minutes 
        seconds = Mathf.FloorToInt(timer % 60); // get timers seconds
        formatedTime = string.Format("{0:00}:{1:00}", minutes, seconds); // format timer
        time.text = formatedTime; // display timer on level complete menu
    }
    
    private void OnTriggerEnter2D(Collider2D other) // called when the player reaches the goal
    {
        commandExecuter.blockRaycast.blocksRaycasts = true; // block commands raycast so that the user cant keep moving them around 
        commandExecuter.gameOver = true; // level complete so game over is true
        TimerFormat();
        deaths.text = commandExecuter.numberDeaths.ToString(); // display number of deaths
        commands.text = commandExecuter.numberCommands.ToString(); // display number of commands used
        levelCompleteCanvas.SetActive(true); // display the level complete canvas/menu
        Time.timeScale = 0; // pause the game by making the time scale = 0
        StartCoroutine(gameData.uploadData()); // upload data to server to store it in mysql database
    }
}