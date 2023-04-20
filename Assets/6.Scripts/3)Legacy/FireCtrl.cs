using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCtrl : MonoBehaviour
{
    private PlayerCtrl playerCtrl;
    private GunEffectCtrl gunEffectCtrl;      //총구에 부착된 발사음 재생 스크립트
    private EffectPooling effectPooling;
    private BulletPooling bulletPooling;

    public int Damage = 1;

    public Transform firePostr;     //총구 오브젝트 transform
    public RaycastHit hitInfo;
    public float fireRange = 60.0f;


    // Start is called before the first frame update
    void Start()
    {
        playerCtrl = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<PlayerCtrl>();
        firePostr = GameObject.FindGameObjectWithTag("FIREPOS").GetComponent<Transform>();
        gunEffectCtrl = GameObject.FindGameObjectWithTag("FIREPOS").GetComponent<GunEffectCtrl>();
        effectPooling = GameObject.FindGameObjectWithTag("POOLINGMAKER").GetComponent<EffectPooling>();
        bulletPooling = GameObject.FindGameObjectWithTag("POOLINGMAKER").GetComponent<BulletPooling>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCtrl.isAiming)
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
