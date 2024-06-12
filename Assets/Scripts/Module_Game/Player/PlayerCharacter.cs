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

    private PlayerCharacterAnimController _AnimController;

    public PlayerCharacterMovement movement => _MovementComponent ?? (_MovementComponent = GetComponent<PlayerCharacterMovement>());
    public PlayerCharacterAnimController animController => 
        _AnimController ?? 
        (_AnimController = GetComponentInChildren<PlayerCharacterAnimController>());

    private void Start()
    {
        movement.Initialize(animController);
        animController.Initailize(movement);

    }


    void IDefaultPlayerInputReceivable.OnMovementInput(Vector2 inputAxis) => movement.OnMovementInput(inputAxis);

    //void IDefaultPlayerInputReceivable.OnJumpInput() => movement.OnJumpInput();
    void IDefaultPlayerInputReceivable.OnJumpInput() => movement.OnDodgeInput();

    void IDefaultPlayerInputReceivable.OnSprintInput(bool isPressed) => movement.OnSprintInput(isPressed);
}
