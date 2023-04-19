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
    private EnemyMove_UniRx enemyMove;
    private PlayerStatus playerStatus;

    private Animator EnemyModelanimator;
    private readonly int hashMove = Animator.StringToHash("isMove");
    private readonly int hashDead = Animator.StringToHash("isDead");

    [SerializeField] private float AttackDist = 5.0f;
    [SerializeField] private float TraceDist = 10.0f;
    [SerializeField] private float distBetweenEnemyPlayer;
    [SerializeField] IntReactiveProperty HP = new IntReactiveProperty(10);
    [SerializeField] private BoolReactiveProperty isDead = new BoolReactiveProperty(false);
    public IObservable<bool> isDeadObservable => isDead;        // isDead�� �����ȣ�� �����Ͽ� ��ũ��Ʈ���� �ڽ��� ��Ȱ��ȭ��Ű���� observer���� ����

    public bool isFire { get; set; }    // private + ������Ƽ�̸� serializefield�ص� inspecter�� ���� �ȵ�

    private WaitForSeconds ws = new WaitForSeconds(0.3f);

    private void Awake()
    {
        enemyMove = GetComponent<EnemyMove_UniRx>();
        EnemyModelanimator = GetComponentInChildren<Animator>();

        var player = GameObject.FindGameObjectWithTag("PLAYER");
        if (player != null)
        {
            Playertr = player.GetComponent<Transform>();
        }
        playerStatus = player.GetComponent<PlayerStatus>();
    }

    private void Start()
    {
        StartCoroutine(CheckState());
        StartCoroutine(Action());

        HP.AsObservable()
            .Where(HP => (HP <= 0))     // ������¸�
            .First()        // where ���͸� ����Ͽ� ���� ����(���������) OnNext �� OnCompleted ����
            .Subscribe(_ =>
            {
                playerStatus.SendMessage("KillPlus");
                isDead.Value = true;
                isFire = false;
                EnemyModelanimator.SetBool(hashDead, true);
                enemyMove.Stop();
                GetComponent<CapsuleCollider>().enabled = false;
                this.enabled = false;
            }, () => Debug.Log("OnCompleted HP stream"));
    }

    IEnumerator CheckState()        // Enemy �� ���� ��ȯ
    {
        while (!isDead.Value)
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
        while (!isDead.Value)
        {
            yield return ws;
            switch (state)
            {
                case State.Patrol:
                    isFire = false;
                    enemyMove.patrolling = true;
                    EnemyModelanimator.SetBool(hashMove, true);
                    break;
                case State.Trace:
                    isFire = false;
                    enemyMove.traceTarget = Playertr.position;
                    EnemyModelanimator.SetBool(hashMove, true);
                    break;
                case State.Attack:
                    enemyMove.Stop();
                    if (isFire == false)
                        isFire = true;
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
