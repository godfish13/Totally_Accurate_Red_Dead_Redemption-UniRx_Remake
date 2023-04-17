using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class FadeMgr_UniRx : MonoBehaviour
{
    [SerializeField] private ButtonCtrl buttonCtrl;
    [SerializeField] private Image Dark;
    private WaitForSeconds ws = new WaitForSeconds(0.01f);
    [SerializeField] private float AlphaValue = 1.0f;

    private void OnEnable()
    {
        StartCoroutine(FadeIn());
    }

    void Start()
    {
        buttonCtrl.StartbtnClickedObservable.Subscribe(message =>   // Startbtn Ŭ�� �̺�Ʈ ����, ����� �޽��� �α׿� ��� + ���̵�ƿ� ����
        {
            Debug.Log(message);
            StartCoroutine(FadeOut());
        });
    }

    IEnumerator FadeIn()
    {
        while (AlphaValue >= 0)
        {
            Dark.color = new Color(0, 0, 0, AlphaValue);
            AlphaValue -= 0.01f;
            yield return ws;
        }
        Dark.enabled = false;
    }

    IEnumerator FadeOut()   //���̵� �ƿ� �� InGame scene �ε�
    {
        Dark.enabled = true;
        while (AlphaValue <= 1)
        {
            Dark.color = new Color(0, 0, 0, AlphaValue);
            AlphaValue += 0.01f;
            yield return ws;
        }
        SceneManager.LoadScene("InGame");
        Cursor.visible = false;
    }
}