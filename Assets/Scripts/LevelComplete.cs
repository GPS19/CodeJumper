using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        levelName = SceneManager.GetActiveScene().name.Replace(" ", "/");
        level.text = levelName;
    }

    private void Update()
    {
        if (!commandExecuter.gameOver)
        {
            timer += Time.deltaTime;
        }
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }

    public void LoadLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
        Time.timeScale = 1;
    }

    private void TimerFormat()
    {
        minutes = Mathf.FloorToInt(timer / 60);
        seconds = Mathf.FloorToInt(timer % 60);
        formatedTime = string.Format("{0:00}:{1:00}", minutes, seconds);
        time.text = formatedTime;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        commandExecuter.blockRaycast.blocksRaycasts = true;
        commandExecuter.gameOver = true;
        TimerFormat();
        deaths.text = commandExecuter.numberDeaths.ToString();
        commands.text = commandExecuter.numberCommands.ToString();
        levelCompleteCanvas.SetActive(true);
        Time.timeScale = 0;
        StartCoroutine(gameData.uploadData());
    }
}