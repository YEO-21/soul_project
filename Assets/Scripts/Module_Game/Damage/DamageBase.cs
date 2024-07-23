using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Damage 객체입니다.
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
    /// 크리티컬 대미지
    /// </summary>
    public bool isCriticalDamage { get; private set; }

    public DamageBase(Transform from, float damage, bool isCriticalDamage)
    {
        this.from = from;
        this.damage = damage;
        this.isCriticalDamage = isCriticalDamage;
    }

    /// <summary>
    /// 피해를 입힙니다.
    /// </summary>
    /// <param name="to">피해자를 전달합니다.</param>
    /// <param name="damageInstance">Damage 객체를 전달합니다.</param>
    public static void Hit(IDamageable to, DamageBase damageInstance) 
    {
        // 피해를 입힙니다.
        to.OnHit(damageInstance);
    }

    /// <summary>
    /// 뒷 방향에서 피해를 입었는지에 대한 여부를 확인합니다.
    /// </summary>
    /// <param name="damagedTransform">피해를 입은 GameObject 의 Transform 을 전달합니다.</param>
    /// <returns></returns>
    public bool IsDamagedFromBackward(Transform damagedTransform)
    {
        Vector3 thisPos = damagedTransform.transform.position;
        Vector3 fromPos = from.position;

        // 피해를 입은 방향 (플레이어로 향하는 방향)
        Vector3 damagedDirection = fromPos - thisPos;
        damagedDirection.y = 0.0f;
        damagedDirection.Normalize();

        // 앞 방향 구하기
        Vector3 thisForward = damagedTransform.forward;

        // 현재 회전 구하기
        float thisYaw = Mathf.Atan2(thisForward.z, thisForward.x) * Mathf.Rad2Deg;

        // 대미지 입은 방향에 대한 Yaw 회전값
        float damagedYaw = Mathf.Atan2(damagedDirection.z, damagedDirection.x) * Mathf.Rad2Deg;

        // 각도차 구하기
        float deltaYaw = Mathf.Abs(Mathf.DeltaAngle(thisYaw, damagedYaw));

        return deltaYaw > 90.0f;
    }

}
