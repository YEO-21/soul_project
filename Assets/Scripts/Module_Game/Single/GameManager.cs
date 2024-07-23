using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameManager : GameManagerBase
{
    private static GameManager _Instance;

    [Header("# �÷��̾� ���� ����")]
    public PlayerAttackInfoScriptableObject m_PlayerAttackInfoScriptableObject;

    [Header("# �� ����")]
    public EnemyInfoScriptableObject m_EnemyInfoScriptableObject;

    public static GameManager instance => _Instance ?? (_Instance = Get<GameManager>());

}
