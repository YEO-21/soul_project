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
    /// ��ȣ�ۿ� �� ĳ���Ͱ� ��ġ�� Ʈ�������� ��ȯ�մϴ�.
    /// </summary>
    /// <returns></returns>
    Transform GetInteractionTransform();
}
