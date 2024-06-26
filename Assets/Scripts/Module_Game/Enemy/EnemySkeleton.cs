using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemySkeleton : EnemyCharacterBase
{
    private SkeletonAnimController _AnimController;

    public SkeletonAnimController animController => _AnimController ??
        (_AnimController = GetComponentInChildren<SkeletonAnimController>());

    #region 이벤트
    /// <summary>
    /// 이동 속력 변경 시 발생하는 이벤트
    /// </summary>
    public event System.Action<float> onMoveSpeedChanged;
    #endregion



    private void Update()
    {
        // 속력을 얻습니다.
        float speed = navAgent.velocity.magnitude;
        onMoveSpeedChanged?.Invoke(speed); ;
    }

    private void Start()
    {
        animController.Initialize(this);
    }

}
