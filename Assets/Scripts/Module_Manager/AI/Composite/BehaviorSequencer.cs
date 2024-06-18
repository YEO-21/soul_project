using System.Collections;
using Unity.VisualScripting;

/// <summary>
/// 등록된 행동들을 순차적으로 실행하는 객체입니다.
/// 등록된 순서대로 행동을 실행하며, 
/// 실행시킨 행동이 실패될 때까지 등록된 행동을 실행합니다.
/// </summary>
public class BehaviorSequencer : BehaviorCompositeBase
{
    public BehaviorSequencer()
    {
        //AddBehavior<Task_Sample>();
        AddBehavior(() => new Task_Sample("행동1"));
        AddBehavior(() => new Task_Sample("행동2"));
        AddBehavior(() => new Task_Sample("행동3"));
    }


    public override IEnumerator OnBehaivorStarted()
    {
        // 기본적으로 성공 상태에서 실행되도록 합니다.
        isSucceeded = true;

        foreach(System.Func<RunnableBehavior> getRunnable in m_Runnalbes)
        {
            // 등록시킨 행동 객체 생성
            RunnableBehavior runnable = getRunnable.Invoke();

            // 행동 실행
            yield return runnable.OnBehaivorStarted();

            // 실행한 행동이 실패한 경우 다음 행동을 실행하지 않도록 합니다.
            if(!runnable.isSucceeded)
            {

                // 다음 행동을 실행하지 않도록 합니다.
                isSucceeded = false;
                yield break;
            }
        }

    }
}
