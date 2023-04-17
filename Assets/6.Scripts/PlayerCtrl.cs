using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public Transform Cameratr;
    private Transform model;    //캐릭터 모델 회전을 위해 캐릭터모델 불러두기

    public float v = 0.0f;      //이동제어 input
    public float h = 0.0f;
    public float MSpeed = 0.0f; //이동속도

    public bool isAiming;

    private void Awake()
    {
        model = GetComponent<Transform>().GetChild(0);       
    }

    // Start is called before the first frame update
    void Start()
    {                   // 캐릭터Rig 회전 비활성화
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
        Cameratr = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {                                       //movement input
        v = Input.GetAxis("Vertical");      //forward(z+), backward(z-)
        h = Input.GetAxis("Horizontal");    //left(x-), right(x+)       
        
        if (Input.GetMouseButton(1))        // 우클릭중일시 aiming모드(이동속도 절반), 아닐시 일반모드
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
        Vector3 movedir = (Vector3.forward * v) + (Vector3.right * h);      //전후좌우 이동 방향 벡터 계산
        GetComponent<Transform>().Translate(movedir.normalized * MSpeed * Time.deltaTime, Space.Self);
        //normalized로 최대크기 1인 정규벡터화(안하면 90의 사이값을 가지는 벡터 두개가 합산되어 최대크기가 1을 넘어감)
    }

    void rotateCharacter()      // 카메라기준 캐릭터 회전
    {
        Vector3 inputMoveXZ = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //좌우 input값, 위아래 input값(== 0), 전후 input값을 Vector3를 통해 저장하고 이동방향을 계산한다.
        inputMoveXZ = GetComponent<Transform>().TransformDirection(inputMoveXZ);  // TransformDirection : 로컬 direction을 world direction으로 변환해줌 
        //Vector3 MoveDir = "               // 캐릭터의 전진방향을 world direction으로 변환하고 이 방향을 기준으로 삼음 
        // 새로운 벡터 만들어 메모리 차지하는 대신 사용한 Vector3변수 inputMoveXZ 재활용

        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            Quaternion cameraRotation = Cameratr.rotation;
            cameraRotation.x = cameraRotation.z = 0;    //y축만 필요하므로 나머지 값은 0으로 바꾼다.
            //자연스러움을 위해 Slerp로 회전시킨다.
            GetComponent<Transform>().rotation = Quaternion.Slerp(GetComponent<Transform>().rotation, cameraRotation, 10.0f * Time.deltaTime);
            if (inputMoveXZ != Vector3.zero)  //Quaternion.LookRotation는 (0,0,0)이 들어가면 경고를 내므로 예외처리 해준다.
            {
                Quaternion characterRotation = Quaternion.LookRotation(inputMoveXZ);
                characterRotation.x = characterRotation.z = 0;
                model.rotation = Quaternion.Slerp(model.rotation, characterRotation, 10.0f * Time.deltaTime);
            }   // 캐릭터 모델의 방향을 위에서 연산한 전진방향으로 Slerp를 통해 회전시킨다.
        }
    }

    void rotateCharacterWhileAiming()   // 카메라 전진방향과 캐릭터 전진방향이 일치하는 회전
    {
        Quaternion CameraRotation = Quaternion.LookRotation(Cameratr.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, CameraRotation, 10.0f * Time.deltaTime);
        model.rotation = Quaternion.Slerp(model.rotation, transform.rotation, 10.0f * Time.deltaTime);
    }
}
