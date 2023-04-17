using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPooling : MonoBehaviour
{
    public FireCtrl fireCtrl;

    public int maxPool = 20;
    public GameObject HittedEffect;
    public List<GameObject> BloodPool = new List<GameObject>();

    private void Awake()        // 오브젝트 풀 스크립트 재생 이전에 미리 만들어두게 Awake()에서 진행
    {
        CreateBloodPooling();
    }

    private void Start()
    {
        fireCtrl = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<FireCtrl>();
    }

    public void CreateBloodPooling()
    {
        GameObject BloodPools = new GameObject("BloodPools"); // 출혈 이펙트 차일드화 해놓을 부모 오브젝트 생성
        for (int i = 0; i < maxPool; i++)
        {
            var obj = Instantiate<GameObject>(HittedEffect, BloodPools.transform);
            obj.name = "Blood_" + i.ToString("00");
            obj.SetActive(false);
            BloodPool.Add(obj);
        }
    }

    public GameObject GetBlood()
    {       
        for (int i = 0; i < BloodPool.Count; i++)
        {
            if (BloodPool[i].activeSelf == false)   // 비활성화되어있는 이펙트 사용
                return BloodPool[i];
        }        

        return null;
    }
}
