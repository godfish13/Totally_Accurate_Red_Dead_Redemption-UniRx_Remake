using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public Transform Cameratr;
    private Transform model;    //ĳ���� �� ȸ���� ���� ĳ���͸� �ҷ��α�

    public float v = 0.0f;      //�̵����� input
    public float h = 0.0f;
    public float MSpeed = 0.0f; //�̵��ӵ�

    public bool isAiming;

    private void Awake()
    {
        model = GetComponent<Transform>().GetChild(0);       
    }

    // Start is called before the first frame update
    void Start()
    {                   // ĳ����Rig ȸ�� ��Ȱ��ȭ
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
        Cameratr = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {                                       //movement input
        v = Input.GetAxis("Vertical");      //forward(z+), backward(z-)
        h = Input.GetAxis("Horizontal");    //left(x-), right(x+)       
        
        if (Input.GetMouseButton(1))        // ��Ŭ�����Ͻ� aiming���(�̵��ӵ� ����), �ƴҽ� �Ϲݸ��
        {
            isAiming = true;
            rotateCharacterWhileAiming();
            MSpeed = 5.0f;
        }
        else
        {
            isAiming = false;
            rotateCharacter();
            MSpeed = 10.0f;
        }
        Move(v, h);
    }

    private void Move(float v, float h)
    {
        Vector3 movedir = (Vector3.forward * v) + (Vector3.right * h);      //�����¿� �̵� ���� ���� ���
        GetComponent<Transform>().Translate(movedir.normalized * MSpeed * Time.deltaTime, Space.Self);
        //normalized�� �ִ�ũ�� 1�� ���Ժ���ȭ(���ϸ� 90�� ���̰��� ������ ���� �ΰ��� �ջ�Ǿ� �ִ�ũ�Ⱑ 1�� �Ѿ)
    }

    void rotateCharacter()      // ī�޶���� ĳ���� ȸ��
    {
        Vector3 inputMoveXZ = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //�¿� input��, ���Ʒ� input��(== 0), ���� input���� Vector3�� ���� �����ϰ� �̵������� ����Ѵ�.
        inputMoveXZ = GetComponent<Transform>().TransformDirection(inputMoveXZ);  // TransformDirection : ���� direction�� world direction���� ��ȯ���� 
        //Vector3 MoveDir = "               // ĳ������ ���������� world direction���� ��ȯ�ϰ� �� ������ �������� ���� 
        // ���ο� ���� ����� �޸� �����ϴ� ��� ����� Vector3���� inputMoveXZ ��Ȱ��

        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            Quaternion cameraRotation = Cameratr.rotation;
            cameraRotation.x = cameraRotation.z = 0;    //y�ุ �ʿ��ϹǷ� ������ ���� 0���� �ٲ۴�.
            //�ڿ��������� ���� Slerp�� ȸ����Ų��.
            GetComponent<Transform>().rotation = Quaternion.Slerp(GetComponent<Transform>().rotation, cameraRotation, 10.0f * Time.deltaTime);
            if (inputMoveXZ != Vector3.zero)  //Quaternion.LookRotation�� (0,0,0)�� ���� ��� ���Ƿ� ����ó�� ���ش�.
            {
                Quaternion characterRotation = Quaternion.LookRotation(inputMoveXZ);
                characterRotation.x = characterRotation.z = 0;
                model.rotation = Quaternion.Slerp(model.rotation, characterRotation, 10.0f * Time.deltaTime);
            }   // ĳ���� ���� ������ ������ ������ ������������ Slerp�� ���� ȸ����Ų��.
        }
    }

    void rotateCharacterWhileAiming()   // ī�޶� ��������� ĳ���� ���������� ��ġ�ϴ� ȸ��
    {
        Quaternion CameraRotation = Quaternion.LookRotation(Cameratr.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, CameraRotation, 10.0f * Time.deltaTime);
        model.rotation = Quaternion.Slerp(model.rotation, transform.rotation, 10.0f * Time.deltaTime);
    }
}
