using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 게임 씬에서 사용될 플레이어 컨트롤러 객체를 나타냅니다.
/// </summary>
public sealed class GameScenePlayerController : PlayerControllerBase
{
    private PlayerInventory _PlayerInventory;

    /// <summary>
    /// 사용자 입력을 처리할 객체입니다.
    /// </summary>
    private IDefaultPlayerInputReceivable _PlayerInputReceivable;

    /// <summary>
    /// GameUIPanel 컴포넌트를 나타냅니다.
    /// </summary>
    public GameUIPanel gameUI { get; private set; }

    /// <summary>
    /// 인벤토리 컴포넌트를 나타냅니다.
    /// </summary>
    public PlayerInventory inventory => _PlayerInventory ??
        (_PlayerInventory = GetComponent<PlayerInventory>());

    public override void InitializePlayerState()
    {
        playerState = new GameScenePlayerState(100.0f, 300.0f);

        GameScenePlayerState gameScenePlayerState = playerState as GameScenePlayerState;
        gameScenePlayerState.SetItemInfo(new ("000001", 5));
    }

    public override void StartControlCharacter(PlayerCharacterBase controlCharacter)
    {
        base.StartControlCharacter(controlCharacter);

        _PlayerInputReceivable = controlCharacter as IDefaultPlayerInputReceivable;

        // 인벤토리 초기화
        inventory.Initialize(this);

        // GameUIPanel 를 찾습니다.
        gameUI = FindObjectOfType<GameUIPanel>();

        // GameUIPanel 초기화
        gameUI.InitializeUI(this);



        // 커서를 화면 중앙에 계속 배치되도록 합니다.
        Cursor.lockState = CursorLockMode.Locked;

        // 커서를 표시하지 않도록 합니다.
        Cursor.visible = false;
    }

    private void Start()
    {
        // 퀵 슬롯에 아이템 등록
        inventory.SetQuickSlotItem("000001");
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
}
