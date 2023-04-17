using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Unity.VisualScripting;

public class FadeInStart : MonoBehaviour
{
    [SerializeField] private Image Dark;
    private WaitForSeconds ws = new WaitForSeconds(0.01f);
    [SerializeField] private float AlphaValue = 1.0f;

    private void OnEnable()
    {
        StartCoroutine(FadeIn());
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
}
