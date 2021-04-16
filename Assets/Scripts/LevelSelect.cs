using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Pablo Yamamoto, Santiago Kohn, Gianluca Beltran
 *
 * Script to handle Level select menu
 */

public class LevelSelect : MonoBehaviour
{
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadFirstLevel()
    {
        SceneManager.LoadScene("MUNDO_1 NIVEL_1");
    }

    public void LoadSecondLevel()
    {
        SceneManager.LoadScene("MUNDO_1 NIVEL_2");
    }

    public void LoadThirdLevel()
    {
        SceneManager.LoadScene("MUNDO_1 NIVEL_3");
    }
}
