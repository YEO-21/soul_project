using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 적 객체를 나타내기 위한 클래스입니다.
/// </summary>
public abstract class EnemyCharacterBase : MonoBehaviour,
    IDamageable
{
    private EnemyBehaviorController _BehaviorController;

    public BehaviorController behaviorController => _BehaviorController ??
        (_BehaviorController = GetComponent<EnemyBehaviorController>());

    private NavMeshAgent _NavAgent;
    public NavMeshAgent navAgent => _NavAgent ?? (_NavAgent = GetComponent<NavMeshAgent>());

    // 피해를 입을 경우 발생하는 이벤트
    #region 이벤트
    public event System.Action<DamageBase> onHit;
    #endregion


    /// <summary>
    /// 이 캐릭터가 피해를 입을 경우 호출되는 메서드입니다.
    /// </summary>
    /// <param name="damage"></param>
    public virtual void OnHit(DamageBase damageInstance) => onHit?.Invoke(damageInstance);





}
