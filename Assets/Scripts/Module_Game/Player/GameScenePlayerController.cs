using GameModule;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 게임 씬에서 사용될 플레이어 컨트롤러 객체를 나타냅니다.
/// </summary>
public sealed class GameScenePlayerController : PlayerControllerBase
{
    private PlayerInventory _PlayerInventory;

    private PlayerInput _PlayerInput;

    /// <summary>
    /// 사용자 입력을 처리할 객체입니다.
    /// </summary>
    private IDefaultPlayerInputReceivable _PlayerInputReceivable;

    public PlayerInput playerInput => _PlayerInput ??
        (_PlayerInput = GetComponent<PlayerInput>());

    /// <summary>
    /// 인벤토리 컴포넌트를 나타냅니다.
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

        // 인벤토리 초기화
        inventory.Initialize(this);

        SetInputMode(Constants.INPUTMODE_GAME, false);

        // 커서를 화면 중앙에 계속 배치되도록 합니다.
        //Cursor.lockState = CursorLockMode.Locked;

        // 커서를 표시하지 않도록 합니다.
        //Cursor.visible = false;
    }


    private void Start()
    {
        // 퀵 슬롯에 아이템 등록
        inventory.SetQuickSlotItem("000001");
    }

    /// <summary>
    /// 입력 모드를 변경합니다.
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
