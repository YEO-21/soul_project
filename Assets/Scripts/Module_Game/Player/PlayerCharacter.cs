using GameModule;
using UnityEngine;

/// <summary>
/// 플레이어 캐릭터를 나타냅니다.
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
    /// GameScenePlayerState 객체를 반환합니다.
    /// </summary>
    public GameScenePlayerState gameScenePlayerState => 
        playerController.playerState as GameScenePlayerState;

    public string objectName { get; private set; } = "PlayerCharacter";
    public float currentHp => gameScenePlayerState.hp;
    public float maxHp => gameScenePlayerState.maxHp;

    /// <summary>
    /// 피해를 입고 있는 상태임을 나타냅니다.
    /// </summary>
    public bool isHit { get; private set; }

    /// <summary>
    /// 피해를 입었을 경우 발생하는 이벤트
    /// </summary>
    public event System.Action<DamageBase> onHit;

    /// <summary>
    /// 구르기중 상태를 나타내기 위한 대리자
    /// </summary>
    private System.Func<bool> _IsDodging;

    private void Start()
    {
        movement.Initialize(this);
        attack.Initialize(this);
        animController.Initialize(this);

        // Hit 애니메이션 끝남 콜백 등록
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
        // 구르기중인 경우 호출 종료.
        if (_IsDodging.Invoke()) return;

        // 피해를 입는 상태로 설정합니다.
        isHit = true;

        // 피해 입음 이벤트 발생
        onHit?.Invoke(damageInstance);

        // 현재 체력을 얻습니다.
        float currentHp = this.currentHp;

        // 체력을 감소시킵니다.
        currentHp -= damageInstance.damage;

        // 계산된 체력을 설정합니다.
        gameScenePlayerState.SetHp(currentHp);

        Debug.Log("현재 체력 : " + this.currentHp);
    }

    private void CALLBACK_OnHitAnimationFinished()
    {
        isHit = false;
    }




}
