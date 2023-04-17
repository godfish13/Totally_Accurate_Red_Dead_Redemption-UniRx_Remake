using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JudgeBetrayer : MonoBehaviour
{
    public bool isBetrayer;

    public void Hitted()
    {
        if(isBetrayer == true)
        {
            Debug.Log("Cathem!");
            SceneManager.LoadScene("WinningScene");
            Cursor.visible = true;
        }
        else
        {
            Debug.Log("Failed...");
            SceneManager.LoadScene("LosingScene");
            Cursor.visible = true;
        }
    }
}
