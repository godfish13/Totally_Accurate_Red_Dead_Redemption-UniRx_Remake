using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStatus : MonoBehaviour
{
    public float HP = 50.0f;
    public Image HPCircle;

    public int KillScore = 0;
    public Image BulletCircle;

    public Image BloodScreen;

    private WaitForSeconds ws = new WaitForSeconds(0.1f);

    public void Hitted()        // 체력원 표시
    {
        HP--;
        StartCoroutine(ShowBloodScreen());
        HPCircle.fillAmount = HP / 50.0f;
        if(HP <= 0)
        {
            Cursor.visible = true;
            SceneManager.LoadScene("LosingScene");
        }
    }

    public void KillPlus()      // 킬수(6완성시 배신자 처단 가능) 표시
    {
        KillScore++;
        
        BulletCircle.fillAmount = KillScore / 6.0f;
        if(KillScore == 6)
        {
            GameObject.Find("Saloon").SendMessage("OpenTheDoor");
        }
    }

    IEnumerator ShowBloodScreen()
    {
        BloodScreen.color = new Color(1, 0, 0, Random.Range(0.2f, 0.3f));
        yield return ws;
        BloodScreen.color = Color.clear;
    }
}
