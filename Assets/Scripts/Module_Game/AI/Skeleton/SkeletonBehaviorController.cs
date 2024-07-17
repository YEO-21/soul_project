using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���̷��� �� ĳ������ �ൿ ���� ������Ʈ�Դϴ�.
/// </summary>
public sealed class SkeletonBehaviorController : EnemyBehaviorController
{

    public const string KEY_ISATTACKABLE = "IsAttackable";

    protected override void Awake()
    {
        base.Awake();

        SetKey(KEY_MAXMOVEDISTANCE, 10.0f);
        SetKey(KEY_ISATTACKABLE, false);

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
        StartBehavior();
    }

    public void Initialize(EnemySkeleton skeleton)
    {
        skeleton.onHit += CALLBACK_OnHit;
    }

    private void CALLBACK_OnHit(DamageBase damageInstance)
    {
        // ���ظ� ���� �÷��̾� ĳ���� ��ü�� ����ϴ�.
        PlayerCharacter playerCharacter = damageInstance.from.GetComponent<PlayerCharacter>();

        // ���� ���� ���� ���
        SetKey(KEY_ISATTACKABLE, false);

        if (!playerCharacter) return;

        if (agent.enabled) agent.SetDestination(transform.position);

        // �������� ���·� �����մϴ�.
        SetKey(KEY_ISAGGRESSIVESTATE, true);

        // �÷��̾� ĳ���� ��ü ����
        SetKey(KEY_PLAYERCHARACTER, playerCharacter);

        BehaviorStartRequest(2.0f);
    }

    private void CALLBACK_OnNewTargetDetected(GameObject newTarget)
    {
        PlayerCharacter playerCharacter = newTarget.GetComponent<PlayerCharacter>();

        if (playerCharacter)
        {
            // �������� ���·� �����մϴ�.
            SetKey(KEY_ISAGGRESSIVESTATE, true);

            // �÷��̾� ĳ���� ��ü ����
            SetKey(KEY_PLAYERCHARACTER, playerCharacter);
        }
    }

    private void CALLBACK_OnTargetLost(GameObject lostTarget)
    {
        PlayerCharacter playerCharacter = lostTarget.GetComponent<PlayerCharacter>();

        if (playerCharacter)
        {
            SetKey(KEY_ISAGGRESSIVESTATE, false);
            SetKey(KEY_PLAYERCHARACTER, null);
        }
    }

    public override void StartBehavior()
    {
        base.StartBehavior();
        StartBehavior<SkeletonRootBehavior>();
    }

}
