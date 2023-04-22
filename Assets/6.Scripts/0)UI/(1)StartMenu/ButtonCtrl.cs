using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public class ButtonCtrl : MonoBehaviour
{
    [SerializeField] private Button Startbtn;
    [SerializeField] private Button Quitbtn;
    [SerializeField] private FadeMgr_UniRx _FadeMgr_UniRx;

    private Subject<string> StartbtnClicked = new Subject<string>();    // 이벤트 발행 주체 StartbtnClicked
    public IObservable<string> StartbtnClickedObservable => StartbtnClicked;    // private StartbtnClicked를 observable하게 getter 생성

    void Start()
    {
        Startbtn.OnClickAsObservable().Subscribe(_ =>
        {
            StartbtnClicked.OnNext("Startbtn Clicked");  // start 버튼 클릭 시 "Startbtn Clicked"메시지 발행 => FadeOut 후 실행되도록 FadeMgr_UniRx에서 관리
            WaitFadeOut().Forget();         //페이드 아웃 진행 후 InGame Scene 호출
            StartbtnClicked.OnCompleted();               // start 버튼 다단클릭 오류 방지 스트림 종료
        });     // + 페이드아웃 시 UI최상층이 검은화면UI로 덮혀서 버튼을 누를수 없게되지만 만일을 대비하자

        Quitbtn.OnClickAsObservable().Subscribe(_ => Application.Quit());
    }

    private async UniTaskVoid WaitFadeOut()
    {
        await _FadeMgr_UniRx.FadeOut();
        SceneManager.LoadScene("InGame");
    }
}
