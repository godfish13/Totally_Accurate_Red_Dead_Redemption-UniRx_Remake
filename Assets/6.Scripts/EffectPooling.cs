using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPooling : MonoBehaviour
{
    public FireCtrl fireCtrl;

    public int maxPool = 20;
    public GameObject HittedEffect;
    public List<GameObject> BloodPool = new List<GameObject>();

    private void Awake()        // ������Ʈ Ǯ ��ũ��Ʈ ��� ������ �̸� �����ΰ� Awake()���� ����
    {
        CreateBloodPooling();
    }

    private void Start()
    {
        fireCtrl = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<FireCtrl>();
    }

    public void CreateBloodPooling()
    {
        GameObject BloodPools = new GameObject("BloodPools"); // ���� ����Ʈ ���ϵ�ȭ �س��� �θ� ������Ʈ ����
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
            if (BloodPool[i].activeSelf == false)   // ��Ȱ��ȭ�Ǿ��ִ� ����Ʈ ���
                return BloodPool[i];
        }        

        return null;
    }
}
