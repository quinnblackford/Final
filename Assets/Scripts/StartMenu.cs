using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void StartGame() 
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Instructions() 
    {
        SceneManager.LoadScene("Instructions");
    }

    public void Quit() 
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
