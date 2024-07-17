using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 칼 무기를 나타내는 컴포넌트입니다.
/// </summary>
public sealed class WeaponSword : WeaponBase
{
    [Header("# 칼 파티클 시스템")]
    public ParticleSystem m_SwordParticle;


    private void Start()
    {
        SetActiveSwordParticle(false);
    }

    public override void StartAttackAreaCheck()
    {
        base.StartAttackAreaCheck();
        SetActiveSwordParticle(true);
    }

    public override void StopAttackAreaCheck()
    {
        base.StopAttackAreaCheck();
        SetActiveSwordParticle(false);
    }

    private void SetActiveSwordParticle(bool active)
    {
        if (active) m_SwordParticle.Play();
        else m_SwordParticle.Stop();
    }

}
