using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// �÷��̾� ĳ���� ������ �����ϴ� ������Ʈ
/// </summary>
public sealed class PlayerCharacterAttack : MonoBehaviour
{
    [Header("# �������� ����")]
    public WeaponBase m_EquippedWeapon;

    [Header("# ���� ���� ���̾�")]
    public LayerMask m_DetectLayer;



    /// <summary>
    /// �÷��̾� ���� ���� ScriptableObject ����
    /// </summary>
    private PlayerAttackInfoScriptableObject _PlayerAttackInfoScriptableObject;

    /// <summary>
    /// �÷��̾� ĳ���� ��ü�� ��Ÿ���ϴ�.
    /// </summary>
    private PlayerCharacter _PlayerCharacter;

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

    /// <summary>
    /// ���ظ� ���� �� �ִ� ��ü�� ������ ��� �߻��ϴ� �̺�Ʈ
    /// </summary>
    public event System.Action<IDamageable> onNewDamageableDetected;

    private void Awake()
    {
        // �÷��̾� ���� ���� ������ ����ϴ�.
        _PlayerAttackInfoScriptableObject = 
            GameManager.instance.m_PlayerAttackInfoScriptableObject;
    }

    private void Start()
    {
        // �������� ���� �ʱ�ȭ
        m_EquippedWeapon.InitializeWeapon(m_DetectLayer);
        m_EquippedWeapon.onDetected += CALLBACK_OnDamageableObjectDetected;
    }

    public void Initialize(
        PlayerCharacter owner,
        PlayerCharacterAnimController animController,
        PlayerCharacterMovement movement)
    {
        _PlayerCharacter = owner;

        animController.onAttackAnimationFinished += CALLBACK_OnAttackAnimationFinished;

        animController.onAttackAreaCheckStarted += CALLBACK_OnAttackAreaCheckStarted;
        animController.onAttackAreaCheckFinished += CALLBACK_OnAttackAreaCheckFinished;

        _IsDodging = () => movement.isDodging;
    }

    private void AnimController_onAttackAreaCheckStarted()
    {
        throw new System.NotImplementedException();
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
        _NextAttack = PlayerAttackBase.GetPlayerAttack(_PlayerCharacter, attackInfo);
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

    /// <summary>
    /// ���� ���� �˻� ���� 
    /// </summary>
    private void CALLBACK_OnAttackAreaCheckStarted()
    {
        m_EquippedWeapon?.StartAttackAreaCheck();
    }

    /// <summary>
    /// ���� ���� �˻� ��
    /// </summary>
    ///
    private void CALLBACK_OnAttackAreaCheckFinished()
    {
        m_EquippedWeapon?.StopAttackAreaCheck();
    }

    /// <summary>
    /// ���ظ� ���� �� �ִ� ��ü�� ������ ���
    /// </summary>
    /// <param name="to"></param>
    private void CALLBACK_OnDamageableObjectDetected(IDamageable to)
    {
        _CurrentAttack?.OnDamageableObjectDetected(to);
        onNewDamageableDetected(to);

    }

  
}
