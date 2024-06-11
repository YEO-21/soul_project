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

    public float m_MoveSpeedBlend = 10.0f;

    /// <summary>
    /// �ִϸ����� �Ķ���Ϳ� ������ �̵��ӷ�
    /// </summary>
    private float _MoveSpeed;

    private float _TargetMoveSpeed;

    private bool _IsOnAir;

    public void Initailize(PlayerCharacterMovement movement)
    {
        // �̵� �ӷ� ���� �̺�Ʈ ���ε�
        movement.onHorizontalSpeedChanged += CALLBACK_OnHorizontalMoveSpeedChanged;

        // ���� ���� �ݹ� ���
        movement.onJumpStarted += CALLBACK_OnJumpStarted;

        // ���� �� �ݹ� ���
        movement.onGrounded += CALLBACK_OnGrounded;
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

}
