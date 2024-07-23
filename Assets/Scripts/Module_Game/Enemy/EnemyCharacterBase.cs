using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// �� ��ü�� ��Ÿ���� ���� Ŭ�����Դϴ�.
/// </summary>
public abstract class EnemyCharacterBase : MonoBehaviour,
    IDamageable
{
    [Header("# �� �ڵ�")]
    public string m_EnemyCode;

    private EnemyBehaviorController _BehaviorController;

    private NavMeshAgent _NavAgent;

    /// <summary>
    /// �� ������ ��Ÿ���ϴ�.
    /// </summary>
    public EnemyInfo enemyInfo { get; private set; }

    public BehaviorController behaviorController => _BehaviorController ??
        (_BehaviorController = GetComponent<EnemyBehaviorController>());
    public NavMeshAgent navAgent => _NavAgent ?? 
        (_NavAgent = GetComponent<NavMeshAgent>());

    public string objectName => enemyInfo.m_Name;
    public float currentHp { get; protected set; }
    public float maxHp => enemyInfo.m_MaxHp;

    #region �̺�Ʈ
    // ���ظ� ���� ��� �߻��ϴ� �̺�Ʈ
    public event System.Action<DamageBase> onHit;
    #endregion

    protected virtual void Awake()
    {
        // �� ������ �ʱ�ȭ�մϴ�.
        InitializeEnemyInfo();
    }

    private void InitializeEnemyInfo()
    {
        EnemyInfoScriptableObject enemyInfoScriptableObject = GameManager.instance.m_EnemyInfoScriptableObject;

        if (enemyInfoScriptableObject.TryGetEnemyInfo(
            m_EnemyCode, out EnemyInfo findedEnemyInfo))
        {
            enemyInfo = findedEnemyInfo;
            currentHp = enemyInfo.m_MaxHp;
        }
#if UNITY_EDITOR
        else Debug.Log($"[{m_EnemyCode}] �� ���� �� ������ ã�� �� �����ϴ�.");
#endif

    }


    /// <summary>
    /// �� ĳ���Ͱ� ���ظ� ���� ��� ȣ��Ǵ� �޼����Դϴ�.
    /// </summary>
    /// <param name="damage"></param>
    public virtual void OnHit(DamageBase damageInstance)
    {
        // ���ط��� ����մϴ�. 
        CalculateDamage(damageInstance);
        onHit?.Invoke(damageInstance);
    }

    /// <summary>
    /// ���ط��� ����մϴ�.
    /// </summary>
    /// <param name="damageInstance"></param>
    protected virtual void CalculateDamage(DamageBase damageInstance)
    {
        currentHp -= damageInstance.damage;
    }

}
