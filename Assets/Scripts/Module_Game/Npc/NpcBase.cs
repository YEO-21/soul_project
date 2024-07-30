using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Npc Ÿ���� ��Ÿ���ϴ�.
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
    /// ��ȣ�ۿ� �� ǥ�õ� �̸�
    /// </summary>
    public string interactableName { get; private set; }

    public NpcInfo npcInfo { get; private set; }

    protected virtual void Awake()
    {
        if (_NpcInfoScriptableObject == null)
            _NpcInfoScriptableObject = Resources.Load<NpcInfoScriptableObject>(
                "ScriptableObject/NpcInfo");

        // Npc ������ ����ϴ�.
        npcInfo = _NpcInfoScriptableObject.GetNpcInfoFromCode(m_NpcCode);
;    }

    /// <summary>
    /// ��ȣ�ۿ� ���� �� ȣ��� �޼���
    /// </summary>
    public virtual void OnInteractStarted(NpcInteractUIPanel useInteractUI)
    {
        Debug.Log("��ȣ�ۿ� ���۵�");
    }

    /// <summary>
    /// ��ȣ�ۿ��� ���� ��� ȣ��� �޼���
    /// </summary>
    public virtual void OnInteractFinished()
    {
        Debug.Log("��ȣ�ۿ� ����");
    }
}
