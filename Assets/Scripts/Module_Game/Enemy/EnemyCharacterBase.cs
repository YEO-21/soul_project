using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적 객체를 나타내기 위한 클래스입니다.
/// </summary>
public abstract class EnemyCharacterBase : MonoBehaviour
{
    /// <summary>
    /// 이 캐릭터가 피해를 입을 경우 호출되는 메서드입니다.
    /// </summary>
    /// <param name="damage"></param>
    public virtual void OnHit(DamageBase damage)
    {
        Debug.Log("damage = " + damage);
    }



}
