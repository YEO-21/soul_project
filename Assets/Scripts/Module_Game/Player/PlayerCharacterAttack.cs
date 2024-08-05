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
    /// ��� ���¿� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public bool isGuardState { get; private set; }

    /// <summary>
    /// ���� ���� �̺�Ʈ
    /// </summary>
    public event System.Action<int /*attackCode*/> onAttackStarted;

    /// <summary>
    /// ���� ��ҵ� �̺�Ʈ
    /// </summary>
    public event System.Action onAttackCanceled;

    /// <summary>
    /// ��� ���� ����� �̺�Ʈ
    /// </summary>
    public event System.Action<bool> onGuardStateUpdated;

    /// <summary>
    /// Stamina ��� �̺�Ʈ
    /// </summary>
    public event System.Func<float, bool> onStaminaUsed;

    /// <summary>
    /// ������ ���� Ȯ���� ���� �븮��
    /// </summary>
    private System.Func<bool> _IsDodging;

    /// <summary>
    /// ���� ���� ���� Ȯ���� ���� �븮��
    /// </summary>
    private System.Func<bool> _IsHit;

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

    public void Initialize(PlayerCharacter owner)
    {
        _PlayerCharacter = owner;

        owner.animController.onAttackAnimationFinished += CALLBACK_OnAttackAnimationFinished;
        owner.animController.onAttackAreaCheckStarted += CALLBACK_OnAttackAreaCheckStarted;
        owner.animController.onAttackAreaCheckFinished += CALLBACK_OnAttackAreaCheckFinished;

        owner.onHit += CALLBACK_OnHit;

        _IsDodging = () => owner.movement.isDodging;
        _IsHit = () => owner.isHit;
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
        // ��û�� ���� ������ �������� �ʴ´ٸ� �Լ� ȣ�� ����.
        if (_NextAttack == null) return;

        // ���� ������ �������� ��� �Լ� ȣ�� ����.
        if (_CurrentAttack != null) return;

        // ���¹̳ʸ� �Ҹ��� �� ���� ��� ��û ���.
        if (!onStaminaUsed.Invoke(50.0f))
        {
            _NextAttack = null;
            return;
        }

        // ���� ���·� �����մϴ�.
        isAttacking = true;

        // ��û�� ������ ���� �������� �����մϴ�.
        _CurrentAttack = _NextAttack;



        // ��û�� ���� ó�� �Ϸ�.
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
        if (!string.IsNullOrEmpty(linkableAttackCode))
        {
            // ���� ������ ���� ���� �ڵ�� �����մϴ�.
            ref_AttackCode = linkableAttackCode;
        }
    }

    public void OnGuardInput(bool isPressed)
    {
        // ���°� ����� ��찡 �ƴ϶�� ȣ�� ����.
        if (isGuardState == isPressed) return;

        // ������ ���¶�� ȣ�� ����
        if (_IsDodging()) return;

        // ���� ������ ��� ȣ�� ����.
        if (isAttacking) return;

        isGuardState = isPressed;

        // ��� ���� ����� �̺�Ʈ �߻�
        onGuardStateUpdated?.Invoke(isGuardState);
    }

    public bool IsParried(Vector3 from)
    {
        // ��� ���°� �ƴ� ��� �Լ� ȣ�� ����
        if (!isGuardState) return false;

        // ĳ������ ���� ��ġ
        Vector3 currentPosition = transform.position;

        // ĳ���Ϳ��� �� ĳ���͸� ���ϴ� ����
        Vector3 direction = from - currentPosition;
        direction.y = 0.0f;
        direction.Normalize();

        // ĳ������ �� ����
        Vector3 forward = transform.forward;

        // ĳ������ Yaw ȸ��
        float thisYaw = Mathf.Atan2(forward.z, forward.x) * Mathf.Rad2Deg;

        // �� ĳ���ͷ� ���ϴ� ������ Yaw ȸ��
        float damagedYaw = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

        // �������� ���մϴ�.
        float deltaYaw = Mathf.Abs(Mathf.DeltaAngle(thisYaw, damagedYaw));

        // ĳ���� �� ����� 40�� �̻� ���̰� ���� �ʴ� ��� �и� ����
        bool isParried = deltaYaw < 40.0f;

        if (isParried) 
            SoundManager.instance.PlayGuardSound(transform.position);

        return isParried;
    }

    /// <summary>
    /// ������ ��û�մϴ�.
    /// </summary>
    /// <param name="attackCode"/>��û��ų ���� �ڵ带 �����մϴ�.</param>
    public void RequestAttack(string attackCode)
    {
        // ������ ��û�Ǿ��� �� ���ظ� �Դ� ���̶�� ��û ���.
        if (_IsHit.Invoke()) return;

        // ������ ��û�Ǿ��� �� ������ ���¶�� ��û ���.
        if (_IsDodging.Invoke()) return;

        // ���� ���¶�� ��û ���
        if (isGuardState) return;

        // ���� ������ �������� ���ϴ� ����� �Լ� ȣ�� ����.
        if (_NextAttack != null) return;

        // ���� �������� ������ �����ϴ� ���
        if (_CurrentAttack != null)
        {
            // ������ �߰��� �� ���� ��� �Լ� ȣ�� ����.
            if (!_CurrentAttack.IsAttackAddable(attackCode)) return;
        }

        // ���� ������ ���� �ڵ�� ��ȯ
        ConvertLinkableAttackCode(ref attackCode);

        // ���� ������ ����ϴ�.
        PlayerAttackInfo attackInfo;
        if (!_PlayerAttackInfoScriptableObject.TryGetPlayerAttackInfo(
            attackCode, out attackInfo)) return;

        // ���� ������ �����մϴ�.
        _NextAttack = PlayerAttackBase.GetPlayerAttack(_PlayerCharacter, attackInfo);
    }

    /// <summary>
    /// ������ ����մϴ�.
    /// </summary>
    private void CancelAttack()
    {
        // ���� ���
        if (isAttacking || (_NextAttack != null))
        {
            // ���� ���� �˻� ��
            m_EquippedWeapon?.StopAttackAreaCheck();

            // ���� ��ü ����
            _CurrentAttack = _NextAttack = null;

            // ���� ���� ���
            isAttacking = false;

            // ���� ��ҵ� �̺�Ʈ �߻�
            onAttackCanceled?.Invoke();
        }

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

    /// <summary>
    /// ���ظ� ���� ��� ȣ��Ǵ� �޼���
    /// </summary>
    /// <param name="damageInstance"></param>
    private void CALLBACK_OnHit(DamageBase damageInstance)
        => CancelAttack();

}
