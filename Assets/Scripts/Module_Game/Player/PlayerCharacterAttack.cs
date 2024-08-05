using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 캐릭터 공격을 관리하는 컴포넌트
/// </summary>
public sealed class PlayerCharacterAttack : MonoBehaviour
{
    [Header("# 장착중인 무기")]
    public WeaponBase m_EquippedWeapon;

    [Header("# 공격 감지 레이어")]
    public LayerMask m_DetectLayer;

    /// <summary>
    /// 플레이어 공격 정보 ScriptableObject 에셋
    /// </summary>
    private PlayerAttackInfoScriptableObject _PlayerAttackInfoScriptableObject;

    /// <summary>
    /// 플레이어 캐릭터 객체를 나타냅니다.
    /// </summary>
    private PlayerCharacter _PlayerCharacter;

    /// <summary>
    /// 현재 공격
    /// </summary>
    private PlayerAttackBase _CurrentAttack;

    /// <summary>
    /// 다음 공격
    /// </summary>
    private PlayerAttackBase _NextAttack;

    /// <summary>
    /// 공격 상태임을 나타내기 위한 프로퍼티
    /// </summary>
    public bool isAttacking { get; private set; }

    /// <summary>
    /// 방어 상태에 대한 프로퍼티입니다.
    /// </summary>
    public bool isGuardState { get; private set; }

    /// <summary>
    /// 공격 시작 이벤트
    /// </summary>
    public event System.Action<int /*attackCode*/> onAttackStarted;

    /// <summary>
    /// 공격 취소됨 이벤트
    /// </summary>
    public event System.Action onAttackCanceled;

    /// <summary>
    /// 방어 상태 변경된 이벤트
    /// </summary>
    public event System.Action<bool> onGuardStateUpdated;

    /// <summary>
    /// Stamina 사용 이벤트
    /// </summary>
    public event System.Func<float, bool> onStaminaUsed;

    /// <summary>
    /// 구르기 상태 확인을 위한 대리자
    /// </summary>
    private System.Func<bool> _IsDodging;

    /// <summary>
    /// 피해 입음 상태 확인을 위한 대리자
    /// </summary>
    private System.Func<bool> _IsHit;

    /// <summary>
    /// 피해를 입을 수 있는 객체를 감지한 경우 발생하는 이벤트
    /// </summary>
    public event System.Action<IDamageable> onNewDamageableDetected;



    private void Awake()
    {
        // 플레이어 공격 정보 에셋을 얻습니다.
        _PlayerAttackInfoScriptableObject = 
            GameManager.instance.m_PlayerAttackInfoScriptableObject;
    }

    private void Start()
    {
        // 장착중인 무기 초기화
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
    /// 요청된 공격을 처리합니다.
    /// </summary>
    private void AttackProcedure()
    {
        // 요청된 다음 공격이 존재하지 않는다면 함수 호출 종료.
        if (_NextAttack == null) return;

        // 현재 공격이 실행중인 경우 함수 호출 종료.
        if (_CurrentAttack != null) return;

        // 스태미너를 소모할 수 없는 경우 요청 취소.
        if (!onStaminaUsed.Invoke(50.0f))
        {
            _NextAttack = null;
            return;
        }

        // 공격 상태로 설정합니다.
        isAttacking = true;

        // 요청된 공격을 현재 공격으로 설정합니다.
        _CurrentAttack = _NextAttack;



        // 요청된 공격 처리 완료.
        _NextAttack = null;
        
        // 공격 시작됨 이벤트 발생
        onAttackStarted?.Invoke(_CurrentAttack.attackInfo.intAttackCode);
    }

    /// <summary>
    /// 연계 가능한 공격 코드로 변환합니다.
    /// 만약 연계 가능한 공격이 요청된 경우, 전달한 매개 변수의 값을 연계 가능한 공격 코드로 설정합니다.
    /// 연계 불가능한 공격이 요청된 경우, 전달한 매개 변수의 값을 변경하지 않습니다.
    /// </summary>
    /// <param name="ref_AttackCode">변환할 공격 코드를 전달합니다.</param>
    private void ConvertLinkableAttackCode(ref string ref_AttackCode)
    {
        // 현재 실행중인 공격이 없다면 실행하지 않습니다.
        if (_CurrentAttack == null) return;

        // 연계 가능한 공격 코드를 얻습니다.
        string linkableAttackCode = _CurrentAttack.ConvertToLinkableAttackCode(ref_AttackCode);

        // 연계 가능한 공격 코드가 존재한다면
        if (!string.IsNullOrEmpty(linkableAttackCode))
        {
            // 다음 공격을 연계 공격 코드로 설정합니다.
            ref_AttackCode = linkableAttackCode;
        }
    }

    public void OnGuardInput(bool isPressed)
    {
        // 상태가 변경된 경우가 아니라면 호출 종료.
        if (isGuardState == isPressed) return;

        // 구르기 상태라면 호출 종료
        if (_IsDodging()) return;

        // 공격 상태인 경우 호출 종료.
        if (isAttacking) return;

        isGuardState = isPressed;

        // 방어 상태 변경됨 이벤트 발생
        onGuardStateUpdated?.Invoke(isGuardState);
    }

    public bool IsParried(Vector3 from)
    {
        // 방어 상태가 아닌 경우 함수 호출 종료
        if (!isGuardState) return false;

        // 캐릭터의 현재 위치
        Vector3 currentPosition = transform.position;

        // 캐릭터에서 적 캐릭터를 향하는 방향
        Vector3 direction = from - currentPosition;
        direction.y = 0.0f;
        direction.Normalize();

        // 캐릭터의 앞 방향
        Vector3 forward = transform.forward;

        // 캐릭터의 Yaw 회전
        float thisYaw = Mathf.Atan2(forward.z, forward.x) * Mathf.Rad2Deg;

        // 적 캐릭터로 향하는 방향의 Yaw 회전
        float damagedYaw = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

        // 각도차를 구합니다.
        float deltaYaw = Mathf.Abs(Mathf.DeltaAngle(thisYaw, damagedYaw));

        // 캐릭터 앞 방향과 40도 이상 차이가 나지 않는 경우 패링 성공
        bool isParried = deltaYaw < 40.0f;

        if (isParried) 
            SoundManager.instance.PlayGuardSound(transform.position);

        return isParried;
    }

    /// <summary>
    /// 공격을 요청합니다.
    /// </summary>
    /// <param name="attackCode"/>요청시킬 공격 코드를 전달합니다.</param>
    public void RequestAttack(string attackCode)
    {
        // 공격이 요청되었을 때 피해를 입는 중이라면 요청 취소.
        if (_IsHit.Invoke()) return;

        // 공격이 요청되었을 때 구르기 상태라면 요청 취소.
        if (_IsDodging.Invoke()) return;

        // 가드 상태라면 요청 취소
        if (isGuardState) return;

        // 다음 공격을 예약하지 못하는 경우라면 함수 호출 종료.
        if (_NextAttack != null) return;

        // 현재 진행중인 공격이 존재하는 경우
        if (_CurrentAttack != null)
        {
            // 공격을 추가할 수 없는 경우 함수 호출 종료.
            if (!_CurrentAttack.IsAttackAddable(attackCode)) return;
        }

        // 연계 가능한 공격 코드로 변환
        ConvertLinkableAttackCode(ref attackCode);

        // 공격 정보를 얻습니다.
        PlayerAttackInfo attackInfo;
        if (!_PlayerAttackInfoScriptableObject.TryGetPlayerAttackInfo(
            attackCode, out attackInfo)) return;

        // 다음 공격을 설정합니다.
        _NextAttack = PlayerAttackBase.GetPlayerAttack(_PlayerCharacter, attackInfo);
    }

    /// <summary>
    /// 공격을 취소합니다.
    /// </summary>
    private void CancelAttack()
    {
        // 공격 취소
        if (isAttacking || (_NextAttack != null))
        {
            // 공격 영역 검사 끝
            m_EquippedWeapon?.StopAttackAreaCheck();

            // 공격 객체 비우기
            _CurrentAttack = _NextAttack = null;

            // 공격 상태 취소
            isAttacking = false;

            // 공격 취소됨 이벤트 발생
            onAttackCanceled?.Invoke();
        }

    }

    /// <summary>
    /// 공격 애니메이션이 끝났을 경우 호출되는 메서드
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
    /// 공격 영역 검사 시작
    /// </summary>
    private void CALLBACK_OnAttackAreaCheckStarted()
    {
        m_EquippedWeapon?.StartAttackAreaCheck();
    }

    /// <summary>
    /// 공격 영역 검사 끝
    /// </summary>
    private void CALLBACK_OnAttackAreaCheckFinished()
    {
        m_EquippedWeapon?.StopAttackAreaCheck();
    }

    /// <summary>
    /// 피해를 입을 수 있는 객체를 감지한 경우
    /// </summary>
    /// <param name="to"></param>
    private void CALLBACK_OnDamageableObjectDetected(IDamageable to)
    {
        _CurrentAttack?.OnDamageableObjectDetected(to);
        onNewDamageableDetected(to);
    }

    /// <summary>
    /// 피해를 입을 경우 호출되는 메서드
    /// </summary>
    /// <param name="damageInstance"></param>
    private void CALLBACK_OnHit(DamageBase damageInstance)
        => CancelAttack();

}
