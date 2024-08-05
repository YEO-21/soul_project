using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerInteractable
{
    string interactableName { get; }

    Transform transform { get; }

    NpcInfo npcInfo { get; }

    void OnInteractStarted(NpcInteractUIPanel useInteractUI);
    void OnInteractFinished();

    /// <summary>
    /// 상호작용 시 캐릭터가 배치될 트랜스폼을 반환합니다.
    /// </summary>
    /// <returns></returns>
    Transform GetInteractionTransform();
}
