using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// �� ĳ���� �ൿ ���� ������Ʈ�Դϴ�.
/// </summary>
public abstract class EnemyBehaviorController : BehaviorController
{
    /// <summary>
    /// �� �� ĳ���Ͱ� ������ ��ġ�� ��Ÿ���ϴ�.
    /// </summary>
    public const string KEY_SPAWNPOSITION = "OriginPosition";


    /// <summary>
    /// ������ ��ġ���� �̵� ������ �ݰ��� ��Ÿ���ϴ�.
    /// </summary>
    public const string KEY_MAXMOVEDISTANCE = "MaxMoveDistance";

    /// <summary>
    /// �̵� ��ǥ ��ġ�� ��Ÿ���ϴ�.
    /// </summary>
    public const string KEY_TARGETPOSITION = "TargetPosition";

    private NavMeshAgent _NavMeshAgent;

    public NavMeshAgent agent => 
        _NavMeshAgent ?? (_NavMeshAgent = GetComponent<NavMeshAgent>());

    protected virtual void Awake()
    {
        // �ִ� �̵� ���� �ݰ� Ű �߰�
        SetKey(KEY_MAXMOVEDISTANCE);

        // ���� ��ġ Ű �߰�
        SetKey(KEY_SPAWNPOSITION, transform.position);

        // �̵� ��ǥ Ű �߰�
        SetKey(KEY_TARGETPOSITION);
    }


}
