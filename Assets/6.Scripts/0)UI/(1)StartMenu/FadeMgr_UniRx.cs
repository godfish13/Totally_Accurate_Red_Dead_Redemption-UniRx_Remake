using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

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
        buttonCtrl.StartbtnClickedObservable.Subscribe(message =>   // Startbtn 클릭 이벤트 구독, 발행된 메시지 로그에 띄움 + 페이드아웃 시작
        {
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

    public IEnumerator FadeOut()   //페이드 아웃 후 InGame scene 로딩
    {
        Dark.enabled = true;
        while (AlphaValue <= 1)
        {
            Dark.color = new Color(0, 0, 0, AlphaValue);
            AlphaValue += 0.01f;
            yield return ws;
        }
        Cursor.visible = false;
    }
}
