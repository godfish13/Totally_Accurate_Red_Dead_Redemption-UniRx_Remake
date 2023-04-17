using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonsCtrl : MonoBehaviour
{
    public GameObject FadeMgr;

    public void startGame()
    {
        FadeMgr.SendMessage("StartFadeOut");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReStart()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
