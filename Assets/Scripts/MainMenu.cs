using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("Game Closed");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Cornfield");

        Timer.instance.BeginTimer();
    }

    public void SelectLevel2()
    {
        SceneManager.LoadScene("Rooftop");

        Timer.instance.BeginTimer();
    }
}
