using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 기본적인 플레이어 입력을 받을 수 있는 객체를 나타내기 위해
/// 사용되는 인터페이스입니다.
/// </summary>
public interface IDefaultPlayerInputReceivable
{
    // 이동
    void OnMovementInput(Vector2 inputAxis);

    // 점프 입력
    void OnJumpInput();

    // 걷기 달리기
    void OnSprintInput(bool isPressed);

    // 기본 공격
    void OnNormalAttackInput();

}
