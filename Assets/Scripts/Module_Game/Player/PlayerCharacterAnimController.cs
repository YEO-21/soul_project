using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// 플레이어 캐릭터 애니메이터를 제어하기 위한 컴포넌트입니다.
/// </summary>
public sealed class PlayerCharacterAnimController : AnimController
{
    private const string PARAM_MOVESPEED = "_MoveSpeed";
    private const string PARAM_ONAIR = "_IsOnAir";
    private const string PARAM_DODGEROLLREQUESTED = "_DodgeRollRequested";


    public float m_MoveSpeedBlend = 10.0f;

    /// <summary>
    /// 애니메이터 파라미터에 설정될 이동속력
    /// </summary>
    private float _MoveSpeed;

    private float _TargetMoveSpeed;

    #region 이벤트
    /// <summary>
    /// 구르기 애니메이션 시작 이벤트
    /// </summary>
    public event System.Action onDodgeRollAnimStarted;

    /// <summary>
    /// 구르기 애니메이션 끝 이벤트
    /// </summary>
    public event System.Action onDodgeRollAnimFinished;
    #endregion


    private bool _IsOnAir;

    public void Initailize(PlayerCharacterMovement movement)
    {
        // 이동 속력 변경 이벤트 바인딩
        movement.onHorizontalSpeedChanged += CALLBACK_OnHorizontalMoveSpeedChanged;

        // 점프 시작 콜백 등록
        movement.onJumpStarted += CALLBACK_OnJumpStarted;

        // 점프 끝 콜백 등록
        movement.onGrounded += CALLBACK_OnGrounded;

        // 피하기 시작 콜백 등록
        movement.onDodgeRollStarted += CALLBACK_OnDodgeRollStarted;
    }

    private void Movement_onJumpStarted()
    {
        throw new System.NotImplementedException();
    }

    private void Update()
    {
        _MoveSpeed = Mathf.Lerp(_MoveSpeed, _TargetMoveSpeed, m_MoveSpeedBlend * Time.deltaTime);
        SetParam(PARAM_MOVESPEED, _MoveSpeed);
        SetParam(PARAM_ONAIR, _IsOnAir);
    }

    /// <summary>
    /// 수평 이동 속력이 변경되었을 경우 호출되는 메서드입니다.
    /// </summary>
    /// <param name="speed"></param>
    private void CALLBACK_OnHorizontalMoveSpeedChanged(float speed) => _TargetMoveSpeed = speed;

    /// <summary>
    /// 점프 시작 시 호출되는 메서드입니다.
    /// </summary>
    private void CALLBACK_OnJumpStarted() => _IsOnAir = true;

    /// <summary>
    /// 땅에 닿았을 경우 호출되는 메서드입니다.
    /// </summary>
    private void CALLBACK_OnGrounded() => _IsOnAir = false;

    /// <summary>
    /// 피하기 시작 시 호출되는 메서드입니다.
    /// </summary>
    private void CALLBACK_OnDodgeRollStarted() => SetParam(PARAM_DODGEROLLREQUESTED);

    /// <summary>
    /// 피하기 애니메이션 시작 이벤트
    /// </summary>
    private void AnimEvent_OnDodgeRollStarted() => onDodgeRollAnimStarted?.Invoke();

    /// <summary>
    /// 피하기 애니메이션 끝 이벤트
    /// </summary>
    private void AnimEvent_OnDodgeRollFinished() => onDodgeRollAnimFinished?.Invoke();

}
