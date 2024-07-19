using GameModule;
using UnityEngine;

/// <summary>
/// �÷��̾� ĳ���͸� ��Ÿ���ϴ�.
/// </summary>
public sealed class PlayerCharacter : PlayerCharacterBase,
    IDefaultPlayerInputReceivable,
    IDamageable
{
    private PlayerCharacterMovement _MovementComponent;
    private PlayerCharacterAttack _AttackComponent;
    private PlayerCharacterAnimController _AnimController;

    public PlayerCharacterMovement movement => _MovementComponent ?? (_MovementComponent = GetComponent<PlayerCharacterMovement>());
    public PlayerCharacterAttack attack => 
        _AttackComponent ?? (_AttackComponent = GetComponent<PlayerCharacterAttack>());
    public PlayerCharacterAnimController animController => _AnimController ??
        (_AnimController = GetComponentInChildren<PlayerCharacterAnimController>());

    /// <summary>
    /// GameScenePlayerState ��ü�� ��ȯ�մϴ�.
    /// </summary>
    public GameScenePlayerState gameScenePlayerState => 
        playerController.playerState as GameScenePlayerState;

    public string objectName { get; private set; } = "PlayerCharacter";
    public float currentHp => gameScenePlayerState.hp;
    public float maxHp => gameScenePlayerState.maxHp;

    /// <summary>
    /// ���ظ� �԰� �ִ� �������� ��Ÿ���ϴ�.
    /// </summary>
    public bool isHit { get; private set; }

    /// <summary>
    /// ���ظ� �Ծ��� ��� �߻��ϴ� �̺�Ʈ
    /// </summary>
    public event System.Action<DamageBase> onHit;

    /// <summary>
    /// �������� ���¸� ��Ÿ���� ���� �븮��
    /// </summary>
    private System.Func<bool> _IsDodging;

    private void Start()
    {
        movement.Initialize(this);
        attack.Initialize(this);
        animController.Initialize(this);

        // Hit �ִϸ��̼� ���� �ݹ� ���
        animController.onHitAnimationFinished += CALLBACK_OnHitAnimationFinished;

        _IsDodging = () => movement.isDodging;
    }


    void IDefaultPlayerInputReceivable.OnMovementInput(Vector2 inputAxis) => movement.OnMovementInput(inputAxis);

    //void IDefaultPlayerInputReceivable.OnJumpInput() => movement.OnJumpInput();
    void IDefaultPlayerInputReceivable.OnJumpInput() => movement.OnDodgeInput();

    void IDefaultPlayerInputReceivable.OnSprintInput(bool isPressed) => movement.OnSprintInput(isPressed);

    void IDefaultPlayerInputReceivable.OnNormalAttackInput() => attack.RequestAttack(Constants.PLAYER_ATTACKCODE_NORMAL);
    
    void IDefaultPlayerInputReceivable.OnUseItem1() { }

    public void OnHit(DamageBase damageInstance)
    {
        // ���������� ��� ȣ�� ����.
        if (_IsDodging.Invoke()) return;

        // ���ظ� �Դ� ���·� �����մϴ�.
        isHit = true;

        // ���� ���� �̺�Ʈ �߻�
        onHit?.Invoke(damageInstance);

        // ���� ü���� ����ϴ�.
        float currentHp = this.currentHp;

        // ü���� ���ҽ�ŵ�ϴ�.
        currentHp -= damageInstance.damage;

        // ���� ü���� �����մϴ�.
        gameScenePlayerState.SetHp(currentHp);

        Debug.Log("���� ü�� : " + this.currentHp);
    }

    private void CALLBACK_OnHitAnimationFinished()
    {
        isHit = false;
    }




}
