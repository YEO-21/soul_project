using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ���� ������ ���� �÷��̾� ��Ʈ�ѷ� ��ü�� ��Ÿ���ϴ�.
/// </summary>
public sealed class GameScenePlayerController : PlayerControllerBase
{
    private PlayerInventory _PlayerInventory;

    /// <summary>
    /// ����� �Է��� ó���� ��ü�Դϴ�.
    /// </summary>
    private IDefaultPlayerInputReceivable _PlayerInputReceivable;

    /// <summary>
    /// GameUIPanel ������Ʈ�� ��Ÿ���ϴ�.
    /// </summary>
    public GameUIPanel gameUI { get; private set; }

    /// <summary>
    /// �κ��丮 ������Ʈ�� ��Ÿ���ϴ�.
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

        // �κ��丮 �ʱ�ȭ
        inventory.Initialize(this);

        // GameUIPanel �� ã���ϴ�.
        gameUI = FindObjectOfType<GameUIPanel>();

        // GameUIPanel �ʱ�ȭ
        gameUI.InitializeUI(this);



        // Ŀ���� ȭ�� �߾ӿ� ��� ��ġ�ǵ��� �մϴ�.
        Cursor.lockState = CursorLockMode.Locked;

        // Ŀ���� ǥ������ �ʵ��� �մϴ�.
        Cursor.visible = false;
    }

    private void Start()
    {
        // �� ���Կ� ������ ���
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
