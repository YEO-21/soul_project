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
    public void SetKey(string keyName, object value)
       => m_Keys[keyName] = value;
    

    private IEnumerator Run<TRunnableBehavior>()
        where TRunnableBehavior : RunnableBehavior, new()
    {
        // �ൿ���� ��� �����ŵ�ϴ�.
        while(true)
        {
            // �ൿ ��ü�� �����մϴ�.
            TRunnableBehavior root = new TRunnableBehavior();


            // �ൿ�� ���۽�Ű�� �ൿ�� ���� ������ ����մϴ�.
            yield return root.OnBehaivorStarted();
        }
    }


    protected virtual void OnDestroy() => StopBehavior();


}
