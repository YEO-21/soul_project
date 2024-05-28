using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 씬에서 사용될 플레이어 컨트롤러 객체를 나타냅니다.
/// </summary>
public sealed class GameScenePlayerController : PlayerControllerBase
{
    public PlayerCharacter m_PlayerCharacter;

    private void Awake()
    {
        StartControlCharacter(m_PlayerCharacter);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            (controlledCharacter as PlayerCharacter).OnSpaceeInput();
        }
    }

}
