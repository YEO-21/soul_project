using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ���� ������ ���� �÷��̾� ��Ʈ�ѷ� ��ü�� ��Ÿ���ϴ�.
/// </summary>
public sealed class GameScenePlayerController : PlayerControllerBase
{
    public PlayerCharacter m_PlayerCharacter;

    /// <summary>
    /// ����� �Է��� ó���� ��ü�Դϴ�.
    /// </summary>
    private IDefaultPlayerInputReceivable _PlayerInputReceivable;

    private void Awake()
    {
        StartControlCharacter(m_PlayerCharacter);
    }

    public override void StartControlCharacter(PlayerCharacterBase controlCharacter)
    {
        base.StartControlCharacter(controlCharacter);

        _PlayerInputReceivable = controlCharacter as IDefaultPlayerInputReceivable;

        // Ŀ���� ȭ�� �߾ӿ� ��� ��ġ�ϵ��� �մϴ�.
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
        //value.isPressed



    }

}
