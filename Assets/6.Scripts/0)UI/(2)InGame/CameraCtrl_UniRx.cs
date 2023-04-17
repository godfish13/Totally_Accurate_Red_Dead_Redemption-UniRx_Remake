using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class CameraCtrl_UniRx : MonoBehaviour
{
    [SerializeField] private Transform Playertr;
    [SerializeField] private PlayerCtrl_UniRx _PlayerCtrl_UniRx;
    [SerializeField] private Transform CameraRigtr;
    [SerializeField] private Transform Cameratr;
    private Vector3 mouseMove;
    [SerializeField] private float mouseSensitivity = 1.5f;

    void Start()
    {
        this.LateUpdateAsObservable().Subscribe(_ =>
        {
            if (Input.GetMouseButton(1))    //�������� �� ī�޶� ��ġ ����
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
        });
    }
}
