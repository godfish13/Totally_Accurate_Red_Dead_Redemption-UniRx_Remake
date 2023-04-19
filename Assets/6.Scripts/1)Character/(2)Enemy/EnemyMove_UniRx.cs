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

    public bool isPatrolling;  //순찰 여부 판단
    public bool patrolling      // getter setter로 patrol과 trace 활성/비활성화
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
            wayPoints.RemoveAt(0);  // WayPointGroup(부모오브젝트) transform 삭제
        }

        agent = GetComponent<NavMeshAgent>();
        agent.speed = patrolSpeed;
        agent.autoBraking = false;

        IDisposable UpdateStream = Observable.EveryUpdate().Subscribe(_ => Move());
        //사망시 Update스트림 dispose할 수 있게 IDisposable로 선언
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
        // agent가 이동중이고 목적지 도착했으면 다음 목적지 계산 및 이동 재개
        if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f && agent.remainingDistance <= 0.5f)
        {                                           // Magnitude는 제곱근연산을 실행하므로 느림 // 이미 제곱근값을 가지는 sqrMagnitude 사용
            Index = UnityEngine.Random.Range(0, wayPoints.Count);
            MoveWayPoint();                         // (Magnitude > 0.2f) == (sqrMagnitude > 0.2f * 0.2f)
        }
    }

    void MoveWayPoint()
    {
        if (agent.isPathStale)  //최단경로 계산 미종료시 다음 실행 x
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
