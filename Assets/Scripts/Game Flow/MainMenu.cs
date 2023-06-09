using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void PlayTDM()
    {
        SceneManager.LoadScene("TDMScene");
    }

    public void ShowTutorial()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    public void ShowCredits()
    {
        SceneManager.LoadScene("CreditsScene");
    }

    public void ShowMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}