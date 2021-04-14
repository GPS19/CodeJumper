using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private PlayerMovement player;
    [SerializeField] CommandExecuter gameOver;
    
    private void Start()
    {
        player = PlayerMovement.instance;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("MUNDO_1 NIVEL_1"); // This function is meant to load the level the player got to in last session. For now just loads first level so game can start
    }
    
    public void LoadLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        gameOver.numberDeaths++;
        player.transform.position = player.startingPos + new Vector3(0, 3, 0);
        player.isOnGround = true;
        gameOver.gameOver = true;
    }
}
