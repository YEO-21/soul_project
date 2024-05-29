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
    }


    private void OnMovementInput(InputValue value)
    {
        Vector2 inputAxis = value.Get<Vector2>();
    
       _PlayerInputReceivable?.OnMovementInput(inputAxis);
    }

}
