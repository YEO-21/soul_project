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

    /// <summary>
    /// ���� �������� ��Ÿ���� ���� ������Ƽ
    /// </summary>
    public bool isAttacking { get; private set; }

    /// <summary>
    /// ���� ���� �̺�Ʈ
    /// </summary>
    public event System.Action<int /*attackCode*/> onAttackStarted;

    /// <summary>
    /// ������ ���� Ȯ���� ���� �븮��
    /// </summary>
    private System.Func<bool> _IsDodging;

    private void Awake()
    {
        // �÷��̾� ���� ���� ������ ����ϴ�.
        _PlayerAttackInfoScriptableObject = 
            GameManager.instance.m_PlayerAttackInfoScriptableObject;
    }

    public void Initialize(PlayerCharacterAnimController animController,
        PlayerCharacterMovement movement)
    {
        animController.onAttackAnimationFinished += CALLBACK_OnAttackAnimationFinished;

        _IsDodging = () => movement.isDodging;
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

        // ���� ���·� �����մϴ�.
        isAttacking = true;

        // ��û�� ������ ���� �������� �����մϴ�.
        _CurrentAttack = _NextAttack;

        // ��û�� ���� ó�� �Ϸ�
        _NextAttack = null;

        // ���� ���۵� �̺�Ʈ �߻�
        onAttackStarted?.Invoke(_CurrentAttack.attackInfo.intAttackCode);
    }

    /// <summary>
    /// ���� ������ ���� �ڵ�� ��ȯ�մϴ�.
    /// ���� ���� ������ ������ ��û�� ���, ������ �Ű� ������ ���� ���� ������ ���� �ڵ�� �����մϴ�.
    /// ���� �Ұ����� ������ ��û�� ���, ������ �Ű� ������ ���� �������� �ʽ��ϴ�.
    /// </summary>
    /// <param name="ref_AttackCode">��ȯ�� ���� �ڵ带 �����մϴ�.</param>
    private void ConvertLinkableAttackCode(ref string ref_AttackCode)
    {
        // ���� �������� ������ ���ٸ� �������� �ʽ��ϴ�.
        if (_CurrentAttack == null) return;

        // ���� ������ ���� �ڵ带 ����ϴ�.
        string linkableAttackCode = _CurrentAttack.ConvertToLinkableAttackCode(ref_AttackCode);

        // ���� ������ ���� �ڵ尡 �����Ѵٸ�
        if(!string.IsNullOrEmpty(linkableAttackCode))
        {
            // ���� ������ ���� ���� �ڵ�� �����մϴ�.
            ref_AttackCode = linkableAttackCode;
        }
    }

    /// <summary>
    /// ������ ��û�մϴ�.
    /// </summary>
    /// <param name="attackCode"/>��û��ų ���� �ڵ带 �����մϴ�.</param>>
    public void RequestAttack(string attackCode)
   {
        // ������ ��û�Ǿ��� �� ������ ���¶�� ��û ���
        if (_IsDodging.Invoke()) return;

        // ���� ������ �������� ���ϴ� ����� �Լ� ȣ�� ����
        if (_NextAttack != null) return;

        // ���� �������� ������ �����ϴ� ���
        if(_CurrentAttack != null)
        {
            // ������ �߰��� �� ���� ��� �Լ� ȣ�� ����
            if (!_CurrentAttack.IsAttackAddable(attackCode)) return;
        }

        // ���� ������ ���� �ڵ�� ��ȯ
        ConvertLinkableAttackCode(ref attackCode);

        // ���� ������ ����ϴ�.
        PlayerAttackInfo attackInfo;
        if (!_PlayerAttackInfoScriptableObject.TryGetPlayerAttackInfo(
            attackCode, out attackInfo)) return;

        // ���� ������ �����մϴ�
        _NextAttack = PlayerAttackBase.GetPlayerAttack(attackInfo);

        Debug.Log($"����� ���� �̸� : {_NextAttack.attackInfo.m_AttackName}");
   }


    /// <summary>
    /// ���� �ִϸ��̼��� ������ ��� ȣ��Ǵ� �޼���
    /// </summary>
    private void CALLBACK_OnAttackAnimationFinished()
    {
        _CurrentAttack = null;

        if (_NextAttack == null) 
        {
            isAttacking = false;        
        }
    }
}
