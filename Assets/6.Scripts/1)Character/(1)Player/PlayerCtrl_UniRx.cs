using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerCtrl_UniRx : MonoBehaviour
{
    [SerializeField] private Transform Cameratr;
    [SerializeField] private Transform model;    //ĳ���� �� ȸ���� ���� ĳ���͸� �ҷ��α�
    [SerializeField] private float MSpeed = 3.0f; //�̵��ӵ�

    void Start()
    {
        this.UpdateAsObservable().Select(_ => new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")))
            .Where(input => input.magnitude > 0.5f)
            .Subscribe(input => Move(input));           // ĳ���� �̵�

        this.UpdateAsObservable()
            .Subscribe(input =>
            {
                if(Input.GetMouseButton(1)) rotateCharacterWhileAiming();   // ���� ���� ȸ��             
                else rotateCharacter();                                     // ������ ���� ȸ��
            });         // ĳ���� ȸ��           // where ���۷����� ��뺸�� if���� �� ������ ��Ȳ�̹Ƿ� if�� ���
    }

    void Move(Vector3 input)
    {
        if (Input.GetMouseButton(1)) MSpeed = 5.0f; // ĳ���� ���ػ��¿� ���� �̵��ӵ� ��ȭ
        else MSpeed = 10.0f;
        Vector3 movedir = (Vector3.forward * input.z) + (Vector3.right * input.x);
        GetComponent<Transform>().Translate(movedir.normalized * MSpeed * Time.deltaTime, Space.Self);
    }

    void rotateCharacter()      // ī�޶���� ĳ���� ȸ�� == �Ϲ� ȸ��
    {
        Vector3 inputMoveXZ = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); // �������� ����
        inputMoveXZ = GetComponent<Transform>().TransformDirection(inputMoveXZ);  // TransformDirection : ���� direction�� world direction���� ��ȯ���� 
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            Quaternion cameraRotation = Cameratr.rotation;
            cameraRotation.x = cameraRotation.z = 0;    //y�ุ �ʿ��ϹǷ� ������ ���� 0����
            GetComponent<Transform>().rotation = Quaternion.Slerp(GetComponent<Transform>().rotation, cameraRotation, 10.0f * Time.deltaTime);
            if (inputMoveXZ != Vector3.zero)  //Quaternion.LookRotation�� (0,0,0)�� ���� ��� ���Ƿ� ����ó��
            {
                Quaternion characterRotation = Quaternion.LookRotation(inputMoveXZ);
                characterRotation.x = characterRotation.z = 0;
                model.rotation = Quaternion.Slerp(model.rotation, characterRotation, 10.0f * Time.deltaTime);
            }   // ĳ���� ���� ������ ������ ������ ������������ Slerp�� ���� ȸ��
        }
    }

    void rotateCharacterWhileAiming()   // ī�޶� ��������� ĳ���� ���������� ��ġ�ϴ� ȸ�� == ���� �� ȸ��
    {
        Quaternion CameraRotation = Quaternion.LookRotation(Cameratr.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, CameraRotation, 10.0f * Time.deltaTime);
        model.rotation = Quaternion.Slerp(model.rotation, transform.rotation, 10.0f * Time.deltaTime);
    }
}