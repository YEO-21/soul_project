using GameModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 캐릭터를 나타냅니다.
/// </summary>
public sealed class PlayerCharacter : PlayerCharacterBase,
    IDefaultPlayerInputReceivable
{
    private PlayerCharacterMovement _MovementComponent;
    private PlayerCharacterAttack _AttackComponent;
    private PlayerCharacterAnimController _AnimController;

    public PlayerCharacterMovement movement => _MovementComponent ?? (_MovementComponent = GetComponent<PlayerCharacterMovement>());
    public PlayerCharacterAttack attack => _AttackComponent ?? (_AttackComponent = GetComponent<PlayerCharacterAttack>());
    public PlayerCharacterAnimController animController => 
        _AnimController ?? 
        (_AnimController = GetComponentInChildren<PlayerCharacterAnimController>());

    private void Start()
    {
        movement.Initialize(animController, attack);
        animController.Initailize(movement, attack);
        attack.Initialize(animController, movement);
    }


    void IDefaultPlayerInputReceivable.OnMovementInput(Vector2 inputAxis) => movement.OnMovementInput(inputAxis);

    //void IDefaultPlayerInputReceivable.OnJumpInput() => movement.OnJumpInput();
    void IDefaultPlayerInputReceivable.OnJumpInput() => movement.OnDodgeInput();

    void IDefaultPlayerInputReceivable.OnSprintInput(bool isPressed) => movement.OnSprintInput(isPressed);

    void IDefaultPlayerInputReceivable.OnNormalAttackInput() => attack.RequestAttack(Constants.PLAYER_ATTACK_NORMAL);
}
