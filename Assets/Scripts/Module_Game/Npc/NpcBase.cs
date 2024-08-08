using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Npc Ÿ���� ��Ÿ���ϴ�.
/// </summary>
public enum NpcType
{ 
    Dialog,
}


[RequireComponent(typeof(Collider))]
public abstract class NpcBase : MonoBehaviour,
    IPlayerInteractable
{
    [Header("# NameHUD YOffset")]
    public float m_HUDYOffset = 2.0f;

    private static NpcInfoScriptableObject _NpcInfoScriptableObject;

    private CinemachineVirtualCamera _VirtualCamera;

    private NpcNamePanel _NameHUD;


    [Header("# NPC Code")]
    public string m_NpcCode;

    [Header("# ��ȣ�ۿ� Ʈ������")]
    public Transform m_InteractTransform;

    /// <summary>
    /// ��ȣ�ۿ� �� ǥ�õ� �̸�
    /// </summary>
    public string interactableName { get; private set; }

    public NpcInfo npcInfo { get; private set; }

    

    protected virtual void Awake()
    {
        if (_NpcInfoScriptableObject == null)
        {
            _NpcInfoScriptableObject = Resources.Load<NpcInfoScriptableObject>(
                "ScriptableObject/NpcInfo");
        }

        // Find Cinemachine Virtual Camera Component
        _VirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        if (_VirtualCamera)
            _VirtualCamera.Priority = GameModule.Constants.INTERACT_CAMEAR_DISABLE_PRIORITY;

        // Npc ������ ����ϴ�.
        npcInfo = _NpcInfoScriptableObject.GetNpcInfoFromCode(m_NpcCode);
    }

    protected virtual void Start()
    {
        GameSceneUIInstance uiInstance = 
            SceneManagerBase.instance.GetSceneInstance<SceneInstance>().
            playerController.uiInstance as GameSceneUIInstance;

        // NPC HUD ����
        _NameHUD = uiInstance.hudUI.CreateNpcNameHUD(
            npcInfo.m_Name,
            transform, 
            m_HUDYOffset);
    }

    protected virtual void OnDestroy()
    {
       // Destroy(_NameHUD.gameObject);
    }


    /// <summary>
    /// ��ȣ�ۿ� ���� �� ȣ��� �޼���
    /// </summary>
    public virtual void OnInteractStarted(NpcInteractUIPanel useInteractUI)
    {
        if (_VirtualCamera)
            _VirtualCamera.Priority = GameModule.Constants.INTERACT_CAMEAR_ENABLE_PRIORITY;
    }

    /// <summary>
    /// ��ȣ�ۿ��� ���� ��� ȣ��� �޼���
    /// </summary>
    public virtual void OnInteractFinished()
    {
        if (_VirtualCamera)
            _VirtualCamera.Priority = GameModule.Constants.INTERACT_CAMEAR_DISABLE_PRIORITY;
    }

    /// <summary>
    /// ��ȣ�ۿ� ��ġ�� ��ȯ�մϴ�.
    /// </summary>
    /// <returns></returns>
    public virtual Transform GetInteractionTransform()
        => m_InteractTransform;
}
