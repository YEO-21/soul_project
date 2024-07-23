using System.Collections;


/// <summary>
/// ��ϵ� �ൿ���� ���������� �����ϴ� ��ü�Դϴ�.
/// ��ϵ� ������� �ൿ�� �����ϸ�
/// �����Ų �ൿ�� ���е� ������ ��ϵ� �ൿ�� �����մϴ�.
/// </summary>
public class BehaviorSequencer : BehaviorCompositeBase
{
    public override IEnumerator OnBehaviorStarted()
    {
        // �⺻������ ���� ���¿��� ����ǵ��� �մϴ�.
        isSucceeded = true;

        foreach(System.Func<RunnableBehavior> getRunnable in m_Runnables)
        {
            // ��Ͻ�Ų �ൿ ��ü ����
            childBehavior = getRunnable.Invoke();

            if (childBehavior.OnInitialized(behaviorController))
            {
                // �ൿ ����
                yield return childBehavior.OnBehaviorStarted();

                // ������ �ൿ�� ������ ��� ���� �ൿ�� �������� �ʵ��� �մϴ�.
                if (!childBehavior.isSucceeded)
                {
                    // ���� �ൿ�� �������� �ʵ��� �մϴ�.
                    isSucceeded = false;
                    yield break;
                }
            }
            else 
            {
                isSucceeded = false;
                yield return null;
            }
        }


    }
}
