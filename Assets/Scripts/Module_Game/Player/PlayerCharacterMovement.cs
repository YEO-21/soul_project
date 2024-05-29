using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public sealed class PlayerCharacterMovement : MonoBehaviour
{
    [Header("# 캐릭터 정보 (임시)")]
    public float m_GravityMultiplier = 0.1f;

    /// <summary>
    /// 이동 입력 상태를 나타냅니다.
    /// </summary>
    private bool _IsMovementInput;

    /// <summary>
    /// 이동 입력 축 값을 기록하기 위한 변수
    /// </summary>
    private Vector2 _MovementInputAxis;


    /// <summary>
    /// 캐릭터에 적용될 수직 속도를 나타냅니다.
    /// </summary>
    private Vector3 _VerticalVelocity;

    



    /// <summary>
    /// CharacterController 컴포넌트를 나타냅니다.
    /// </summary>
    private CharacterController _CharacterController;

    /// <summary>
    /// CharacterController 컴포넌트에 대한 프로퍼티입니다.
    /// </summary>
    public CharacterController characterController => _CharacterController ??
        (_CharacterController = GetComponent<CharacterController>());

    /// <summary>
    /// 캐릭터에 적용될 중력에 대한 읽기 전용 프로퍼티입니다.
    /// </summary>
    public Vector3 gravity => Physics.gravity * m_GravityMultiplier;


    public void Update()
    {
        // 이동 처리
        Movement();

        
    }


    /// <summary>
    /// 이동과 관련된 연산을 모두 이곳에서 진행합니다.
    /// </summary>
    private void Movement()
    {
        // 이동 입력을 월드 기준 방향으로 변환합니다.
        Vector3 worldDirection = InputToWorldDirection();

        // 점프 / 중력을 계산합니다.
        CalculateJumpAndGravity();

        // 이동
        characterController.Move(
            (worldDirection * Time.deltaTime) + 
            (_VerticalVelocity * Time.deltaTime));

    }

    /// <summary>
    /// 입력값을 월드 방향으로 변환합니다.
    /// </summary>
    /// <returns></returns>
    private Vector3 InputToWorldDirection()
    {
        Vector3 worldDirection = new Vector3(_MovementInputAxis.x, 0.0f, _MovementInputAxis.y);

        // 계산된 방향을 반환합니다.
        return worldDirection.normalized; 
    }

    /// <summary>
    /// 점프 / 중력을 계산합니다.
    /// </summary>
    private void CalculateJumpAndGravity()
    {
        // 수직 속도에 중력을 더하여 가속하도록 합니다.
        _VerticalVelocity += gravity * Time.deltaTime;
    }


    /// <summary>
    /// 이동 입력 시 호출되는 메서드입니다.
    /// </summary>
    /// <param name="inputAxis">키보드 입력 축 값이 전달됩니다.</param>
    public void OnMovementInput(Vector2 inputAxis)
    {
        _IsMovementInput = (_MovementInputAxis = inputAxis).sqrMagnitude != 0.0f;

    }


}
