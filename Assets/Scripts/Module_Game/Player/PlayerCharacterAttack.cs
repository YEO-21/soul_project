using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// �÷��̾� ĳ���� ������ �����ϴ� ������Ʈ
/// </summary>
public sealed class PlayerCharacterAttack : MonoBehaviour
{
    /// <summary>
    /// �÷��̾� ���� ���� ScriptableObject ����
    /// </summary>
    private PlayerAttackInfoScriptableObject _PlayerAttackInfoScriptableObject;


    /// <summary>
    /// ���� ����
    /// </summary>
    private PlayerAttackBase _CurrentAttack;


    /// <summary>
    /// ���� ����
    /// </summary>
    private PlayerAttackBase _NextAttack;

    private void Awake()
    {
        // �÷��̾� ���� ���� ������ ����ϴ�.
        _PlayerAttackInfoScriptableObject = 
            GameManager.instance.m_PlayerAttackInfoScriptableObject;
    }

    private void Update()
    {
        AttackProcedure();
    }

    /// <summary>
    /// ��û�� ������ ó���մϴ�.
    /// </summary>
    private void AttackProcedure()
    {
        // ��û�� ���� ������ �������� �ʴ´ٸ� �Լ� ȣ�� ����
        if (_NextAttack == null) return;

        // ���� ������ �������� ��� �Լ� ȣ�� ����
        if (_CurrentAttack != null) return;

        // ��û�� ������ ���� �������� �����մϴ�.
        _CurrentAttack = _NextAttack;
        Debug.Log($"����� ���� �̸� : {_CurrentAttack.attackInfo.m_AttackName}");

        // ��û�� ���� ó�� �Ϸ�
        _NextAttack = null;

    }

    /// <summary>
    /// ������ ��û�մϴ�.
    /// </summary>
    /// <param name="attackCode"/>��û��ų ���� �ڵ带 �����մϴ�.</param>>
    public void RequestAttack(string attackCode)
   {
        // ���� ������ �������� ���ϴ� ����� �Լ� ȣ�� ����
        if (_NextAttack != null) return;

        // ���� ������ ���� �ڵ�� ��ȯ


        // ���� ������ ����ϴ�.
        PlayerAttackInfo attackInfo;
        if (!_PlayerAttackInfoScriptableObject.TryGetPlayerAttackInfo(
            attackCode, out attackInfo)) return;

        // ���� ������ �����մϴ�
        _NextAttack = PlayerAttackBase.GetPlayerAttack(attackInfo);

        Debug.Log($"����� ���� �̸� : {_NextAttack.attackInfo.m_AttackName}");
   }

}
