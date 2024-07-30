using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Npc 타입을 나타냅니다.
/// </summary>
public enum NpcType
{
    Dialog
}




[RequireComponent(typeof(Collider))]
public abstract class NpcBase : MonoBehaviour,
    IPlayerInteractable
{
    private static NpcInfoScriptableObject _NpcInfoScriptableObject;

    [Header("# NPC Code")]
    public string m_NpcCode;


    /// <summary>
    /// 상호작용 시 표시될 이름
    /// </summary>
    public string interactableName { get; private set; }

    public NpcInfo npcInfo { get; private set; }

    protected virtual void Awake()
    {
        if (_NpcInfoScriptableObject == null)
            _NpcInfoScriptableObject = Resources.Load<NpcInfoScriptableObject>(
                "ScriptableObject/NpcInfo");

        // Npc 정보를 얻습니다.
        npcInfo = _NpcInfoScriptableObject.GetNpcInfoFromCode(m_NpcCode);
;    }

    /// <summary>
    /// 상호작용 시작 시 호출될 메서드
    /// </summary>
    public virtual void OnInteractStarted(NpcInteractUIPanel useInteractUI)
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
