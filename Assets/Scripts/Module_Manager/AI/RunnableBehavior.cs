

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Behavior Controller 객체를 통해 실행될 수 있는 객체의 최상위 형태
/// </summary>
public abstract class RunnableBehavior
{
    /// <summary>
    /// 이 Task 에서 사용되는 서비스 목록입니다.
    /// </summary>
    private List<System.Func<BehaviorService>> _Services = new List<System.Func<BehaviorService>>();

    /// <summary>
    /// 서비스 루틴을 나타냅니다.
    /// </summary>
    private Coroutine _ServiceRoutine;

    /// <summary>
    /// 생성된 서비스 객체들을 나타냅니다.
    /// </summary>
    public List<BehaviorService> behaviorServices { get; private set; }

    /// <summary>
    /// 이 행동을 제어하는 객체를 나타냅니다.
    /// </summary>
    public BehaviorController behaviorController { get; private set; } 


    /// <summary>
    /// 이 행동의 실행 성공 여부를 나타냅니다.
    /// </summary>
    public bool isSucceeded { get; protected set; }

    /// <summary>
    /// 이 행동 객체가 초기화될 때 호출되는 메서드입니다.
    /// </summary>
    /// <param name="behaviorController">이 행동을 제어하는 객체가 전달됩니다.</param>
    public virtual bool OnInitialized(BehaviorController behaviorController)
    {
        this.behaviorController = behaviorController;

        return true;
    }


    /// <summary>
    /// 행동이 시작되었을 때 호출되는 함수입니다.
    /// 동작 방식을 정의합니다.
    /// </summary>
    /// <returns></returns>
    public abstract IEnumerator OnBehaivorStarted();


}
