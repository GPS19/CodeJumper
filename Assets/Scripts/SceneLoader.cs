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
        SceneManager.LoadScene("Jugar");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        player.transform.position = player.startingPos;
        gameOver.gameOver = true;
    }
}
