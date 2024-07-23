
using System.Collections;
using UnityEngine;

/// <summary>
/// 등록된 행동들을 순차적으로 실행하는 객체입니다.
/// 등록된 순서대로 행동을 실행하며
/// 실행시킨 행동이 성공할 때까지 등록된 행동을 실행합니다.
/// </summary>
public class BehaviorSelector : BehaviorCompositeBase
{
    public override IEnumerator OnBehaviorStarted()
    {
        // 기본적으로 실패 상태에서 실행되도록 합니다.
        isSucceeded = false;

        foreach (System.Func<RunnableBehavior> getRunnable in m_Runnables)
        {
            // 등록시킨 행동 객체 생성
            childBehavior = getRunnable.Invoke();

            if (childBehavior.OnInitialized(behaviorController))
            {
                // 행동 실행
                yield return childBehavior.OnBehaviorStarted();

                // 실행한 행동이 성공한 경우 다음 행동을 실행하지 않도록 합니다.
                if (childBehavior.isSucceeded)
                {
                    // 다음 행동을 실행하지 않도록 합니다.
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

