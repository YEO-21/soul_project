using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 게임 씬에서 사용될 플레이어 컨트롤러 객체를 나타냅니다.
/// </summary>
public sealed class GameScenePlayerController : PlayerControllerBase
{
    /// <summary>
    /// 사용자 입력을 처리할 객체입니다.
    /// </summary>
    private IDefaultPlayerInputReceivable _PlayerInputReceivable;

    /// <summary>
    /// GameUIPanel 객체를 나타냅니다.
    /// </summary>
    public GameUIPanel gameUI { get; private set; }


    public override void StartControlCharacter(PlayerCharacterBase controlCharacter)
    {
        base.StartControlCharacter(controlCharacter);

        _PlayerInputReceivable = controlCharacter as IDefaultPlayerInputReceivable;

        // GameUIPanel 를 찾습니다.
        gameUI = FindObjectOfType<GameUIPanel>();

        // GameUIPanel 초기화
        gameUI.InitializeUI(this);



        // 커서를 화면 중앙에 계속 배치되도록 합니다.
        Cursor.lockState = CursorLockMode.Locked;

        // 커서를 표시하지 않도록 합니다.
        Cursor.visible = false;
    }


    private void OnMovementInput(InputValue value)
    {
        Vector2 inputAxis = value.Get<Vector2>();
        _PlayerInputReceivable?.OnMovementInput(inputAxis);
    }

    private void OnJumpInput()
    {
        _PlayerInputReceivable?.OnJumpInput();
    }

    private void OnSprintInput(InputValue value)
    {
        _PlayerInputReceivable?.OnSprintInput(value.isPressed);
    }

    private void OnNormalAttackInput()
    {
        _PlayerInputReceivable?.OnNormalAttackInput();
    }
}
