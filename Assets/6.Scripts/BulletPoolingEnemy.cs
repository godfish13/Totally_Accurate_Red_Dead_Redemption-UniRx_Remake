using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolingEnemy : MonoBehaviour
{
    public int maxPool = 20;
    public GameObject E_Bullet;
    public List<GameObject> BulletPool = new List<GameObject>();

    private void Awake()        // 오브젝트 풀 스크립트 재생 이전에 미리 만들어두게 Awake()에서 진행
    {
        CreateBulletPooling();
    }

    public void CreateBulletPooling()
    {
        GameObject EnemyBulletPool = new GameObject("EnemyBulletPool"); // 총알 차일드화 해놓을 부모 오브젝트 생성
        for (int i = 0; i < maxPool; i++)
        {
            var obj = Instantiate<GameObject>(E_Bullet, EnemyBulletPool.transform);
            obj.name = "Bullet_" + i.ToString("00");
            obj.SetActive(false);
            BulletPool.Add(obj);
        }
    }

    public GameObject GetBullet()
    {
        for (int i = 0; i < BulletPool.Count; i++)
        {
            if (BulletPool[i].activeSelf == false)   // 비활성화되어있는 총알 사용
                return BulletPool[i];
        }

        return null;
    }
}
