using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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

    private void CALLBACK_OnDamaged(DamageBase damageInstance)
    {
        #region YeoStyle
        //SetParam(PARAM_ISDAMAGED);

        //// �� ĳ���� �� ���⺤��
        //Vector3 skeletonForward = _Skeleton.transform.forward;

        //// �÷��̾� ĳ���� �� ���⺤��
        //Vector3 playerForwawrd = damageInstance.from.forward;

        //// ���⺤���� �� ����
        //bool IsBackAttack = (skeletonForward.z * playerForwawrd.z) > 0;

        //float attackNumber = 0.0f;

        //// �ڿ��� ���� �޾��� ��� -1
        //// �տ��� ���� �޾��� ��� 1
        //attackNumber = IsBackAttack ? -1.0f : 1.0f;

        //SetParam(PARAM_DAMAGEDDIRECTIONZ, attackNumber);
        #endregion

        float damagedDirectionZ = damageInstance.IsDamagedFromBackward(_Skeleton.transform) ? -1.0f : 1.0f;
        

        // �������� ����
        if (damageInstance.isCriticalDamage) SetParam(PARAM_ISCRITICALDAMAGE);
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

}
