

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
    /// �������� ���� ��带 ��Ÿ���ϴ�.
    /// </summary>
    public RunnableBehavior childBehavior { get; protected set; }

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

        // �����ų ���񽺰� �����Ѵٸ�
        if(_Services.Count != 0)
        {
            // ��ϵ� ���� ��ü�� ��� ����/�ʱ�ȭ�մϴ�.
            behaviorServices = new List<BehaviorService>();
            foreach(System.Func<BehaviorService> getService in _Services)
            {
                // ���� ��ü ����
                BehaviorService runService = getService.Invoke();

                // ���� ��ü ����
                runService.OnServiceStarted(behaviorController);

                // �����ų ���� ��ü ���
                behaviorServices.Add(runService);

            }
            // ���� ����
            _ServiceRoutine = behaviorController.StartCoroutine(ServiceRoutine());

        }
        


        return true;
    }

    public virtual IEnumerator ServiceRoutine()
    {
        while(behaviorServices != null)
        {
            foreach(BehaviorService service in behaviorServices)
            {
                service.ServiceTick();
            }

            yield return null;
        }
    }


    /// <summary>
    /// �ൿ�� ���۵Ǿ��� �� ȣ��Ǵ� �Լ��Դϴ�.
    /// ���� ����� �����մϴ�.
    /// </summary>
    /// <returns></returns>
    public abstract IEnumerator OnBehaivorStarted();

    public virtual void OnBehaviorFinished()
    {
        // ���� ��� ����ó��
        childBehavior?.OnBehaviorFinished();

        // ���񽺸� �����ϰ� �ִ� ���
        if(_ServiceRoutine != null)
        {
            // ���� ��ƾ ����
            behaviorController.StopCoroutine(_ServiceRoutine);
            _ServiceRoutine = null;

            // ���� ��ü ����ó��
            foreach(BehaviorService service in behaviorServices)
                service.OnServiceFinished();

            // ���� ��ü�� ����
            behaviorServices.Clear();

        }

    }

    public void AddService<TBehaviorService>() where
        TBehaviorService : BehaviorService, new()
        => AddService(() => new TBehaviorService());

    public void AddService(System.Func<BehaviorService> fngetService)
        => _Services.Add(fngetService);

#if UNITY_EDITOR
    public virtual void OnDrawGizmos()
    {

        if(behaviorServices != null)
        {
            foreach(BehaviorService service in behaviorServices)
            {
                service.OnDrawGizmos();
            }
        }

        childBehavior?.OnDrawGizmos();
    }
#endif
}
