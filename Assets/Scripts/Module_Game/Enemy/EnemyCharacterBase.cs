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

    private NavMeshAgent _NavAgent;
    public NavMeshAgent navAgent => _NavAgent ?? (_NavAgent = GetComponent<NavMeshAgent>());


    /// <summary>
    /// 이 캐릭터가 피해를 입을 경우 호출되는 메서드입니다.
    /// </summary>
    /// <param name="damage"></param>
    public virtual void OnHit(DamageBase damageInstance)
    {
        Debug.Log("피해량 : " + damageInstance.damage);
        Debug.Log("유발자 : " + damageInstance.from.gameObject.name);
    }



}
