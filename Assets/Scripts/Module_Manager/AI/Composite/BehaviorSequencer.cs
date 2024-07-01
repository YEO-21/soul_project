using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ��ϵ� �ൿ���� ���������� �����ϴ� ��ü�Դϴ�.
/// ��ϵ� ������� �ൿ�� �����ϸ�, 
/// �����Ų �ൿ�� ���е� ������ ��ϵ� �ൿ�� �����մϴ�.
/// </summary>
public class BehaviorSequencer : BehaviorCompositeBase
{


    public override IEnumerator OnBehaivorStarted()
    {
        // �⺻������ ���� ���¿��� ����ǵ��� �մϴ�.
        isSucceeded = true;

        foreach(System.Func<RunnableBehavior> getRunnable in m_Runnalbes)
        {
            // ��Ͻ�Ų �ൿ ��ü ����
            childBehavior = getRunnable.Invoke();
           

            if (childBehavior.OnInitialized(behaviorController))
            {
                // �ൿ ����
                yield return childBehavior.OnBehaivorStarted();

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
