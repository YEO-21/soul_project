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
    /// 공격 시작 이벤트
    /// </summary>
    public event System.Action<int /*attackCode*/> onAttackStarted;

    /// <summary>
    /// 구르기 상태 확인을 위한 대리자
    /// </summary>
    private System.Func<bool> _IsDodging;

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

    public void Initialize(
        PlayerCharacter owner,
        PlayerCharacterAnimController animController,
        PlayerCharacterMovement movement)
    {
        _PlayerCharacter = owner;

        animController.onAttackAnimationFinished += CALLBACK_OnAttackAnimationFinished;

        animController.onAttackAreaCheckStarted += CALLBACK_OnAttackAreaCheckStarted;
        animController.onAttackAreaCheckFinished += CALLBACK_OnAttackAreaCheckFinished;

        _IsDodging = () => movement.isDodging;
    }

    private void AnimController_onAttackAreaCheckStarted()
    {
        throw new System.NotImplementedException();
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
        // 요청된 다음 공격이 존재하지 않는다면 함수 호출 종료
        if (_NextAttack == null) return;

        // 현재 공격이 실행중인 경우 함수 호출 종료
        if (_CurrentAttack != null) return;

        // 공격 상태로 설정합니다.
        isAttacking = true;

        // 요청된 공격을 현재 공격으로 설정합니다.
        _CurrentAttack = _NextAttack;


        // 요청된 공격 처리 완료
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
        if(!string.IsNullOrEmpty(linkableAttackCode))
        {
            // 다음 공격을 연계 공격 코드로 설정합니다.
            ref_AttackCode = linkableAttackCode;
        }
    }

    /// <summary>
    /// 공격을 요청합니다.
    /// </summary>
    /// <param name="attackCode"/>요청시킬 공격 코드를 전달합니다.</param>>
    public void RequestAttack(string attackCode)
   {
        // 공격이 요청되었을 때 구르기 상태라면 요청 취소
        if (_IsDodging.Invoke()) return;

        // 다음 공격을 예약하지 못하는 경우라면 함수 호출 종료
        if (_NextAttack != null) return;

        // 현재 진행중인 공격이 존재하는 경우
        if(_CurrentAttack != null)
        {
            // 공격을 추가할 수 없는 경우 함수 호출 종료
            if (!_CurrentAttack.IsAttackAddable(attackCode)) return;
        }

        // 연계 가능한 공격 코드로 변환
        ConvertLinkableAttackCode(ref attackCode);

        // 공격 정보를 얻습니다.
        PlayerAttackInfo attackInfo;
        if (!_PlayerAttackInfoScriptableObject.TryGetPlayerAttackInfo(
            attackCode, out attackInfo)) return;

        // 다음 공격을 설정합니다
        _NextAttack = PlayerAttackBase.GetPlayerAttack(_PlayerCharacter, attackInfo);
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
    ///
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

  
}
