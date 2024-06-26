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

    public DamageBase(Transform from, float damage)
    {
        this.from = from;
        this.damage = damage;
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
