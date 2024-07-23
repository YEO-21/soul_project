using UnityEngine;

public sealed class EnemySkeleton : EnemyCharacterBase
{
    private SkeletonAttack _SkeletonAttack;

    private SkeletonAnimController _AnimController;

    /// <summary>
    /// 목표 회전을 나타내는 방향
    /// </summary>
    private Vector3 _TargetDirection;

    public SkeletonAttack attack => _SkeletonAttack ?? 
        (_SkeletonAttack = GetComponent<SkeletonAttack>());

    public SkeletonAnimController animController => _AnimController ??
        (_AnimController = GetComponentInChildren<SkeletonAnimController>());

    #region 이벤트
    /// <summary>
    /// 이동 속력 변경 이벤트
    /// </summary>
    public event System.Action<float> onMoveSpeedChanged;
    #endregion


    private void Start()
    {
        (behaviorController as SkeletonBehaviorController).Initialize(this);
        animController.Initialize(this);
        attack.Initialize(this);

        attack.onAttackStarted += CALLBACK_OnAttackStarted;
    }
    private void Update()
    {
        // 속력을 얻습니다.
        float speed = navAgent.velocity.magnitude;
        onMoveSpeedChanged?.Invoke(speed);

        // 목표를 향해 회전합니다.
        TurnToTarget();
    }

    private void TurnToTarget()
    {
        // NavMeshAgent 컴포넌트가 활성화된 경우 함수 호출 종료
        if (navAgent.enabled) return;

        // 목표 회전을 계산합니다.
        float targetYawAngle = (Mathf.Atan2(_TargetDirection.x, _TargetDirection.z) * Mathf.Rad2Deg);
        float currentYawAngle = transform.eulerAngles.y;

        float newYawAngle = Mathf.MoveTowardsAngle(
            currentYawAngle, targetYawAngle, navAgent.angularSpeed * Time.deltaTime);
        transform.eulerAngles = Vector3.up * newYawAngle;
    }

    public override void OnHit(DamageBase damageInstance)
    {
        base.OnHit(damageInstance);

        // 뒷 방향에서 피해를 입었는지 확인합니다.
        bool damagedFromBack = damageInstance.IsDamagedFromBackward(transform);

        // 피해를 입은 방향을 계산합니다.
        Vector3 direction = transform.position - damageInstance.from.position;
        direction.y = 0.0f;
        direction.Normalize();

        // 방향을 적용합니다.
        transform.forward = direction * (damagedFromBack ? 1 : -1);
    }


    private void CALLBACK_OnAttackStarted()
    {
        // 공격 방향을 목표 방향으로 설정합니다.
        _TargetDirection = attack.attackDirection;

        // NavMeshAgent 잠시 비활성화
        navAgent.SetDestination(transform.position);
        navAgent.enabled = false;
    }


}
