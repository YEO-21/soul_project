using Cinemachine;
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

    private CinemachineVirtualCamera _VirtualCamera;

    [Header("# NPC Code")]
    public string m_NpcCode;

    [Header("# 상호작용 트랜스폼")]
    public Transform m_InteractTransform;


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

        // Find Cinemachine Virtual Camera Component
        _VirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();

        if (_VirtualCamera)
            _VirtualCamera.Priority = GameModule.Constants.INTERACT_CAMERA_DISABLE_PRIORITY;

        // Npc 정보를 얻습니다.
        npcInfo = _NpcInfoScriptableObject.GetNpcInfoFromCode(m_NpcCode);
;    }

    /// <summary>
    /// 상호작용 시작 시 호출될 메서드
    /// </summary>
    public virtual void OnInteractStarted(NpcInteractUIPanel useInteractUI)
    {

        if (_VirtualCamera)
        _VirtualCamera.Priority = GameModule.Constants.INTERACT_CAMERA_ENABLE_PRIORITY;
    }

    /// <summary>
    /// 상호작용이 끝난 경우 호출될 메서드
    /// </summary>
    public virtual void OnInteractFinished()
    {
        if (_VirtualCamera)
            _VirtualCamera.Priority = GameModule.Constants.INTERACT_CAMERA_DISABLE_PRIORITY;
    }

    /// <summary>
    /// 상호작용 위치를 반환합니다.
    /// </summary>
    /// <returns></returns>
    public virtual Transform GetInteractionTransform()
        => m_InteractTransform;
}
