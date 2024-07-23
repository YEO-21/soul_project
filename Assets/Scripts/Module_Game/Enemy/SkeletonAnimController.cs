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

    #region �̺�Ʈ
    /// <summary>
    /// ���� ���� �˻� ���� �̺�Ʈ
    /// </summary>
    public event System.Action onAttackAreaCheckStarted;

    /// <summary>
    /// ���� ���� �˻� �� �̺�Ʈ
    /// </summary>
    public event System.Action onAttackAreaCheckFinished;

    /// <summary>
    /// ���� �ִϸ��̼� �� �̺�Ʈ
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

        // �������� ����
        if (damgeInstance.isCriticalDamage) SetParam(PARAM_ISCRITICALDAMAGE);
        else SetParam(PARAM_ISDAMAGED);

        // ���� ���� ���� ����
        SetParam(PARAM_DAMAGEDDIRECTIONZ, damagedDirectionZ);

    }

    private void CALLBACK_OnAttackStarted()
    {
        SetParam(PARAM_ATTACKREQUESTED);
    }

    /// <summary>
    /// ���� �ִϸ��̼� �� �Լ�
    /// </summary>
    private void AnimEvent_OnAttackAnimFinished()
        => onAttackAnimationFinished?.Invoke();

    /// <summary>
    /// ���� ���� �˻� ���� �Լ�
    /// </summary>
    private void AnimEvent_OnAttackAreaCheckStarted()
        => onAttackAreaCheckStarted.Invoke();

    /// <summary>
    /// ���� ���� �˻� �� �Լ�
    /// </summary>
    private void AnimEvent_OnAttackAreaCheckFinished()
        => onAttackAreaCheckFinished.Invoke();

}
