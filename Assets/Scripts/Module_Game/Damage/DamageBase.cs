using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Damage(피해) 객체입니다.
/// </summary>
public abstract class DamageBase
{
    /// <summary>
    /// 유발자를 나타냅니다.
    /// </summary>
    public Transform from { get; private set; }

    /// <summary>
    /// 입힐 피해량을 나타냅니다.
    /// </summary>
    public float damage { get; private set; }

    /// <summary>
    ///  크리티컬 대미지
    /// </summary>
    public bool isCriticalDamage { get; private set; }

    public DamageBase(Transform from, float damage, bool isCriticalDamage)
    {
        this.from = from;
        this.damage = damage;
        this.isCriticalDamage = isCriticalDamage;
    }


    /// <summary>
    /// 피해를 입힙니다
    /// </summary>
    /// <param name="to">피해자를 전달합니다.</param>
    /// <param name="damageInstance">Damage 객체를 전달합니다.</param>
    public static void Hit(IDamageable to, DamageBase damageInstance) 
    {
       

        // 피해를 입힙니다.
        to.OnHit(damageInstance);

    }

}
