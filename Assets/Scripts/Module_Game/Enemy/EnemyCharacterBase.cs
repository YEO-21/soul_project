using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 적 객체를 나타내기 위한 클래스입니다.
/// </summary>
public abstract class EnemyCharacterBase : MonoBehaviour,
    IDamageable
{
    [Header("# 적 코드")]
    public string m_EnemyCode;

    private EnemyBehaviorController _BehaviorController;

    private NavMeshAgent _NavAgent;

    /// <summary>
    /// 적 정보를 나타냅니다.
    /// </summary>
    public EnemyInfo enemyInfo { get; private set; }

    public BehaviorController behaviorController => _BehaviorController ??
        (_BehaviorController = GetComponent<EnemyBehaviorController>());
    public NavMeshAgent navAgent => _NavAgent ?? 
        (_NavAgent = GetComponent<NavMeshAgent>());

    public string objectName => enemyInfo.m_Name;
    public float currentHp { get; protected set; }
    public float maxHp => enemyInfo.m_MaxHp;

    #region 이벤트
    // 피해를 입을 경우 발생하는 이벤트
    public event System.Action<DamageBase> onHit;
    #endregion

    protected virtual void Awake()
    {
        // 적 정보를 초기화합니다.
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
        else Debug.Log($"[{m_EnemyCode}] 에 대한 적 정보를 찾을 수 없습니다.");
#endif

    }


    /// <summary>
    /// 이 캐릭터가 피해를 입을 경우 호출되는 메서드입니다.
    /// </summary>
    /// <param name="damage"></param>
    public virtual void OnHit(DamageBase damageInstance)
    {
        // 피해량을 계산합니다. 
        CalculateDamage(damageInstance);
        onHit?.Invoke(damageInstance);
    }

    /// <summary>
    /// 피해량을 계산합니다.
    /// </summary>
    /// <param name="damageInstance"></param>
    protected virtual void CalculateDamage(DamageBase damageInstance)
    {
        currentHp -= damageInstance.damage;
    }

}
