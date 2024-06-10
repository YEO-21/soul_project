using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ���� ������ ���� �÷��̾� ��Ʈ�ѷ� ��ü�� ��Ÿ���ϴ�.
/// </summary>
public sealed class GameScenePlayerController : PlayerControllerBase
{
    /// <summary>
    /// ����� �Է��� ó���� ��ü�Դϴ�.
    /// </summary>
    private IDefaultPlayerInputReceivable _PlayerInputReceivable;


    public override void StartControlCharacter(PlayerCharacterBase controlCharacter)
    {
        base.StartControlCharacter(controlCharacter);

        _PlayerInputReceivable = controlCharacter as IDefaultPlayerInputReceivable;

        // Ŀ���� ȭ�� �߾ӿ� ��� ��ġ�ǵ��� �մϴ�.
        Cursor.lockState = CursorLockMode.Locked;

        // Ŀ���� ǥ������ �ʵ��� �մϴ�.
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
}
