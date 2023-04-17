using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public Transform Playertr;
    private PlayerCtrl playerCtrl;

    public Transform CameraRigtr;
    public Transform Cameratr;

    private Vector3 mouseMove;
    public float mouseSensitivity = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        Playertr = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<Transform>();
        playerCtrl = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<PlayerCtrl>();
        CameraRigtr = GetComponent<Transform>();
        Cameratr = Camera.main.transform;       
    }


    private void LateUpdate()   // Camera 이동및 회전 
    {

        if (playerCtrl.isAiming)    //조준중일 시 카메라 위치 변경
        {           
            Cameratr.localPosition = Vector3.MoveTowards(Cameratr.localPosition, new Vector3(2, -2, -5.5f), 0.4f);
        }
        else
        {
            Cameratr.localPosition = Vector3.MoveTowards(Cameratr.localPosition, new Vector3(0, -1, -10), 0.1f);
        }

        CameraRigtr.position = Playertr.position + Vector3.up * 6.5f;  //플레이어블 캐릭터의 머리 높이쯤
        
        mouseMove += new Vector3(0, Input.GetAxisRaw("Mouse X") * mouseSensitivity, 0);   //마우스의 움직임을 가감
        
        CameraRigtr.localEulerAngles = mouseMove;   //마우스 이동방향으로 회전
    }
}
