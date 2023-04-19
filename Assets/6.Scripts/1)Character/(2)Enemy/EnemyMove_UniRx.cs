using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx;
using UniRx.Triggers;
using System;

public class EnemyMove_UniRx : MonoBehaviour
{
    [SerializeField] private EnemyAI_UniRx _EnemyAI_UniRx;
    public List<Transform> wayPoints;
    public int Index = 0;

    private NavMeshAgent agent;

    private readonly float patrolSpeed = 6.0f;
    private readonly float traceSpeed = 30.0f;

    public bool isPatrolling;  //���� ���� �Ǵ�
    public bool patrolling      // getter setter�� patrol�� trace Ȱ��/��Ȱ��ȭ
    {
        get
        {
            return isPatrolling;
        }
        set
        {
            isPatrolling = value;
            if (isPatrolling)
            {
                agent.speed = patrolSpeed;
                MoveWayPoint();
            }
        }
    }

    private Vector3 Target;
    public Vector3 traceTarget
    {
        get
        {
            return Target;
        }
        set
        {
            Target = value;
            agent.speed = traceSpeed;
            TraceTarget(Target);
        }
    }

    void Start()
    {
        var group = GameObject.Find("WayPointGroup");
        if (group != null)
        {
            group.GetComponentsInChildren<Transform>(wayPoints);
            wayPoints.RemoveAt(0);  // WayPointGroup(�θ������Ʈ) transform ����
        }

        agent = GetComponent<NavMeshAgent>();
        agent.speed = patrolSpeed;
        agent.autoBraking = false;

        IDisposable UpdateStream = Observable.EveryUpdate().Subscribe(_ => Move());
        //����� Update��Ʈ�� dispose�� �� �ְ� IDisposable�� ����
        _EnemyAI_UniRx.isDeadObservable
            .Where(OnNextValue => OnNextValue == true)
            .First()
            .Subscribe(_ => 
            {
                this.enabled = false;
                UpdateStream.Dispose();
            }, () => Debug.Log("EnemyMove stream OnCompleted"));
    }

    void Move()
    {
        if (!isPatrolling) return;
        // agent�� �̵����̰� ������ ���������� ���� ������ ��� �� �̵� �簳
        if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f && agent.remainingDistance <= 0.5f)
        {                                           // Magnitude�� �����ٿ����� �����ϹǷ� ���� // �̹� �����ٰ��� ������ sqrMagnitude ���
            Index = UnityEngine.Random.Range(0, wayPoints.Count);
            MoveWayPoint();                         // (Magnitude > 0.2f) == (sqrMagnitude > 0.2f * 0.2f)
        }
    }

    void MoveWayPoint()
    {
        if (agent.isPathStale)  //�ִܰ�� ��� ������� ���� ���� x
        {
            return;
        }

        agent.destination = wayPoints[Index].position;
        agent.isStopped = false;
    }

    void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale)
        {
            return;
        }

        agent.destination = pos;
        agent.isStopped = false;
    }

    public void Stop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        isPatrolling = false;
    }
}
