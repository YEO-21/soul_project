
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �������� ��Ÿ���� ���� Ŭ�����Դϴ�.
/// </summary>
public abstract class BehaviorCompositeBase : RunnableBehavior
{
    /// <summary>
    /// ���������� �����ų �ൿ ��ü���� ��Ÿ���ϴ�.
    /// </summary>
    public List<System.Func<RunnableBehavior>> m_Runnalbes = new();

  
    public void AddBehavior<TRunnableBehavior>()
        where TRunnableBehavior : RunnableBehavior, new()
    {
        m_Runnalbes.Add(() => new TRunnableBehavior());

    }

    public void AddBehavior(System.Func<RunnableBehavior> runnable)
    {
        m_Runnalbes.Add(runnable);
    }

   
}
