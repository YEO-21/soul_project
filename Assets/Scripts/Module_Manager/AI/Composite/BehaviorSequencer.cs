using System.Collections;
using Unity.VisualScripting;

/// <summary>
/// ��ϵ� �ൿ���� ���������� �����ϴ� ��ü�Դϴ�.
/// ��ϵ� ������� �ൿ�� �����ϸ�, 
/// �����Ų �ൿ�� ���е� ������ ��ϵ� �ൿ�� �����մϴ�.
/// </summary>
public class BehaviorSequencer : BehaviorCompositeBase
{
    public BehaviorSequencer()
    {
        //AddBehavior<Task_Sample>();
        AddBehavior(() => new Task_Sample("�ൿ1"));
        AddBehavior(() => new Task_Sample("�ൿ2"));
        AddBehavior(() => new Task_Sample("�ൿ3"));
    }


    public override IEnumerator OnBehaivorStarted()
    {
        // �⺻������ ���� ���¿��� ����ǵ��� �մϴ�.
        isSucceeded = true;

        foreach(System.Func<RunnableBehavior> getRunnable in m_Runnalbes)
        {
            // ��Ͻ�Ų �ൿ ��ü ����
            RunnableBehavior runnable = getRunnable.Invoke();

            // �ൿ ����
            yield return runnable.OnBehaivorStarted();

            // ������ �ൿ�� ������ ��� ���� �ൿ�� �������� �ʵ��� �մϴ�.
            if(!runnable.isSucceeded)
            {

                // ���� �ൿ�� �������� �ʵ��� �մϴ�.
                isSucceeded = false;
                yield break;
            }
        }

    }
}
