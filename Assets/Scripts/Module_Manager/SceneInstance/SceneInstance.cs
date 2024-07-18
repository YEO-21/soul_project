using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 씬 하나를 관리하기 위한 컴포넌트
/// </summary>
public class SceneInstance : MonoBehaviour
{
    [Header("# 플레이어 컨트롤러 프리팹")]
    public PlayerControllerBase m_PlayerControllerPrefab;

    /// <summary>
    /// 생성된 플레이어 컨트롤러 객체를 나타냅니다.
    /// </summary>
    public PlayerControllerBase playerController { get; private set; }

    protected virtual void Awake()
    {
        if (m_PlayerControllerPrefab)
        {
            // 플레이어 컨트롤러 생성
            playerController = Instantiate(m_PlayerControllerPrefab);

            // 플레이어 스테이트 초기화
            playerController.InitializePlayerState();

            // 컨트롤러가 조종할 캐릭터를 찾습니다.
            PlayerCharacterBase playerCharacter = FindObjectOfType<PlayerCharacterBase>();

            // 캐릭터를 찾은 경우
            if (playerCharacter)
            {
                // 해당 캐릭터를 조종하도록 합니다.
                playerController.StartControlCharacter(playerCharacter);
            }
        }
    }
}
