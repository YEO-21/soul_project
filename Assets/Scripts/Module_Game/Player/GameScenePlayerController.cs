using GameModule;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ���� ������ ���� �÷��̾� ��Ʈ�ѷ� ��ü�� ��Ÿ���ϴ�.
/// </summary>
public sealed class GameScenePlayerController : PlayerControllerBase
{
    private PlayerInventory _PlayerInventory;

    private PlayerInput _PlayerInput;

    /// <summary>
    /// ����� �Է��� ó���� ��ü�Դϴ�.
    /// </summary>
    private IDefaultPlayerInputReceivable _PlayerInputReceivable;

    public PlayerInput playerInput => _PlayerInput ??
        (_PlayerInput = GetComponent<PlayerInput>());

    /// <summary>
    /// �κ��丮 ������Ʈ�� ��Ÿ���ϴ�.
    /// </summary>
    public PlayerInventory inventory => _PlayerInventory ??
        (_PlayerInventory = GetComponent<PlayerInventory>());


    public override void InitializePlayerState()
    {
        playerState = new GameScenePlayerState(100.0f, 300.0f);

        GameScenePlayerState gameScenePlayerState = playerState as GameScenePlayerState;
        gameScenePlayerState.SetItemInfo(new ("000001", 0));
    }

    public override void StartControlCharacter(PlayerCharacterBase controlCharacter)
    {
        base.StartControlCharacter(controlCharacter);

        _PlayerInputReceivable = controlCharacter as IDefaultPlayerInputReceivable;

        // �κ��丮 �ʱ�ȭ
        inventory.Initialize(this);

        SetInputMode(Constants.INPUTMODE_GAME, false);

        // Ŀ���� ȭ�� �߾ӿ� ��� ��ġ�ǵ��� �մϴ�.
        //Cursor.lockState = CursorLockMode.Locked;

        // Ŀ���� ǥ������ �ʵ��� �մϴ�.
        //Cursor.visible = false;
    }


    private void Start()
    {
        // �� ���Կ� ������ ���
        inventory.SetQuickSlotItem("000001");
    }

    /// <summary>
    /// �Է� ��带 �����մϴ�.
    /// </summary>
    /// <param name="InputModeNmae"></param>
    public void SetInputMode(string InputModeName, bool bShowCursor = false)
    {
        playerInput.SwitchCurrentActionMap(InputModeName);    


        if (bShowCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

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

    private void OnUseItem1()
    {
        _PlayerInputReceivable?.OnUseItem1();
        inventory.UseItemFromQuickSlot(0);
    }

    private void OnGuardInput(InputValue value)
    {
        _PlayerInputReceivable?.OnGuardInput(value.isPressed);
    }

    private void OnInteractInput()
    {
        _PlayerInputReceivable?.OnInteractInput();
    }

    private void OnCloseUIInput()
    {
        _PlayerInputReceivable?.OnCloseUIInput();
    }
}
