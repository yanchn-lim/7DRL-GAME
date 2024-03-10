using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("LevelScene");
    }

    public void Controls()
    {
        SceneManager.LoadScene("Controls");
    }

    public void BackToMain()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
