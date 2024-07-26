using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(Collider))]
public abstract class NpcBase : MonoBehaviour,
    IPlayerInteractable
{
    /// <summary>
    /// 상호작용 시 표시될 이름
    /// </summary>
    public string interactableName { get; private set; }

    /// <summary>
    /// 상호작용 시작 시 호출될 메서드
    /// </summary>
    public virtual void OnInteractStarted()
    {
        Debug.Log("상호작용 시작됨");

    }

    /// <summary>
    /// 상호작용이 끝난 경우 호출될 메서드
    /// </summary>
    public virtual void OnInteractFinished()
    {
        Debug.Log("상호작용 끝남");
    }

}
