using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotion : MonoBehaviour
{
    private PlayerCtrl playerCtrl;

    public Animator animator;
    private readonly int isMove = Animator.StringToHash("isMove");
    private readonly int isShooting = Animator.StringToHash("isShooting");

    // Start is called before the first frame update
    void Start()
    {
        playerCtrl = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<PlayerCtrl>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCtrl.h != 0 || playerCtrl.v != 0)
        {
            animator.SetBool(isMove, true);
        }
        else if (playerCtrl.h == 0 || playerCtrl.v == 0)
        {
            animator.SetBool(isMove, false);
        }

        if(playerCtrl.isAiming)
        {
            animator.SetBool(isShooting, true);
        }
        else if (!playerCtrl.isAiming)
        {
            animator.SetBool(isShooting, false);
        }

    }
}
