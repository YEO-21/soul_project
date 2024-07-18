using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �� �ϳ��� �����ϱ� ���� ������Ʈ
/// </summary>
public class SceneInstance : MonoBehaviour
{
    [Header("# �÷��̾� ��Ʈ�ѷ� ������")]
    public PlayerControllerBase m_PlayerControllerPrefab;

    /// <summary>
    /// ������ �÷��̾� ��Ʈ�ѷ� ��ü�� ��Ÿ���ϴ�.
    /// </summary>
    public PlayerControllerBase playerController { get; private set; }

    protected virtual void Awake()
    {
        if (m_PlayerControllerPrefab)
        {
            // �÷��̾� ��Ʈ�ѷ� ����
            playerController = Instantiate(m_PlayerControllerPrefab);

            // �÷��̾� ������Ʈ �ʱ�ȭ
            playerController.InitializePlayerState();

            // ��Ʈ�ѷ��� ������ ĳ���͸� ã���ϴ�.
            PlayerCharacterBase playerCharacter = FindObjectOfType<PlayerCharacterBase>();

            // ĳ���͸� ã�� ���
            if (playerCharacter)
            {
                // �ش� ĳ���͸� �����ϵ��� �մϴ�.
                playerController.StartControlCharacter(playerCharacter);
            }
        }
    }
}
