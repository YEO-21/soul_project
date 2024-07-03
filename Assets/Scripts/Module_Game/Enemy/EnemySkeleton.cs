using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemySkeleton : EnemyCharacterBase
{
    private SkeletonAttack _SkeletonAttack;

    private SkeletonAnimController _AnimController;

    public SkeletonAttack attack => _SkeletonAttack ?? 
        (_SkeletonAttack = GetComponent<SkeletonAttack>());

    public SkeletonAnimController animController => _AnimController ??
        (_AnimController = GetComponentInChildren<SkeletonAnimController>());

    #region �̺�Ʈ
    /// <summary>
    /// �̵� �ӷ� ���� �� �߻��ϴ� �̺�Ʈ
    /// </summary>
    public event System.Action<float> onMoveSpeedChanged;
    #endregion



    private void Update()
    {
        // �ӷ��� ����ϴ�.
        float speed = navAgent.velocity.magnitude;
        onMoveSpeedChanged?.Invoke(speed); ;
    }

    private void Start()
    {
        (behaviorController as SkeletonBehaviorController).Initialize(this);
        animController.Initialize(this);
        attack.Initialize(this);
    }

}
