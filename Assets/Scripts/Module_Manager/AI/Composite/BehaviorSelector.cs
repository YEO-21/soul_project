
using System.Collections;
using Unity.VisualScripting;

/// <summary>
/// ��ϵ� �ൿ���� ���������� �����ϴ� ��ü�Դϴ�.
/// ��ϵ� ������� �ൿ�� �����ϸ�, 
/// �����Ų �ൿ�� ������ ������ ��ϵ� �ൿ�� �����մϴ�.
/// </summary>
public class BehaviorSelector : BehaviorCompositeBase
{
   
    public override IEnumerator OnBehaivorStarted()
    {
        // �⺻������ ���� ���¿��� ����ǵ��� �մϴ�.
        isSucceeded = false;

        foreach (System.Func<RunnableBehavior> getRunnable in m_Runnalbes)
        {
            // ��Ͻ�Ų �ൿ ��ü ����
            RunnableBehavior runnable = getRunnable.Invoke();

            if (runnable.OnInitialized(behaviorController))
            {
                // �ൿ ����
                yield return runnable.OnBehaivorStarted();

                // ������ �ൿ�� ������ ��� ���� �ൿ�� �������� �ʵ��� �մϴ�.
                if (runnable.isSucceeded)
                {

                    // ���� �ൿ�� �������� �ʵ��� �մϴ�.
                    isSucceeded = true;
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
