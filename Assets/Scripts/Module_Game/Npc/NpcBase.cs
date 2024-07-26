using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(Collider))]
public abstract class NpcBase : MonoBehaviour,
    IPlayerInteractable
{
    /// <summary>
    /// ��ȣ�ۿ� �� ǥ�õ� �̸�
    /// </summary>
    public string interactableName { get; private set; }

    /// <summary>
    /// ��ȣ�ۿ� ���� �� ȣ��� �޼���
    /// </summary>
    public virtual void OnInteractStarted()
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
