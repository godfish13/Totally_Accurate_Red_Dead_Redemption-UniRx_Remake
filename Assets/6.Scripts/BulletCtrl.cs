using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public float speed = 100.0f;
    public float LifeTime = 0.3f;
    public float Limitance = 0;
    private void OnEnable()
    {
        gameObject.GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }                                       //velocity : ������ ������� velocity�� �ӵ��� �̵�
    

    // Update is called once per frame
    void Update()
    {
        Limitance += 0.1f * Time.deltaTime;
        if(Limitance >= LifeTime)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        Limitance = 0;
    }
}
