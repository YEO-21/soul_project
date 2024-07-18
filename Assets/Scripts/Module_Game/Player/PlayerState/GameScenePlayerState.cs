using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �ʿ��� �÷��̾� ���¸� ��Ÿ���� ���� Ŭ�����Դϴ�.
/// </summary>
public sealed class GameScenePlayerState : PlayerStateBase
{
    public float maxHp { get; private set; }
    public float hp { get; private set; }

    /// <summary>
    /// Hp ����� �̺�Ʈ
    /// </summary>
    public event System.Action<float/*maxHp*/, float/*hp*/> onHpChanged;

    public GameScenePlayerState(float initialiHp)
    {
        hp = maxHp = initialiHp;
    }

    /// <summary>
    /// Hp ��ġ�� �����մϴ�.
    /// </summary>
    /// <param name="newHp"></param>
    public void SetHp(float newHp)
    {
        hp = newHp;
        if(hp > maxHp) hp = maxHp;

        // ü�� ����� �̺�Ʈ �߻�
        onHpChanged?.Invoke(maxHp, hp);
    }

}
