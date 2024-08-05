using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 기본적인 플레이어 입력을 받을 수 있는 객체를 나타내기 위해
/// 사용되는 인터페이스입니다.
/// </summary>
public interface IDefaultPlayerInputReceivable
{
    /// <summary>
    /// 이동
    /// </summary>
    /// <param name="inputAxis"></param>
    void OnMovementInput(Vector2 inputAxis);

    /// <summary>
    /// 점프 입력
    /// </summary>
    void OnJumpInput();

    /// <summary>
    /// 걷기 달리기
    /// </summary>
    /// <param name="isPressed"></param>
    void OnSprintInput(bool isPressed);

    /// <summary>
    /// 기본 공격
    /// </summary>
    void OnNormalAttackInput();

    /// <summary>
    /// 첫번째 아이템 사용
    /// </summary>
    void OnUseItem1();

    /// <summary>
    /// 방어 입력
    /// </summary>
    /// <param name="isPressed"></param>
    void OnGuardInput(bool isPressed);

    /// <summary>
    /// 상호작용 입력
    /// </summary>
    void OnInteractInput();

    /// <summary>
    /// UI 닫기 입력
    /// </summary>
    void OnCloseUIInput();

}
