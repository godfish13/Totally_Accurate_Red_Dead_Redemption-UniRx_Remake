using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimCtrl : MonoBehaviour
{
    public PlayerCtrl playerCtrl;
    public GameObject AimImage;

    private void Start()
    {
        playerCtrl = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<PlayerCtrl>();
    }

    private void Update()
    {
        if(playerCtrl.isAiming)
        {
            AimImage.SetActive(true);
        }
        else
        {
            AimImage.SetActive(false);
        }
    }

    
}
