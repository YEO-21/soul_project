using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 스켈레톤 적 캐릭터의 행동 제어 컴포넌트입니다.
/// </summary>
public sealed class SkeletonBehaviorController : EnemyBehaviorController
{
    protected override void Awake()
    {
        base.Awake();

        SetKey(KEY_MAXMOVEDISTANCE, 10.0f);

        SightSense sightSense = RegisterSense<SightSense>();
        sightSense.sightRadius = 2.0f;
        sightSense.sightMaxAngle = 90.0f;
        sightSense.detectHeight = 1.5f;
        sightSense.detectLayer = LayerMask.GetMask("PlayerCharacter");

        sightSense.onTargetDetected += CALLBACK_OnNewTargetDetected;
        sightSense.onTargetLost += CALLBACK_OnTargetLost;


    }


    private void Start()
    {
        StartBehaivor<SkeletonRootBehavior>();
        
    }

    public void Initialize(EnemySkeleton skeleton)
    {
        skeleton.onHit += CALLBACK_OnHit;
    }

    private void CALLBACK_OnHit(DamageBase damageInstance)
    {
        // 피해를 입힌 플레이어 캐릭터 객체를 얻습니다.
        PlayerCharacter playerChracter = damageInstance.from.GetComponent<PlayerCharacter>();

        if (!playerChracter) return;

        // 공격적인 상태로 설정합니다.
        SetKey(KEY_ISAGGRESSIVESTATE, true);

        // 플레이어 캐릭터 객체 설정
        SetKey(KEY_PLAYERCHARACTER, playerChracter);
        
    }

    private void CALLBACK_OnNewTargetDetected(GameObject newTarget)
    {
        // 피해를 입힌 플레이어 캐릭터 객체를 얻습니다.
        PlayerCharacter playerChracter = newTarget.GetComponent<PlayerCharacter>();

        if(playerChracter)
        {
            // 플레이어 캐릭터 객체 설정
            SetKey(KEY_PLAYERCHARACTER, playerChracter);

            // 공격적인 상태로 설정합니다.
            SetKey(KEY_ISAGGRESSIVESTATE, true);
        }
        
    }

    private void CALLBACK_OnTargetLost(GameObject lostTarget)
    {
        PlayerCharacter playerChracter = lostTarget.GetComponent<PlayerCharacter>();


        if (playerChracter)
        {
            SetKey(KEY_ISAGGRESSIVESTATE, false);
            SetKey(KEY_PLAYERCHARACTER, null);
        }
    }


}
