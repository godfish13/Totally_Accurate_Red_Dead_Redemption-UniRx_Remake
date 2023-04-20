using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class EnemyFire_UniRx : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Transform Playertr;
    [SerializeField] private EnemyAI_UniRx _EnemyAI_UniRx;
    [SerializeField] private EffectPooling effectPooling;
    [SerializeField] private BulletPoolingEnemy bulletPoolingEnemy;

    private float nextFire = 0.0f;
    private readonly float FireCoolDown = 0.3f;
    private readonly float damping = 10.0f;
    public float fireRange = 40.0f;

    public AudioClip EnemyGunFireSound;

    public Transform EnemyfirePostr;
    public ParticleSystem EnemyMuzzleFlash;
    public RaycastHit hitInfo;

    void Start()
    {
        Playertr = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<Transform>();
        effectPooling = GameObject.FindGameObjectWithTag("POOLINGMAKER").GetComponent<EffectPooling>();
        bulletPoolingEnemy = GameObject.FindGameObjectWithTag("POOLINGMAKER").GetComponent<BulletPoolingEnemy>();

        IDisposable UpdateStream = Observable.EveryUpdate().Subscribe(_ => FireChecker());
        //사망시 Update스트림 dispose할 수 있게 IDisposable로 선언
        _EnemyAI_UniRx.isDeadObservable
           .Where(OnNextValue => OnNextValue == true)
           .First()
           .Subscribe(_ =>
           {
               this.enabled = false;
               UpdateStream.Dispose();
           }, () => Debug.Log("EnemyFire Stream Oncompleted"));
        // AddTo(this) 후 사망 시 자기 스크립트를 destroy하도록 UpdateStream관리를 시도했으나 destroy이후 Subscribe를 한번 더 실행하고
        // RotateWhileFiring에서 transform.position을 탐색하나 이미 destroy된 후이므로
        // null reference오류가 발생, 이후 dispose되는 것을 확인 // 해당 오류를 피하기 위해 IDisposable로 구현
    }

    void FireChecker()  // 발포중인지 아닌지 판별
    {
        if (_EnemyAI_UniRx.isFire)
        {
            RotateWhileFiring();
            nextFire += 0.1f * Time.deltaTime;
            if (nextFire >= FireCoolDown)
            {
                E_Fire();
                nextFire = UnityEngine.Random.Range(0.0f, 0.1f);
            }
        }
        else
        {
            nextFire = 0.2f;
        }
    }

    void E_Fire() // 발포
    {
        EnemyMuzzleFlash.Play();
        BulletShot();
        Debug.DrawRay(EnemyfirePostr.position, EnemyfirePostr.forward * fireRange, Color.blue, 1.0f);
        audioSource.PlayOneShot(EnemyGunFireSound, 0.5f);
        if (Physics.Raycast(EnemyfirePostr.position, EnemyfirePostr.forward, out hitInfo, fireRange))
        {
            if (hitInfo.collider.CompareTag("PLAYER"))
            {
                Debug.Log("Player Hitted!!");
                hitInfo.collider.SendMessage("Hitted");

                HittedBlood();  // 플레이어 피격위치에 혈흔 이펙트 풀링해서 재생
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
        var Bullet = bulletPoolingEnemy.GetBullet();
        Bullet.transform.position = EnemyfirePostr.position;
        Bullet.transform.rotation = Quaternion.FromToRotation(Vector3.forward, EnemyfirePostr.forward);
        Bullet.SetActive(true);
    }

    void RotateWhileFiring()
    {
        Quaternion rot = Quaternion.LookRotation(Playertr.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * damping);
    }
}
