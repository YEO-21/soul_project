using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ϵ� �ൿ���� ������ ��ü�Դϴ�.
/// �⺻������ Task �� Composite �� �̿��Ͽ� �ൿ�� ������ �� �ֽ��ϴ�.
/// Task : �ϳ��� �ൿ(�۾�)�� ��Ÿ���ϴ�.
/// Composite : �бⰡ ����Ǵ� ����� �⺻ ��Ģ�� �����մϴ�.
/// Service : Task �� Composite �� �߰��Ǿ� �ش� �бⰡ 
/// �������� �� ���ÿ� ����Ǹ� Ư���� �����͸� �����մϴ�.
/// 
/// BehaviorController : �ϳ��� �ൿ(Root) �� ����Ǹ� 
/// �ൿ�� �����ϴ� �⺻���� ����� �����մϴ�.
/// </summary>
public class BehaviorController : MonoBehaviour
{
   

    /// <summary>
    /// �ൿ ��ƾ�� ��Ÿ���ϴ�.
    /// </summary>
    private Coroutine _BehaviorRoutine;

    /// <summary>
    /// �ൿ ��ü���� ����ϰ� �� �����͸� ��Ÿ���ϴ�.
    /// </summary>
    protected Dictionary<string, object> m_Keys = new();

    /// <summary>
    /// ���� ��ü���� ��Ÿ���ϴ�.
    /// </summary>
    protected List<SenseBase> m_Senses = new();

    /// <summary>
/// m_Keys�� ���� �б� ���� ������Ƽ �Դϴ�.
/// </summary>
    public Dictionary<string, object> keys => m_Keys;


    protected virtual void Update()
    {
        SenseTick();
    }

    /// <summary>
    /// ���� ��ü Tick
    /// </summary>
    protected virtual void SenseTick()
    {
        foreach(SenseBase senseInstance in m_Senses)
        {
            senseInstance.OnSenseUpdated();
        }
    }

    /// <summary>
    /// �ൿ�� �����ŵ�ϴ�.
    /// </summary>
    /// <typeparam name="TRunnableBehavior">�����ų �ൿ ������ �����մϴ�.</typeparam>
    public void StartBehaivor<TRunnableBehavior>()
        where TRunnableBehavior : RunnableBehavior, new()
    {
        _BehaviorRoutine = StartCoroutine(Run<TRunnableBehavior>());
    }

    /// <summary>
    /// �ൿ�� �ߴ��մϴ�.
    /// </summary>
    public void StopBehavior()
    {
        // �������� ��ƾ�� �����Ѵٸ�
        if(_BehaviorRoutine !=null)
        {
            // ��ƾ �ߴ�
            StopCoroutine(_BehaviorRoutine);
            _BehaviorRoutine = null;

        }
    }

    /// <summary>
    /// Key�� �߰��ϰų�, �����մϴ�.
    /// </summary>
    /// <param name="keyName"></param>
    /// <param name="value"></param>
    public void SetKey(string keyName, object value = null)
       => m_Keys[keyName] = value;

    public T GetKey<T>(string keyName) => (T)GetKey(keyName);
    

    public object GetKey(string keyName)
    {
        if(m_Keys.TryGetValue(keyName, out object value))
        {
            return value;
        }

#if UNITY_EDITOR
        Debug.LogError($"[{keyName}] �� ���� ������ ã�� �� �����ϴ�.");
#endif
        return null;
    }

    /// <summary>
    /// ���� ��ü�� ����մϴ�.
    /// </summary>
    /// <typeparam name="TSense">���� ��ü ������ �����մϴ�.</typeparam>
    /// <returns></returns>
    public TSense RegisterSense<TSense>()
        where TSense : SenseBase, new()
    {
        // ���� ��ü ����
        TSense senseInstance = new TSense();

        // ���� ��ü �ʱ�ȭ
        senseInstance.OnSenseIntialized(this);

        // ���� ��ü �߰�
        m_Senses.Add(senseInstance);

        return senseInstance;

    }


    private IEnumerator Run<TRunnableBehavior>()
        where TRunnableBehavior : RunnableBehavior, new()
    {
        // �ൿ���� ��� �����ŵ�ϴ�.
        while(true)
        {

            // �ൿ ��ü�� �����մϴ�.
            TRunnableBehavior root = new TRunnableBehavior();

            // �ൿ ��ü �ʱ�ȭ ���� ��
            if (root.OnInitialized(this))
            {
                // �ൿ�� ���۽�Ű�� �ൿ�� ���� ������ ����մϴ�.
                yield return root.OnBehaivorStarted();
            }
            else yield return null;
        }
    }


    protected virtual void OnDestroy() => StopBehavior();


}
