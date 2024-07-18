using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 맵에서 플레이어 상태를 나타내기 위한 클래스입니다.
/// </summary>
public sealed class GameScenePlayerState : PlayerStateBase
{
    public float maxHp { get; private set; }
    public float hp { get; private set; }

    /// <summary>
    /// Hp 변경됨 이벤트
    /// </summary>
    public event System.Action<float/*maxHp*/, float/*hp*/> onHpChanged;

    public GameScenePlayerState(float initialiHp)
    {
        hp = maxHp = initialiHp;
    }

    /// <summary>
    /// Hp 수치를 설정합니다.
    /// </summary>
    /// <param name="newHp"></param>
    public void SetHp(float newHp)
    {
        hp = newHp;
        if(hp > maxHp) hp = maxHp;

        // 체력 변경됨 이벤트 발생
        onHpChanged?.Invoke(maxHp, hp);
    }

}
