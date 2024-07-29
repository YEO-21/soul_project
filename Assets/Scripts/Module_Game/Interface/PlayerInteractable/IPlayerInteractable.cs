using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerInteractable
{
    string interactableName { get; }

    Transform transform { get; }

    NpcInfo npcInfo { get; }

    void OnInteractStarted();
    void OnInteractFinished();
}
