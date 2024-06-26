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


}
