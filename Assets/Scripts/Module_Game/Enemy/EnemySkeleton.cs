using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemySkeleton : EnemyCharacterBase
{
    private SkeletonAnimController _AnimController;

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
        animController.Initialize(this);
    }

}
