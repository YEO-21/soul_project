using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 게임 씬에서 사용될 플레이어 컨트롤러 객체를 나타냅니다.
/// </summary>
public sealed class GameScenePlayerController : PlayerControllerBase
{
    public PlayerCharacter m_PlayerCharacter;

    /// <summary>
    /// 사용자 입력을 처리할 객체입니다.
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
