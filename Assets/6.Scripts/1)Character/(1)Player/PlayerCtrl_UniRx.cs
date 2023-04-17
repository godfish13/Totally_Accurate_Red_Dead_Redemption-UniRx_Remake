using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerCtrl_UniRx : MonoBehaviour
{
    [SerializeField] private Transform Cameratr;
    [SerializeField] private Transform model;    //캐릭터 모델 회전을 위해 캐릭터모델 불러두기
    [SerializeField] private float MSpeed = 3.0f; //이동속도

    void Start()
    {
        this.UpdateAsObservable().Select(_ => new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")))
            .Where(input => input.magnitude > 0.5f)
            .Subscribe(input => Move(input));           // 캐릭터 이동

        this.UpdateAsObservable()
            .Subscribe(input =>
            {
                if(Input.GetMouseButton(1)) rotateCharacterWhileAiming();   // 조준 상태 회전             
                else rotateCharacter();                                     // 비조준 상태 회전
            });         // 캐릭터 회전           // where 오퍼레이터 사용보다 if문이 더 간결한 상황이므로 if문 사용
    }

    void Move(Vector3 input)
    {
        if (Input.GetMouseButton(1)) MSpeed = 5.0f; // 캐릭터 조준상태에 따른 이동속도 변화
        else MSpeed = 10.0f;
        Vector3 movedir = (Vector3.forward * input.z) + (Vector3.right * input.x);
        GetComponent<Transform>().Translate(movedir.normalized * MSpeed * Time.deltaTime, Space.Self);
    }

    void rotateCharacter()      // 카메라기준 캐릭터 회전 == 일반 회전
    {
        Vector3 inputMoveXZ = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); // 전진방향 연산
        inputMoveXZ = GetComponent<Transform>().TransformDirection(inputMoveXZ);  // TransformDirection : 로컬 direction을 world direction으로 변환해줌 
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            Quaternion cameraRotation = Cameratr.rotation;
            cameraRotation.x = cameraRotation.z = 0;    //y축만 필요하므로 나머지 값은 0으로
            GetComponent<Transform>().rotation = Quaternion.Slerp(GetComponent<Transform>().rotation, cameraRotation, 10.0f * Time.deltaTime);
            if (inputMoveXZ != Vector3.zero)  //Quaternion.LookRotation는 (0,0,0)이 들어가면 경고를 내므로 예외처리
            {
                Quaternion characterRotation = Quaternion.LookRotation(inputMoveXZ);
                characterRotation.x = characterRotation.z = 0;
                model.rotation = Quaternion.Slerp(model.rotation, characterRotation, 10.0f * Time.deltaTime);
            }   // 캐릭터 모델의 방향을 위에서 연산한 전진방향으로 Slerp를 통해 회전
        }
    }

    void rotateCharacterWhileAiming()   // 카메라 전진방향과 캐릭터 전진방향이 일치하는 회전 == 조준 중 회전
    {
        Quaternion CameraRotation = Quaternion.LookRotation(Cameratr.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, CameraRotation, 10.0f * Time.deltaTime);
        model.rotation = Quaternion.Slerp(model.rotation, transform.rotation, 10.0f * Time.deltaTime);
    }
}