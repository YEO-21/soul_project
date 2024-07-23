using UnityEngine;

public sealed class EnemySkeleton : EnemyCharacterBase
{
    private SkeletonAttack _SkeletonAttack;

    private SkeletonAnimController _AnimController;

    /// <summary>
    /// ��ǥ ȸ���� ��Ÿ���� ����
    /// </summary>
    private Vector3 _TargetDirection;

    public SkeletonAttack attack => _SkeletonAttack ?? 
        (_SkeletonAttack = GetComponent<SkeletonAttack>());

    public SkeletonAnimController animController => _AnimController ??
        (_AnimController = GetComponentInChildren<SkeletonAnimController>());

    #region �̺�Ʈ
    /// <summary>
    /// �̵� �ӷ� ���� �̺�Ʈ
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
        // �ӷ��� ����ϴ�.
        float speed = navAgent.velocity.magnitude;
        onMoveSpeedChanged?.Invoke(speed);

        // ��ǥ�� ���� ȸ���մϴ�.
        TurnToTarget();
    }

    private void TurnToTarget()
    {
        // NavMeshAgent ������Ʈ�� Ȱ��ȭ�� ��� �Լ� ȣ�� ����
        if (navAgent.enabled) return;

        // ��ǥ ȸ���� ����մϴ�.
        float targetYawAngle = (Mathf.Atan2(_TargetDirection.x, _TargetDirection.z) * Mathf.Rad2Deg);
        float currentYawAngle = transform.eulerAngles.y;

        float newYawAngle = Mathf.MoveTowardsAngle(
            currentYawAngle, targetYawAngle, navAgent.angularSpeed * Time.deltaTime);
        transform.eulerAngles = Vector3.up * newYawAngle;
    }

    public override void OnHit(DamageBase damageInstance)
    {
        base.OnHit(damageInstance);

        // �� ���⿡�� ���ظ� �Ծ����� Ȯ���մϴ�.
        bool damagedFromBack = damageInstance.IsDamagedFromBackward(transform);

        // ���ظ� ���� ������ ����մϴ�.
        Vector3 direction = transform.position - damageInstance.from.position;
        direction.y = 0.0f;
        direction.Normalize();

        // ������ �����մϴ�.
        transform.forward = direction * (damagedFromBack ? 1 : -1);
    }


    private void CALLBACK_OnAttackStarted()
    {
        // ���� ������ ��ǥ �������� �����մϴ�.
        _TargetDirection = attack.attackDirection;

        // NavMeshAgent ��� ��Ȱ��ȭ
        navAgent.SetDestination(transform.position);
        navAgent.enabled = false;
    }


}
