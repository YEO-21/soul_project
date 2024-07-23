using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Į ���⸦ ��Ÿ���� ������Ʈ�Դϴ�.
/// </summary>
public sealed class WeaponSword : WeaponBase
{
    [Header("# Į ��ƼŬ �ý���")]
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
