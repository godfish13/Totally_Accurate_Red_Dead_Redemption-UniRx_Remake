using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BtnCtrl_EndScene : MonoBehaviour
{
    [SerializeField] private Button ReStartbtn;
    [SerializeField] private Button Quitbtn;

    void Start()
    {
        ReStartbtn.OnClickAsObservable().Subscribe(_ => SceneManager.LoadScene("StartMenu"));
        Quitbtn.OnClickAsObservable().Subscribe(_ => Application.Quit());
    }
}
