using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class EnemyAI_UniRx : MonoBehaviour
{
    public enum State
    {
        Idle,
        Patrol,
        Trace,
        Attack,
        Die
    }

    public State state = State.Patrol;

    private Transform Playertr;

    [SerializeField] IntReactiveProperty HP = new IntReactiveProperty(10);
    public float AttackDist = 5.0f;
    public float TraceDist = 10.0f;
    public float distBetweenEnemyPlayer;

    public bool isMove = true;
    public bool isDead = false;

    private WaitForSeconds ws;

    private EnemyMove_UniRx enemyMove;
    private EnemyFire_UniRx enemyFire;
    private PlayerStatus playerStatus;

    private Animator EnemyModelanimator;
    private readonly int hashMove = Animator.StringToHash("isMove");
    private readonly int hashDead = Animator.StringToHash("isDead");
    private void Awake()
    {
        enemyMove = GetComponent<EnemyMove_UniRx>();
        enemyFire = GetComponent<EnemyFire_UniRx>();
        EnemyModelanimator = GetComponentInChildren<Animator>();

        var player = GameObject.FindGameObjectWithTag("PLAYER");
        if (player != null)
        {
            Playertr = player.GetComponent<Transform>();
        }
        playerStatus = player.GetComponent<PlayerStatus>();

        ws = new WaitForSeconds(0.3f);

    }

    private void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(Action());
    }
    private void Start()
    {
        StartCoroutine(CheckState());
        StartCoroutine(Action());

        HP.AsObservable()
            .Where(HP => (HP <= 0))     // 사망상태면
            .First()        // where 필터를 통과하여 값이 오면(사망했으면) OnNext 후 OnCompleted 발행
            .Subscribe(_ =>
            {
                playerStatus.SendMessage("KillPlus");
                enemyFire.enabled = false;
                isDead = true;
                enemyFire.isFire = false;
                EnemyModelanimator.SetBool(hashDead, true);
                enemyMove.Stop();
                GetComponent<CapsuleCollider>().enabled = false;
                GetComponent<EnemyMove_UniRx>().enabled = false;
                Destroy(GetComponent<EnemyAI_UniRx>());
            }, () => Debug.Log("OnCompleted HP stream"));
    }

    IEnumerator CheckState()        // Enemy 현 상태 변환
    {
        while (!isDead)
        {
            if (state == State.Die)
            {
                yield break;
            }

            distBetweenEnemyPlayer = Vector3.Distance(Playertr.position, transform.position);

            if (distBetweenEnemyPlayer <= AttackDist)
            {
                state = State.Attack;
            }
            else if (distBetweenEnemyPlayer <= TraceDist)
            {
                state = State.Trace;
            }
            else
            {
                state = State.Patrol;
            }

            yield return ws;
        }
    }

    IEnumerator Action()
    {
        while (!isDead)
        {
            yield return ws;
            switch (state)
            {
                case State.Patrol:
                    enemyFire.isFire = false;
                    enemyMove.patrolling = true;
                    EnemyModelanimator.SetBool(hashMove, true);
                    break;
                case State.Trace:
                    enemyFire.isFire = false;
                    enemyMove.traceTarget = Playertr.position;
                    EnemyModelanimator.SetBool(hashMove, true);
                    break;
                case State.Attack:
                    enemyMove.Stop();
                    if (enemyFire.isFire == false)
                        enemyFire.isFire = true;
                    EnemyModelanimator.SetBool(hashMove, false);
                    break;
            }
        }
    }

    public void Hitted()
    {
        HP.Value--;
    }
}
