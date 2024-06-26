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
    private const string PARAM_ATTACKCODE = "_AttackCode";
    private const string PARAM_ISATTACKING = "_IsAttacking";


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

    /// <summary>
    /// 공격 애니메이션 재생 끝 이벤트
    /// </summary>
    public event System.Action onAttackAnimationFinished;

    /// <summary>
    /// 공격 영역 검사 시작 이벤트
    /// </summary>
    public event System.Action onAttackAreaCheckStarted;

    /// <summary>
    /// 공격 영역 검사 끝 이벤트
    /// </summary>
    public event System.Action onAttackAreaCheckFinished;
    #endregion


    private bool _IsOnAir;

    public void Initailize(PlayerCharacterMovement movement, PlayerCharacterAttack attack)
    {
        // 이동 속력 변경 이벤트 바인딩
        movement.onHorizontalSpeedChanged += CALLBACK_OnHorizontalMoveSpeedChanged;

        // 점프 시작 콜백 등록
        movement.onJumpStarted += CALLBACK_OnJumpStarted;

        // 점프 끝 콜백 등록
        movement.onGrounded += CALLBACK_OnGrounded;

        // 피하기 시작 콜백 등록
        movement.onDodgeRollStarted += CALLBACK_OnDodgeRollStarted;

        // 공격 시작 콜백 등록
        attack.onAttackStarted += CALLBACK_OnAttackStarted;
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
    /// 공격 시작 시 호출되는 메서드입니다.
    /// </summary>
    /// <param name="attackCode"></param>
    private void CALLBACK_OnAttackStarted(int attackCode) => SetParam(PARAM_ATTACKCODE, attackCode);

    /// <summary>
    /// 피하기 애니메이션 시작 이벤트
    /// </summary>
    private void AnimEvent_OnDodgeRollStarted() => onDodgeRollAnimStarted?.Invoke();

    /// <summary>
    /// 피하기 애니메이션 끝 이벤트
    /// </summary>
    private void AnimEvent_OnDodgeRollFinished() => onDodgeRollAnimFinished?.Invoke();

    private void AnimEvent_OnAttackSectionFinished()
    {
        // 공격 코드 초기화
        SetParam(PARAM_ATTACKCODE, 0);

        // 공격 상태 변수 초기화
        SetParam(PARAM_ISATTACKING, false);

        // 공격 애니메이션 끝 이벤트 발생
        onAttackAnimationFinished?.Invoke();
    }

    /// <summary>
    /// 공격 영역 확인 시작됨
    /// </summary>
    private void AnimEvent_OnAttackAreaCheckStarted()
    {
        onAttackAreaCheckStarted?.Invoke();
    }

    /// <summary>
    /// 공격 영역 확인 끝남
    /// </summary>
    private void AnimEvent_OnAttackAreaCheckFinished()
    {
        onAttackAreaCheckFinished?.Invoke();
    }

}
