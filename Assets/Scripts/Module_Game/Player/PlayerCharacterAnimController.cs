using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �÷��̾� ĳ���� �ִϸ����͸� �����ϱ� ���� ������Ʈ�Դϴ�.
/// </summary>
public sealed class PlayerCharacterAnimController : AnimController
{
    private const string PARAM_MOVESPEED = "_MoveSpeed";
    private const string PARAM_ISONAIR = "_IsOnAir";
    private const string PARAM_DODGEROLLREQUESTED = "_DodgeRollRequested";
    private const string PARAM_ATTACKCODE = "_AttackCode";
    private const string PARAM_ISATTACKING = "_IsAttacking";
    private const string PARAM_ISDAMAGED = "_IsDamaged";
    private const string PARAM_ISGUARD = "_IsGuard";

    public float m_MoveSpeedBlend = 10.0f;

    /// <summary>
    /// �ִϸ����� �Ķ���Ϳ� ������ ���� �̵��ӷ�
    /// </summary>
    private float _MoveSpeed;
    private float _TargetMoveSpeed;

    #region �̺�Ʈ
    /// <summary>
    /// ������ �ִϸ��̼� ���� �̺�Ʈ
    /// </summary>
    public event System.Action onDodgeRollAnimStarted;

    /// <summary>
    /// ������ �ִϸ��̼� �� �̺�Ʈ
    /// </summary>
    public event System.Action onDodgeRollAnimFinished;

    /// <summary>
    /// ���� �ִϸ��̼� ��� �� �̺�Ʈ
    /// </summary>
    public event System.Action onAttackAnimationFinished;

    /// <summary>
    /// ���� ���� �˻� ���� �̺�Ʈ
    /// </summary>
    public event System.Action onAttackAreaCheckStarted;

    /// <summary>
    /// ���� ���� �˻� �� �̺�Ʈ
    /// </summary>
    public event System.Action onAttackAreaCheckFinished;

    /// <summary>
    /// �������� �ִϸ��̼� ��� �� �̺�Ʈ
    /// </summary>
    public event System.Action onHitAnimationFinished;

    #endregion



    public void Initialize(PlayerCharacter playerCharacter)
    {
        // �̵� �ӷ� ���� �ݹ� ���
        playerCharacter.movement.onHorizontalSpeedChanged += CALLBACK_OnHorizontalMoveSpeedChanged;

        // ���� ���� �ݹ� ���
        playerCharacter.movement.onJumpStarted += CALLBACK_OnJumpStarted;

        // ���� �� �ݹ� ���
        playerCharacter.movement.onGrounded += CALLBACK_OnGrounded;

        // ���ϱ� ���� �ݹ� ���
        playerCharacter.movement.onDodgeRollStarted += CALLBACK_OnDodgeRollStarted;

        // ���� ���� �ݹ� ���
        playerCharacter.attack.onAttackStarted += CALLBACK_OnAttackStarted;

        // ���� ��ҵ� �ݹ� ���
        playerCharacter.attack.onAttackCanceled += CALLBACK_OnAttackCanceled;

        // ��� ���� ����� �ݹ� ���
        playerCharacter.attack.onGuardStateUpdated += CALLBACK_OnGuardStateUpdated;

        // ���� ���� �ݹ� ���
        playerCharacter.onHit += CALLBACK_OnHit;
    }

    /// <summary>
    /// ���� ���� �ִϸ��̼� ����� ������ ��� ȣ��˴ϴ�.
    /// </summary>
    public void OnHitAnimationFinished()
        => onHitAnimationFinished?.Invoke();

    private void Update()
    {
        _MoveSpeed = Mathf.Lerp(_MoveSpeed, _TargetMoveSpeed, m_MoveSpeedBlend * Time.deltaTime);
        SetParam(PARAM_MOVESPEED, _MoveSpeed);
    }

    /// <summary>
    /// ���� �̵� �ӷ��� ����Ǿ��� ��� ȣ��Ǵ� �޼����Դϴ�.
    /// </summary>
    /// <param name="speed"></param>
    private void CALLBACK_OnHorizontalMoveSpeedChanged(float speed) 
        => _TargetMoveSpeed = speed;

    /// <summary>
    /// ���� ���۽� ȣ��Ǵ� �޼����Դϴ�.
    /// </summary>
    private void CALLBACK_OnJumpStarted() => SetParam(PARAM_ISONAIR, true);

    /// <summary>
    /// ���� ����� ��� ȣ��Ǵ� �޼����Դϴ�.
    /// </summary>
    private void CALLBACK_OnGrounded() => SetParam(PARAM_ISONAIR, false);

    /// <summary>
    /// ���ϱ� ���۽� ȣ��Ǵ� �޼����Դϴ�.
    /// </summary>
    private void CALLBACK_OnDodgeRollStarted() => SetParam(PARAM_DODGEROLLREQUESTED);

    /// <summary>
    /// ���� ���� �� ȣ��Ǵ� �޼����Դϴ�.
    /// </summary>
    /// <param name="attackCode"></param>
    private void CALLBACK_OnAttackStarted(int attackCode) => SetParam(PARAM_ATTACKCODE, attackCode);

    /// <summary>
    /// ���ظ� ���� ��� ȣ��Ǵ� �޼����Դϴ�.
    /// </summary>
    /// <param name="damageInstance"></param>
    private void CALLBACK_OnHit(DamageBase damageInstance)
        => SetParam(PARAM_ISDAMAGED);

    /// <summary>
    /// ������ ��ҵǾ��� ��� ȣ��Ǵ� �޼����Դϴ�.
    /// </summary>
    private void CALLBACK_OnAttackCanceled()
    {
        SetParam(PARAM_ATTACKCODE, 0);
        SetParam(PARAM_ISATTACKING, false);
    }

    private void CALLBACK_OnGuardStateUpdated(bool isGuardState)
    {
        SetParam(PARAM_ISGUARD, isGuardState);
    }

    /// <summary>
    /// ���ϱ� �ִϸ��̼� ���� �̺�Ʈ
    /// </summary>
    private void AnimEvent_OnDodgeRollStarted() => onDodgeRollAnimStarted?.Invoke();

    /// <summary>
    /// ���ϱ� �ִϸ��̼� �� �̺�Ʈ
    /// </summary>
    private void AnimEvent_OnDodgeRollFinished() => onDodgeRollAnimFinished?.Invoke();

    private void AnimEvent_OnAttackSectionFinished()
    {
        // ���� �ڵ� �ʱ�ȭ
        SetParam(PARAM_ATTACKCODE, 0);

        // ���� ���� ���� �ʱ�ȭ
        SetParam(PARAM_ISATTACKING, false);

        // ���� �ִϸ��̼� �� �̺�Ʈ �߻�
        onAttackAnimationFinished?.Invoke();
    }

    /// <summary>
    /// ���� ���� Ȯ�� ���۵�
    /// </summary>
    private void AnimEvent_OnAtackAreaCheckStarted()
        => onAttackAreaCheckStarted?.Invoke();

    /// <summary>
    /// ���� ���� Ȯ�� ����
    /// </summary>
    private void AnimEvent_OnAttackAreaCheckFinished()
        => onAttackAreaCheckFinished?.Invoke();

    private void AnimEvent_PlayEffectSound01() =>
        SoundManager.instance.PlayEffectSound(SoundManager.EFFECT_SWING01, transform.position);

    private void AnimEvent_PlayEffectSound02() =>
        SoundManager.instance.PlayEffectSound(SoundManager.EFFECT_SWING02, transform.position);

    private void AnimEvent_PlayEffectSound03() =>
        SoundManager.instance.PlayEffectSound(SoundManager.EFFECT_SWING03, transform.position);
}
