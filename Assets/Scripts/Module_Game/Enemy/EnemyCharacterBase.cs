using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// �� ��ü�� ��Ÿ���� ���� Ŭ�����Դϴ�.
/// </summary>
public abstract class EnemyCharacterBase : MonoBehaviour,
    IDamageable
{
    private EnemyBehaviorController _BehaviorController;

    public BehaviorController behaviorController => _BehaviorController ??
        (_BehaviorController = GetComponent<EnemyBehaviorController>());

    private NavMeshAgent _NavAgent;
    public NavMeshAgent navAgent => _NavAgent ?? (_NavAgent = GetComponent<NavMeshAgent>());

    // ���ظ� ���� ��� �߻��ϴ� �̺�Ʈ
    #region �̺�Ʈ
    public event System.Action<DamageBase> onHit;
    #endregion


    /// <summary>
    /// �� ĳ���Ͱ� ���ظ� ���� ��� ȣ��Ǵ� �޼����Դϴ�.
    /// </summary>
    /// <param name="damage"></param>
    public virtual void OnHit(DamageBase damageInstance) => onHit?.Invoke(damageInstance);





}
