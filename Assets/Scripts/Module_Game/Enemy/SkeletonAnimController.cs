using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SkeletonAnimController : AnimController
{
    private const string PARAM_SPEED = "_Speed";
    private const string PARAM_ISDAMAGED = "_IsDamaged";
    private const string PARAM_ISCRITICALDAMAGE = "_IsCriticalDamage";
    private const string PARAM_DAMAGEDDIRECTIONZ = "_DamagedDirectionZ";
    private const string PARAM_ATTACKREQUESTED = "_AttackRequested";

    private EnemySkeleton _Skeleton;

    #region 이벤트
    /// <summary>
    /// 공격 영역 검사 시작 이벤트
    /// </summary>
    public event System.Action onAttackAreaCheckStarted;

    /// <summary>
    /// 공격 영역 검사 끝 이벤트
    /// </summary>
    public event System.Action onAttackAreaCheckFinished;

    /// <summary>
    /// 공격 애니메이션 끝 이벤트
    /// </summary>
    public event System.Action onAttackAnimationFinished;

    #endregion


    public void Initialize(EnemySkeleton skeleton)
    {
        _Skeleton = skeleton;

        skeleton.onMoveSpeedChanged += CALLBACK_OnMoveSpeedChanged;
        skeleton.onHit += CALLBACK_OnDamaged;

        skeleton.attack.onAttackStarted += CALLBACK_OnAttackStarted;
    }

    private void CALLBACK_OnMoveSpeedChanged(float speed)
    {
        SetParam(PARAM_SPEED, speed);
    }

    private void CALLBACK_OnDamaged(DamageBase damgeInstance)
    {
        float damagedDirectionZ = damgeInstance.IsDamagedFromBackward(_Skeleton.transform) ? -1.0f : 1.0f;

        // 피해입음 설정
        if (damgeInstance.isCriticalDamage) SetParam(PARAM_ISCRITICALDAMAGE);
        else SetParam(PARAM_ISDAMAGED);

        // 피해 입은 방향 설정
        SetParam(PARAM_DAMAGEDDIRECTIONZ, damagedDirectionZ);

    }

    private void CALLBACK_OnAttackStarted()
    {
        SetParam(PARAM_ATTACKREQUESTED);
    }

    /// <summary>
    /// 공격 애니메이션 끝 함수
    /// </summary>
    private void AnimEvent_OnAttackAnimFinished()
        => onAttackAnimationFinished?.Invoke();

    /// <summary>
    /// 공격 영역 검사 시작 함수
    /// </summary>
    private void AnimEvent_OnAttackAreaCheckStarted()
        => onAttackAreaCheckStarted.Invoke();

    /// <summary>
    /// 공격 영역 검사 끝 함수
    /// </summary>
    private void AnimEvent_OnAttackAreaCheckFinished()
        => onAttackAreaCheckFinished.Invoke();

}
