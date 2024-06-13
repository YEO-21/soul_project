using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 플레이어 캐릭터 공격을 관리하는 컴포넌트
/// </summary>
public sealed class PlayerCharacterAttack : MonoBehaviour
{
    /// <summary>
    /// 플레이어 공격 정보 ScriptableObject 에셋
    /// </summary>
    private PlayerAttackInfoScriptableObject _PlayerAttackInfoScriptableObject;


    /// <summary>
    /// 현재 공격
    /// </summary>
    private PlayerAttackBase _CurrentAttack;


    /// <summary>
    /// 다음 공격
    /// </summary>
    private PlayerAttackBase _NextAttack;

    private void Awake()
    {
        // 플레이어 공격 정보 에셋을 얻습니다.
        _PlayerAttackInfoScriptableObject = 
            GameManager.instance.m_PlayerAttackInfoScriptableObject;
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

        // 요청된 공격을 현재 공격으로 설정합니다.
        _CurrentAttack = _NextAttack;
        Debug.Log($"실행된 공격 이름 : {_CurrentAttack.attackInfo.m_AttackName}");

        // 요청된 공격 처리 완료
        _NextAttack = null;

    }

    /// <summary>
    /// 공격을 요청합니다.
    /// </summary>
    /// <param name="attackCode"/>요청시킬 공격 코드를 전달합니다.</param>>
    public void RequestAttack(string attackCode)
   {
        // 다음 공격을 예약하지 못하는 경우라면 함수 호출 종료
        if (_NextAttack != null) return;

        // 연계 가능한 공격 코드로 변환


        // 공격 정보를 얻습니다.
        PlayerAttackInfo attackInfo;
        if (!_PlayerAttackInfoScriptableObject.TryGetPlayerAttackInfo(
            attackCode, out attackInfo)) return;

        // 다음 공격을 설정합니다
        _NextAttack = PlayerAttackBase.GetPlayerAttack(attackInfo);

        Debug.Log($"예약된 공격 이름 : {_NextAttack.attackInfo.m_AttackName}");
   }

}
