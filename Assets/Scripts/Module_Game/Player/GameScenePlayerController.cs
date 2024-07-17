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

    /// <summary>
    /// GameUIPanel ��ü�� ��Ÿ���ϴ�.
    /// </summary>
    public GameUIPanel gameUI { get; private set; }


    public override void StartControlCharacter(PlayerCharacterBase controlCharacter)
    {
        base.StartControlCharacter(controlCharacter);

        _PlayerInputReceivable = controlCharacter as IDefaultPlayerInputReceivable;

        // GameUIPanel �� ã���ϴ�.
        gameUI = FindObjectOfType<GameUIPanel>();

        // GameUIPanel �ʱ�ȭ
        gameUI.InitializeUI(this);



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

    private void OnNormalAttackInput()
    {
        _PlayerInputReceivable?.OnNormalAttackInput();
    }
}
