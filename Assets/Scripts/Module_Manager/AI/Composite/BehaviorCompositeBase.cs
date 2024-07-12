
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 컴포짓을 나타내기 위한 클래스입니다.
/// </summary>
public abstract class BehaviorCompositeBase : RunnableBehavior
{
    /// <summary>
    /// 순차적으로 실행시킬 행동 객체들을 나타냅니다.
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
