using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPooling : MonoBehaviour
{
    public int maxPool = 20;
    public GameObject Bullet;
    public List<GameObject> BulletPool = new List<GameObject>();

    private void Awake()        // ������Ʈ Ǯ ��ũ��Ʈ ��� ������ �̸� �����ΰ� Awake()���� ����
    {
        CreateBulletPooling();
    }

    public void CreateBulletPooling()
    {
        GameObject BulletPools = new GameObject("BulletPools"); // �Ѿ� ���ϵ�ȭ �س��� �θ� ������Ʈ ����
        for (int i = 0; i < maxPool; i++)
        {
            var obj = Instantiate<GameObject>(Bullet, BulletPools.transform);
            obj.name = "Bullet_" + i.ToString("00");
            obj.SetActive(false);
            BulletPool.Add(obj);
        }
    }

    public GameObject GetBullet()
    {
        for (int i = 0; i < BulletPool.Count; i++)
        {
            if (BulletPool[i].activeSelf == false)   // ��Ȱ��ȭ�Ǿ��ִ� �Ѿ� ���
                return BulletPool[i];
        }

        return null;
    }
}
