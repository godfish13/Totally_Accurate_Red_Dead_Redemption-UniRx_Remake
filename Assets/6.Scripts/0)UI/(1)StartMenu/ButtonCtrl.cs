using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using System;

public class ButtonCtrl : MonoBehaviour
{
    [SerializeField] private Button Startbtn;
    [SerializeField] private Button Quitbtn;

    private Subject<string> StartbtnClicked = new Subject<string>();    // �̺�Ʈ ���� ��ü StartbtnClicked
    public IObservable<string> StartbtnClickedObservable => StartbtnClicked;    // private StartbtnClicked�� observable�ϰ� getter ����

    void Start()
    {
        Startbtn.OnClickAsObservable().Subscribe(_ =>
        { 
            StartbtnClicked.OnNext("Startbtn Clicked");  // start ��ư Ŭ�� �� "Startbtn Clicked"�޽��� ���� => FadeOut �� ����ǵ��� FadeMgr_UniRx���� ����
            StartbtnClicked.OnCompleted();               // start ��ư �ٴ�Ŭ�� ���� ���� ��Ʈ�� ����
        });                                             // + ���̵�ƿ� �� UI�ֻ����� ����ȭ��UI�� ������ ��ư�� ������ ���Ե����� ������ �������
        Quitbtn.OnClickAsObservable().Subscribe(_ => Application.Quit());
    }
}
