

using System.Collections;
/// <summary>
/// Behavior Controller ��ü�� ���� ����� �� �ִ� ��ü�� �ֻ��� ����
/// </summary>
public abstract class RunnableBehavior
{
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
