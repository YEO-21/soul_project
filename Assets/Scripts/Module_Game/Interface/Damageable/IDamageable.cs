using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 피해를 입을 수 있는 객체에서 구현하는 인터페이스입니다.
/// </summary>
public interface IDamageable
{
    string objectName { get; }
    float currentHp { get; }
    float maxHp { get; }

    /// <summary>
    /// 피해를 입었을 경우 호출됩니다.
    /// </summary>
    /// <param name="damageInstance"></param>
    void OnHit(DamageBase damageInstance);



}
