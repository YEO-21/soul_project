using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public sealed class SkeletonAnimController : AnimController
{
    private const string PARAM_SPEED = "_Speed";
    private const string PARAM_ISDAMAGED = "_IsDamaged";
    private const string PARAM_ISCRITICALDAMAGE = "_IsCriticalDamage";
    private const string PARAM_DAMAGEDDIRECTIONZ = "_DamagedDirectionZ";
    private const string PARAM_ATTACKREQUESTED = "_AttackRequested";

    private EnemySkeleton _Skeleton;

    #region 이벤트
    /// <summary>
    /// 공격 애니메이션 끝 이벤트
    /// </summary>
    public event System.Action onAttackAnimationFinished;
    #endregion

    public void Initialize(EnemySkeleton skeleton)
    {
        _Skeleton = skeleton;

        skeleton.onMoveSpeedChanged += CALLBACK_OnMoveSpeedChanged;
        skeleton.onHit += CALLBACK_OnDamaged;

        skeleton.attack.onAttackStarted += CALLBACK_OnAttackStarted;
     

        

    }

    private void CALLBACK_OnMoveSpeedChanged(float speed)
    {
        SetParam(PARAM_SPEED, speed);
    }

    private void CALLBACK_OnDamaged(DamageBase damageInstance)
    {
        #region YeoStyle
        //SetParam(PARAM_ISDAMAGED);

        //// 적 캐릭터 앞 방향벡터
        //Vector3 skeletonForward = _Skeleton.transform.forward;

        //// 플레이어 캐릭터 앞 방향벡터
        //Vector3 playerForwawrd = damageInstance.from.forward;

        //// 방향벡터의 곱 연산
        //bool IsBackAttack = (skeletonForward.z * playerForwawrd.z) > 0;

        //float attackNumber = 0.0f;

        //// 뒤에서 공격 받았을 경우 -1
        //// 앞에서 공격 받았을 경우 1
        //attackNumber = IsBackAttack ? -1.0f : 1.0f;

        //SetParam(PARAM_DAMAGEDDIRECTIONZ, attackNumber);
        #endregion

        Vector3 thisPos = _Skeleton.transform.position;
        Vector3 fromPos = damageInstance.from.position;
        
        // 피해를 입은 방향 (플레이어로 향하는 방향)
        Vector3 damagedDirection = fromPos - thisPos;
        damagedDirection.y = 0.0f;
        damagedDirection.Normalize();

        // 앞 방향 구하기
        Vector3 thisForward = _Skeleton.transform.forward;

        // 현재 회전 구하기
        float thisYaw = Mathf.Atan2(thisForward.z, thisForward.x) * Mathf.Rad2Deg;

        // 대미지 입은 방향에 대한 Yaw 회전값
        float damagedYaw = Mathf.Atan2(damagedDirection.z, damagedDirection.x) * Mathf.Rad2Deg;

        // 각도차 구하기
        float deltaYaw = Mathf.Abs(Mathf.DeltaAngle(thisYaw, damagedYaw));

        float damagedDirectionZ = deltaYaw > 90.0f ? -1.0f : 1.0f;

        // 피해입음 설정
        if (damageInstance.isCriticalDamage) SetParam(PARAM_ISCRITICALDAMAGE);
        else SetParam(PARAM_ISDAMAGED);


        // 피해 입은 방향 설정
        SetParam(PARAM_DAMAGEDDIRECTIONZ, damagedDirectionZ);

          
            


    }

    private void CALLBACK_OnAttackStarted()
    {
        SetParam(PARAM_ATTACKREQUESTED);
    }

    /// <summary>
    /// 공격 애니메이션 끝 함수
    /// </summary>
    private void AnimEvent_OnAttackAnimFinished()
        => onAttackAnimationFinished?.Invoke();

}
