using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UnityEngine;
using UniRx;

public class AimCtrl_UniRx : MonoBehaviour
{
    public GameObject AimImage;

    private void Start()
    {
        this.UpdateAsObservable()
            .Subscribe(input =>
            {
                if (Input.GetMouseButton(1)) AimImage.SetActive(true);               
                else AimImage.SetActive(false);               
            });
    }
}
