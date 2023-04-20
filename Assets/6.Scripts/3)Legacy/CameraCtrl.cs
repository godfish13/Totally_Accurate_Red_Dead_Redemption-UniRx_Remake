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


    private void LateUpdate()   // Camera �̵��� ȸ�� 
    {

        if (playerCtrl.isAiming)    //�������� �� ī�޶� ��ġ ����
        {           
            Cameratr.localPosition = Vector3.MoveTowards(Cameratr.localPosition, new Vector3(2, -2, -5.5f), 0.4f);
        }
        else
        {
            Cameratr.localPosition = Vector3.MoveTowards(Cameratr.localPosition, new Vector3(0, -1, -10), 0.1f);
        }

        CameraRigtr.position = Playertr.position + Vector3.up * 6.5f;  //�÷��̾�� ĳ������ �Ӹ� ������
        
        mouseMove += new Vector3(0, Input.GetAxisRaw("Mouse X") * mouseSensitivity, 0);   //���콺�� �������� ����
        
        CameraRigtr.localEulerAngles = mouseMove;   //���콺 �̵��������� ȸ��
    }
}
