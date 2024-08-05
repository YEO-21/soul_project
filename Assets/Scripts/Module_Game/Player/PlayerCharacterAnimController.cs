using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 캐릭터 애니메이터를 제어하기 위한 컴포넌트입니다.
/// </summary>
public sealed class PlayerCharacterAnimController : AnimController
{
    private const string PARAM_MOVESPEED = "_MoveSpeed";
    private const string PARAM_ISONAIR = "_IsOnAir";
    private const string PARAM_DODGEROLLREQUESTED = "_DodgeRollRequested";
    private const string PARAM_ATTACKCODE = "_AttackCode";
    private const string PARAM_ISATTACKING = "_IsAttacking";
    private const string PARAM_ISDAMAGED = "_IsDamaged";
    private const string PARAM_ISGUARD = "_IsGuard";

    public float m_MoveSpeedBlend = 10.0f;

    /// <summary>
    /// 애니메이터 파라미터에 설정될 현재 이동속력
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

    /// <summary>
    /// 피해입음 애니메이션 재생 끝 이벤트
    /// </summary>
    public event System.Action onHitAnimationFinished;

    #endregion



    public void Initialize(PlayerCharacter playerCharacter)
    {
        // 이동 속력 변경 콜백 등록
        playerCharacter.movement.onHorizontalSpeedChanged += CALLBACK_OnHorizontalMoveSpeedChanged;

        // 점프 시작 콜백 등록
        playerCharacter.movement.onJumpStarted += CALLBACK_OnJumpStarted;

        // 점프 끝 콜백 등록
        playerCharacter.movement.onGrounded += CALLBACK_OnGrounded;

        // 피하기 시작 콜백 등록
        playerCharacter.movement.onDodgeRollStarted += CALLBACK_OnDodgeRollStarted;

        // 공격 시작 콜백 등록
        playerCharacter.attack.onAttackStarted += CALLBACK_OnAttackStarted;

        // 공격 취소됨 콜백 등록
        playerCharacter.attack.onAttackCanceled += CALLBACK_OnAttackCanceled;

        // 방어 상태 변경됨 콜백 등록
        playerCharacter.attack.onGuardStateUpdated += CALLBACK_OnGuardStateUpdated;

        // 피해 입음 콜백 등록
        playerCharacter.onHit += CALLBACK_OnHit;
    }

    /// <summary>
    /// 피해 입음 애니메이션 재생이 끝났을 경우 호출됩니다.
    /// </summary>
    public void OnHitAnimationFinished()
        => onHitAnimationFinished?.Invoke();

    private void Update()
    {
        _MoveSpeed = Mathf.Lerp(_MoveSpeed, _TargetMoveSpeed, m_MoveSpeedBlend * Time.deltaTime);
        SetParam(PARAM_MOVESPEED, _MoveSpeed);
    }

    /// <summary>
    /// 수평 이동 속력이 변경되었을 경우 호출되는 메서드입니다.
    /// </summary>
    /// <param name="speed"></param>
    private void CALLBACK_OnHorizontalMoveSpeedChanged(float speed) 
        => _TargetMoveSpeed = speed;

    /// <summary>
    /// 점프 시작시 호출되는 메서드입니다.
    /// </summary>
    private void CALLBACK_OnJumpStarted() => SetParam(PARAM_ISONAIR, true);

    /// <summary>
    /// 땅에 닿았을 경우 호출되는 메서드입니다.
    /// </summary>
    private void CALLBACK_OnGrounded() => SetParam(PARAM_ISONAIR, false);

    /// <summary>
    /// 피하기 시작시 호출되는 메서드입니다.
    /// </summary>
    private void CALLBACK_OnDodgeRollStarted() => SetParam(PARAM_DODGEROLLREQUESTED);

    /// <summary>
    /// 공격 시작 시 호출되는 메서드입니다.
    /// </summary>
    /// <param name="attackCode"></param>
    private void CALLBACK_OnAttackStarted(int attackCode) => SetParam(PARAM_ATTACKCODE, attackCode);

    /// <summary>
    /// 피해를 입을 경우 호출되는 메서드입니다.
    /// </summary>
    /// <param name="damageInstance"></param>
    private void CALLBACK_OnHit(DamageBase damageInstance)
        => SetParam(PARAM_ISDAMAGED);

    /// <summary>
    /// 공격이 취소되었을 경우 호출되는 메서드입니다.
    /// </summary>
    private void CALLBACK_OnAttackCanceled()
    {
        SetParam(PARAM_ATTACKCODE, 0);
        SetParam(PARAM_ISATTACKING, false);
    }

    private void CALLBACK_OnGuardStateUpdated(bool isGuardState)
    {
        SetParam(PARAM_ISGUARD, isGuardState);
    }

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
    private void AnimEvent_OnAtackAreaCheckStarted()
        => onAttackAreaCheckStarted?.Invoke();

    /// <summary>
    /// 공격 영역 확인 끝남
    /// </summary>
    private void AnimEvent_OnAttackAreaCheckFinished()
        => onAttackAreaCheckFinished?.Invoke();

    private void AnimEvent_PlayEffectSound01() =>
        SoundManager.instance.PlayEffectSound(SoundManager.EFFECT_SWING01, transform.position);

    private void AnimEvent_PlayEffectSound02() =>
        SoundManager.instance.PlayEffectSound(SoundManager.EFFECT_SWING02, transform.position);

    private void AnimEvent_PlayEffectSound03() =>
        SoundManager.instance.PlayEffectSound(SoundManager.EFFECT_SWING03, transform.position);
}
