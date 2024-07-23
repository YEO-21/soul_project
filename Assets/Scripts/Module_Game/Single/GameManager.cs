using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameManager : GameManagerBase
{
    private static GameManager _Instance;

    [Header("# 플레이어 공격 정보")]
    public PlayerAttackInfoScriptableObject m_PlayerAttackInfoScriptableObject;

    [Header("# 적 정보")]
    public EnemyInfoScriptableObject m_EnemyInfoScriptableObject;

    public static GameManager instance => _Instance ?? (_Instance = Get<GameManager>());

}
