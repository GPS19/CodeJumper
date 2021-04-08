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
    
    private bool visible = false;
    private CommandExecuter commandExecuter;

    private void Start()
    {
        commandExecuter = FindObjectOfType<CommandExecuter>();
        level.text = SceneManager.GetActiveScene().name.Replace(" ", "/");
        //time.text = TODO start timer when the active scene is loaded
        deaths.text = commandExecuter.numberDeaths.ToString();
        commands.text = commandExecuter.numberCommands.ToString();
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            deaths.text = commandExecuter.numberDeaths.ToString();
            commands.text = commandExecuter.numberCommands.ToString();
            levelCompleteCanvas.SetActive(!visible);
            visible = !visible;
        }
    }
    
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        //player.transform.position = player.startingPos;
        //gameOver.gameOver = true;
    }
}