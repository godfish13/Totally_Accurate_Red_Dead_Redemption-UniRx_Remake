using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPooling : MonoBehaviour
{
    public FireCtrl fireCtrl;

    public int maxPool = 20;
    public GameObject Bullet;
    public List<GameObject> BulletPool = new List<GameObject>();

    private void Awake()        // ������Ʈ Ǯ ��ũ��Ʈ ��� ������ �̸� �����ΰ� Awake()���� ����
    {
        CreateBulletPooling();
    }

    private void Start()
    {
        fireCtrl = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<FireCtrl>();
    }

    public void CreateBulletPooling()
    {
        GameObject BulletPools = new GameObject("BulletPools"); // ���� ����Ʈ ���ϵ�ȭ �س��� �θ� ������Ʈ ����
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
