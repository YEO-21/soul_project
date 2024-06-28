using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ���̷��� �� ĳ������ �ൿ ���� ������Ʈ�Դϴ�.
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
        // ���ظ� ���� �÷��̾� ĳ���� ��ü�� ����ϴ�.
        PlayerCharacter playerChracter = damageInstance.from.GetComponent<PlayerCharacter>();

        if (!playerChracter) return;

        // �������� ���·� �����մϴ�.
        SetKey(KEY_ISAGGRESSIVESTATE, true);

        // �÷��̾� ĳ���� ��ü ����
        SetKey(KEY_PLAYERCHARACTER, playerChracter);
        
    }

    private void CALLBACK_OnNewTargetDetected(GameObject newTarget)
    {
        // ���ظ� ���� �÷��̾� ĳ���� ��ü�� ����ϴ�.
        PlayerCharacter playerChracter = newTarget.GetComponent<PlayerCharacter>();

        if(playerChracter)
        {
            // �÷��̾� ĳ���� ��ü ����
            SetKey(KEY_PLAYERCHARACTER, playerChracter);

            // �������� ���·� �����մϴ�.
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
