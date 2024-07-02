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
    /// BehaviorController ���� �������� ��Ʈ ��带 ��Ÿ���ϴ�.
    /// </summary>
    private RunnableBehavior _RootRunnable;

    /// <summary>
    /// ��û�� �ൿ ����� �ð��� ��Ÿ���ϴ�.
    /// </summary>
    private float _BehaviorRestartRequestTime;

    /// <summary>
    /// ����� ��û�� ������ ��Ÿ���ϴ�.
    /// </summary>
    private bool _BehaviorRestartRequested;

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

        // ����� ��û�� �����Ѵٸ�
        if (_BehaviorRestartRequested)
        {
            if(_BehaviorRestartRequestTime <= Time.time) 
            {
                // ��û ó����
                _BehaviorRestartRequested = false;

                // �ൿ �����
                OnBehaviorRestarted();
            }
        }
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
    /// �ൿ�� ����� �� �� ȣ��˴ϴ�.
    /// </summary>
    protected virtual void OnBehaviorRestarted()
    {

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
        // �ൿ ����
        if (_RootRunnable != null)
        {
            _RootRunnable.OnBehaviorFinished();
            _RootRunnable = null;
        }

        // �������� ��ƾ�� �����Ѵٸ�
        if (_BehaviorRoutine !=null)
        {
            // ��ƾ �ߴ�
            StopCoroutine(_BehaviorRoutine);
            _BehaviorRoutine = null;

        }
    }

    /// <summary>
    /// �ൿ ������� ��û�մϴ�.
    /// �������� �ൿ�� �ﰢ������ �ߴܵǸ�, ���� ������� �ƴ� ���
    /// ��û�� �ð��� ���� ���� �ð��� �ൿ�� ����۵˴ϴ�.
    /// </summary>
    /// <param name="startDelay"></param>
    /// <param name="forceRestart"></param>
    public virtual void BehaviorStartRequest(
        float startDelay = 0.0f, bool forceRestart = false)
    {
        Debug.Log("�ൿ ����� ��û��");


        // �ൿ �ߴ�
        StopBehavior();

        // ����� ��û�� �ð��� ����մϴ�.
        float requestTime = Time.time + startDelay;

        if(forceRestart)
        {
            _BehaviorRestartRequestTime = requestTime;
           
        }

        // ���� ��û�� ������ ������ �ð����� ���� ������ ��� ��û ����
        else if(_BehaviorRestartRequestTime > requestTime) return;

        _BehaviorRestartRequestTime = requestTime;
        _BehaviorRestartRequested = true;


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
            _RootRunnable = new TRunnableBehavior();

            // �ൿ ��ü �ʱ�ȭ ���� ��
            if (_RootRunnable.OnInitialized(this))
            {
                // �ൿ�� ���۽�Ű�� �ൿ�� ���� ������ ����մϴ�.
                
                yield return _RootRunnable.OnBehaivorStarted();

            }
            else yield return null;

            _RootRunnable = null;
        }
    }


    protected virtual void OnDestroy() => StopBehavior();

#if UNITY_EDITOR
    protected virtual void OnDrawGizmos()
    {
        foreach(SenseBase senseInstance in m_Senses)
        {
            senseInstance.OnDrawGizmos();
        }

        _RootRunnable?.OnDrawGizmos();
    }
#endif


}
