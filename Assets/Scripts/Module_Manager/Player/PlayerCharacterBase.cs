using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 플레이어 캐릭터를 나타내기 위한 컴포넌트입니다.
/// </summary>
public class PlayerCharacterBase : MonoBehaviour
{
    /// <summary>
    /// 이 캐릭터를 조종중인 컨트롤러 객체를 나타냅니다.
    /// </summary>
    public PlayerControllerBase playerController { get; private set; }

    /// <summary>
    /// 이 캐릭터의 조종이 시작되었을 경우 호출됩니다.
    /// </summary>
    /// <param name="playerController"></param>
    public virtual void OnControlStarted(PlayerControllerBase playerController)
        => this.playerController = playerController;

    /// <summary>
    /// 이 캐릭터의 조종이 끝났을 경우 호출됩니다.
    /// </summary>
    public virtual void OnControlFinished() => playerController = null;



}
