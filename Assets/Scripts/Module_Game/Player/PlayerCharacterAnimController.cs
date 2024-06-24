using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// �÷��̾� ĳ���� �ִϸ����͸� �����ϱ� ���� ������Ʈ�Դϴ�.
/// </summary>
public sealed class PlayerCharacterAnimController : AnimController
{
    private const string PARAM_MOVESPEED = "_MoveSpeed";
    private const string PARAM_ONAIR = "_IsOnAir";
    private const string PARAM_DODGEROLLREQUESTED = "_DodgeRollRequested";
    private const string PARAM_ATTACKCODE = "_AttackCode";
    private const string PARAM_ISATTACKING = "_IsAttacking";


    public float m_MoveSpeedBlend = 10.0f;

    /// <summary>
    /// �ִϸ����� �Ķ���Ϳ� ������ �̵��ӷ�
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
    #endregion


    private bool _IsOnAir;

    public void Initailize(PlayerCharacterMovement movement, PlayerCharacterAttack attack)
    {
        // �̵� �ӷ� ���� �̺�Ʈ ���ε�
        movement.onHorizontalSpeedChanged += CALLBACK_OnHorizontalMoveSpeedChanged;

        // ���� ���� �ݹ� ���
        movement.onJumpStarted += CALLBACK_OnJumpStarted;

        // ���� �� �ݹ� ���
        movement.onGrounded += CALLBACK_OnGrounded;

        // ���ϱ� ���� �ݹ� ���
        movement.onDodgeRollStarted += CALLBACK_OnDodgeRollStarted;

        // ���� ���� �ݹ� ���
        attack.onAttackStarted += CALLBACK_OnAttackStarted;
    }

    private void Movement_onJumpStarted()
    {
        throw new System.NotImplementedException();
    }

    private void Update()
    {
        _MoveSpeed = Mathf.Lerp(_MoveSpeed, _TargetMoveSpeed, m_MoveSpeedBlend * Time.deltaTime);
        SetParam(PARAM_MOVESPEED, _MoveSpeed);
        SetParam(PARAM_ONAIR, _IsOnAir);
    }

    /// <summary>
    /// ���� �̵� �ӷ��� ����Ǿ��� ��� ȣ��Ǵ� �޼����Դϴ�.
    /// </summary>
    /// <param name="speed"></param>
    private void CALLBACK_OnHorizontalMoveSpeedChanged(float speed) => _TargetMoveSpeed = speed;

    /// <summary>
    /// ���� ���� �� ȣ��Ǵ� �޼����Դϴ�.
    /// </summary>
    private void CALLBACK_OnJumpStarted() => _IsOnAir = true;

    /// <summary>
    /// ���� ����� ��� ȣ��Ǵ� �޼����Դϴ�.
    /// </summary>
    private void CALLBACK_OnGrounded() => _IsOnAir = false;

    /// <summary>
    /// ���ϱ� ���� �� ȣ��Ǵ� �޼����Դϴ�.
    /// </summary>
    private void CALLBACK_OnDodgeRollStarted() => SetParam(PARAM_DODGEROLLREQUESTED);

    /// <summary>
    /// ���� ���� �� ȣ��Ǵ� �޼����Դϴ�.
    /// </summary>
    /// <param name="attackCode"></param>
    private void CALLBACK_OnAttackStarted(int attackCode) => SetParam(PARAM_ATTACKCODE, attackCode);

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
    private void AnimEvent_OnAttackAreaCheckStarted()
    {
        onAttackAreaCheckStarted?.Invoke();
    }

    /// <summary>
    /// ���� ���� Ȯ�� ����
    /// </summary>
    private void AnimEvent_OnAttackAreaCheckFinished()
    {
        onAttackAreaCheckFinished?.Invoke();
    }

}
