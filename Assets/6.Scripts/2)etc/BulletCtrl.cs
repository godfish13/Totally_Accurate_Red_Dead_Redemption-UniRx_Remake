using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class BulletCtrl : MonoBehaviour
{
    [SerializeField] private float speed = 100.0f;
    [SerializeField] private float LifeTime = 0.3f;
    [SerializeField] private float Limitance = 0;
    private void OnEnable()
    {
        gameObject.GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }                                       //velocity : 질량에 관계없이 velocity의 속도로 이동

    private void Start()
    {
        this.UpdateAsObservable()
            .Subscribe(_ => BulletLifeTime());
    }

    void BulletLifeTime()
    {
        Limitance += 0.1f * Time.deltaTime;
        if (Limitance >= LifeTime)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        Limitance = 0;
    }
}
