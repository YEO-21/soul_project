using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 적 캐릭터 행동 제어 컴포넌트입니다.
/// </summary>
public abstract class EnemyBehaviorController : BehaviorController
{
    /// <summary>
    /// 이 적 캐릭터가 생성된 위치를 나타냅니다.
    /// </summary>
    public const string KEY_SPAWNPOSITION = "OriginPosition";


    /// <summary>
    /// 생성된 위치부터 이동 가능한 반경을 나타냅니다.
    /// </summary>
    public const string KEY_MAXMOVEDISTANCE = "MaxMoveDistance";

    /// <summary>
    /// 이동 목표 위치를 나타냅니다.
    /// </summary>
    public const string KEY_TARGETPOSITION = "TargetPosition";

    private NavMeshAgent _NavMeshAgent;

    public NavMeshAgent agent => 
        _NavMeshAgent ?? (_NavMeshAgent = GetComponent<NavMeshAgent>());

    protected virtual void Awake()
    {
        // 최대 이동 가능 반경 키 추가
        SetKey(KEY_MAXMOVEDISTANCE);

        // 생성 위치 키 추가
        SetKey(KEY_SPAWNPOSITION, transform.position);

        // 이동 목표 키 추가
        SetKey(KEY_TARGETPOSITION);
    }


}
