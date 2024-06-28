

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Behavior Controller ��ü�� ���� ����� �� �ִ� ��ü�� �ֻ��� ����
/// </summary>
public abstract class RunnableBehavior
{
    /// <summary>
    /// �� Task ���� ���Ǵ� ���� ����Դϴ�.
    /// </summary>
    private List<System.Func<BehaviorService>> _Services = new List<System.Func<BehaviorService>>();

    /// <summary>
    /// ���� ��ƾ�� ��Ÿ���ϴ�.
    /// </summary>
    private Coroutine _ServiceRoutine;

    /// <summary>
    /// ������ ���� ��ü���� ��Ÿ���ϴ�.
    /// </summary>
    public List<BehaviorService> behaviorServices { get; private set; }

    /// <summary>
    /// �� �ൿ�� �����ϴ� ��ü�� ��Ÿ���ϴ�.
    /// </summary>
    public BehaviorController behaviorController { get; private set; } 


    /// <summary>
    /// �� �ൿ�� ���� ���� ���θ� ��Ÿ���ϴ�.
    /// </summary>
    public bool isSucceeded { get; protected set; }

    /// <summary>
    /// �� �ൿ ��ü�� �ʱ�ȭ�� �� ȣ��Ǵ� �޼����Դϴ�.
    /// </summary>
    /// <param name="behaviorController">�� �ൿ�� �����ϴ� ��ü�� ���޵˴ϴ�.</param>
    public virtual bool OnInitialized(BehaviorController behaviorController)
    {
        this.behaviorController = behaviorController;

        return true;
    }


    /// <summary>
    /// �ൿ�� ���۵Ǿ��� �� ȣ��Ǵ� �Լ��Դϴ�.
    /// ���� ����� �����մϴ�.
    /// </summary>
    /// <returns></returns>
    public abstract IEnumerator OnBehaivorStarted();


}
