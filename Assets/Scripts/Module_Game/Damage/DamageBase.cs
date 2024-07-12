using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Damage(����) ��ü�Դϴ�.
/// </summary>
public abstract class DamageBase
{
    /// <summary>
    /// �����ڸ� ��Ÿ���ϴ�.
    /// </summary>
    public Transform from { get; private set; }

    /// <summary>
    /// ���� ���ط��� ��Ÿ���ϴ�.
    /// </summary>
    public float damage { get; private set; }

    /// <summary>
    ///  ũ��Ƽ�� �����
    /// </summary>
    public bool isCriticalDamage { get; private set; }

    public DamageBase(Transform from, float damage, bool isCriticalDamage)
    {
        this.from = from;
        this.damage = damage;
        this.isCriticalDamage = isCriticalDamage;
    }


    /// <summary>
    /// ���ظ� �����ϴ�
    /// </summary>
    /// <param name="to">�����ڸ� �����մϴ�.</param>
    /// <param name="damageInstance">Damage ��ü�� �����մϴ�.</param>
    public static void Hit(IDamageable to, DamageBase damageInstance) 
    {
       
        // ���ظ� �����ϴ�.
        to.OnHit(damageInstance);

    }

    /// <summary>
    /// �� ���⿡�� ���ظ� �Ծ������� ���� ���θ� Ȯ���մϴ�.
    /// </summary>
    /// <param name="damagedTransform">���ظ� ���� GameObject �� Transform �� �����մϴ�.</param>
    /// <returns></returns>
    public bool IsDamagedFromBackward(Transform damagedTransform)
    {
        return true;
    }


}
