using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeMgr : MonoBehaviour
{
    public Image Dark;

    private WaitForSeconds ws = new WaitForSeconds(0.01f);
    public float AlphaValue = 1.0f;

    private void OnEnable()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        while(AlphaValue >= 0)
        {
            Dark.color = new Color(0, 0, 0, AlphaValue);
            AlphaValue -= 0.01f;
            yield return ws;
        }
        Dark.enabled = false;
    }

    IEnumerator FadeOut()
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

    void StartFadeOut()
    {
        StartCoroutine(FadeOut());
    }
}
