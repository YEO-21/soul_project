using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ������ ���� �÷��̾� ��Ʈ�ѷ� ��ü�� ��Ÿ���ϴ�.
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
