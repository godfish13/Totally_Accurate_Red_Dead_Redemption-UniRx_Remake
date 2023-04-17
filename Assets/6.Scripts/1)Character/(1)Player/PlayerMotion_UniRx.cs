using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UnityEngine;
using UniRx;

public class PlayerMotion_UniRx : MonoBehaviour
{
    private Animator animator;
    private readonly int isMove = Animator.StringToHash("isMove");
    private readonly int isShooting = Animator.StringToHash("isShooting");

    void Start()
    {
        animator = GetComponent<Animator>();

        this.UpdateAsObservable().Select(_ => new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")))
            .Subscribe(input =>
            {
                if (input.z != 0 || input.x != 0)
                {
                    animator.SetBool(isMove, true);
                }
                else if (input.z == 0 || input.x == 0)
                {
                    animator.SetBool(isMove, false);
                }
            });

        this.UpdateAsObservable()
            .Subscribe(input =>
            {
                if (Input.GetMouseButton(1))        // 우클릭중일시 aiming모션
                {
                    animator.SetBool(isShooting, true);
                }
                else
                {
                    animator.SetBool(isShooting, false);
                }
            });
    }
}
