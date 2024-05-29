using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public sealed class PlayerCharacterMovement : MonoBehaviour
{
    [Header("# ĳ���� ���� (�ӽ�)")]
    public float m_GravityMultiplier = 0.1f;

    /// <summary>
    /// �̵� �Է� ���¸� ��Ÿ���ϴ�.
    /// </summary>
    private bool _IsMovementInput;

    /// <summary>
    /// �̵� �Է� �� ���� ����ϱ� ���� ����
    /// </summary>
    private Vector2 _MovementInputAxis;


    /// <summary>
    /// ĳ���Ϳ� ����� ���� �ӵ��� ��Ÿ���ϴ�.
    /// </summary>
    private Vector3 _VerticalVelocity;

    



    /// <summary>
    /// CharacterController ������Ʈ�� ��Ÿ���ϴ�.
    /// </summary>
    private CharacterController _CharacterController;

    /// <summary>
    /// CharacterController ������Ʈ�� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public CharacterController characterController => _CharacterController ??
        (_CharacterController = GetComponent<CharacterController>());

    /// <summary>
    /// ĳ���Ϳ� ����� �߷¿� ���� �б� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public Vector3 gravity => Physics.gravity * m_GravityMultiplier;


    public void Update()
    {
        // �̵� ó��
        Movement();

        
    }


    /// <summary>
    /// �̵��� ���õ� ������ ��� �̰����� �����մϴ�.
    /// </summary>
    private void Movement()
    {
        // �̵� �Է��� ���� ���� �������� ��ȯ�մϴ�.
        Vector3 worldDirection = InputToWorldDirection();

        // ���� / �߷��� ����մϴ�.
        CalculateJumpAndGravity();

        // �̵�
        characterController.Move(
            (worldDirection * Time.deltaTime) + 
            (_VerticalVelocity * Time.deltaTime));

    }

    /// <summary>
    /// �Է°��� ���� �������� ��ȯ�մϴ�.
    /// </summary>
    /// <returns></returns>
    private Vector3 InputToWorldDirection()
    {
        Vector3 worldDirection = new Vector3(_MovementInputAxis.x, 0.0f, _MovementInputAxis.y);

        // ���� ������ ��ȯ�մϴ�.
        return worldDirection.normalized; 
    }

    /// <summary>
    /// ���� / �߷��� ����մϴ�.
    /// </summary>
    private void CalculateJumpAndGravity()
    {
        // ���� �ӵ��� �߷��� ���Ͽ� �����ϵ��� �մϴ�.
        _VerticalVelocity += gravity * Time.deltaTime;
    }


    /// <summary>
    /// �̵� �Է� �� ȣ��Ǵ� �޼����Դϴ�.
    /// </summary>
    /// <param name="inputAxis">Ű���� �Է� �� ���� ���޵˴ϴ�.</param>
    public void OnMovementInput(Vector2 inputAxis)
    {
        _IsMovementInput = (_MovementInputAxis = inputAxis).sqrMagnitude != 0.0f;

    }


}
