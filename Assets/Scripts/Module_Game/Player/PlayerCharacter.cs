using GameModule;
using UnityEngine;

/// <summary>
/// �÷��̾� ĳ���͸� ��Ÿ���ϴ�.
/// </summary>
public sealed class PlayerCharacter : PlayerCharacterBase,
    IDefaultPlayerInputReceivable,
    IDamageable
{
    /// <summary>
    /// ���������� ���¹̳ʸ� ����� �ð�
    /// </summary>
    private float _LastStaminaUsedTime;

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

        // ���׹̳� ��� �ݹ� ���
        movement.onStaminaUsed += UseStamina;
        attack.onStaminaUsed += UseStamina;

        _IsDodging = () => movement.isDodging;
    }

    private void Update() => RechargeStamina();


    void IDefaultPlayerInputReceivable.OnMovementInput(Vector2 inputAxis) => movement.OnMovementInput(inputAxis);

    //void IDefaultPlayerInputReceivable.OnJumpInput() => movement.OnJumpInput();
    void IDefaultPlayerInputReceivable.OnJumpInput() => movement.OnDodgeInput();

    void IDefaultPlayerInputReceivable.OnSprintInput(bool isPressed) => movement.OnSprintInput(isPressed);

    void IDefaultPlayerInputReceivable.OnNormalAttackInput() => attack.RequestAttack(Constants.PLAYER_ATTACKCODE_NORMAL);
    
    void IDefaultPlayerInputReceivable.OnUseItem1() { }
    void IDefaultPlayerInputReceivable.OnGuardInput(bool isPressed) => attack.OnGuardInput(isPressed);

    public void OnHit(DamageBase damageInstance)
    {
        // ���������� ��� ȣ�� ����.
        if (_IsDodging.Invoke()) return;

        // �и� ���� ���� Ȯ��
        if (attack.IsParried(damageInstance.from.position)) return;
        // �и� ���� �� ���� ���� ��Ȱ��ȭ
        else attack.OnGuardInput(false);

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

    public bool UseStamina(float useStamina)
    {
        // ���¹̳� ��� �ð��� ����մϴ�.
        _LastStaminaUsedTime = Time.time;

        // ���� Stamina
        float stamina = gameScenePlayerState.stamina;

        if (stamina < useStamina)
            return false;

        stamina -= useStamina;

        gameScenePlayerState.SetStamina(stamina);
        return true;
    }

    private void RechargeStamina()
    {
        if (_LastStaminaUsedTime + 0.1f > Time.time) return;

        float rechargeStamina = 100.0f * Time.deltaTime;

        float stamina = gameScenePlayerState.stamina;
        stamina += rechargeStamina;

        gameScenePlayerState.SetStamina(stamina);
    }

    private void CALLBACK_OnHitAnimationFinished()
    {
        isHit = false;
    }




}
