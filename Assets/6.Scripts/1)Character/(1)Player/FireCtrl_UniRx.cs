using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UnityEngine;
using UniRx;

public class FireCtrl_UniRx : MonoBehaviour
{
    [SerializeField] private GunEffectCtrl gunEffectCtrl;      //총구에 부착된 발사음 재생 스크립트
    [SerializeField] private EffectPooling effectPooling;
    [SerializeField] private BulletPooling bulletPooling;
    [SerializeField] private Transform firePostr;     //총구 오브젝트 transform
    [SerializeField] private RaycastHit hitInfo;
    [SerializeField] private float fireRange = 60.0f;

    void Start()
    {
        this.UpdateAsObservable()
            .Subscribe(input => Fire());
    }

    void Fire()
    {
        if (Input.GetMouseButton(1))
        {
            if (Input.GetMouseButtonDown(0))
            {
                BulletShot();
                gunEffectCtrl.SendMessage("Fire");  //발포음 재생 명령
                if (Physics.Raycast(firePostr.position, firePostr.forward, out hitInfo, fireRange))
                {
                    if (hitInfo.collider.CompareTag("ENEMY"))
                    {
                        Debug.Log("Hitted Enemy!!");
                        hitInfo.collider.SendMessage("Hitted");
                        HittedBlood();  // 적 피격위치에 혈흔 이펙트 풀링해서 재생
                    }
                }
                Debug.DrawRay(firePostr.position, firePostr.forward * fireRange, Color.blue, 1.0f);
            }
        }
    }

    void HittedBlood()
    {
        var Blood = effectPooling.GetBlood();
        Blood.transform.position = hitInfo.point;
        Blood.transform.rotation = Quaternion.FromToRotation(-Vector3.forward, hitInfo.normal);
        Blood.SetActive(true);
    }

    void BulletShot()
    {
        var Bullet = bulletPooling.GetBullet();
        Bullet.transform.position = firePostr.position;
        Bullet.transform.rotation = Quaternion.FromToRotation(Vector3.forward, firePostr.forward);
        Bullet.SetActive(true);
    }
}
