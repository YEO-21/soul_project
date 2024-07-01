

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
    /// 실행중인 하위 노드를 나타냅니다.
    /// </summary>
    public RunnableBehavior childBehavior { get; protected set; }

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

        // 실행시킬 서비스가 존재한다면
        if(_Services.Count != 0)
        {
            // 등록된 서비스 객체를 모두 생성/초기화합니다.
            behaviorServices = new List<BehaviorService>();
            foreach(System.Func<BehaviorService> getService in _Services)
            {
                // 서비스 객체 생성
                BehaviorService runService = getService.Invoke();

                // 서비스 객체 시작
                runService.OnServiceStarted(behaviorController);

                // 실행시킬 서비스 객체 등록
                behaviorServices.Add(runService);

            }
            // 서비스 실행
            _ServiceRoutine = behaviorController.StartCoroutine(ServiceRoutine());

        }
        


        return true;
    }

    public virtual IEnumerator ServiceRoutine()
    {
        while(behaviorServices != null)
        {
            foreach(BehaviorService service in behaviorServices)
            {
                service.ServiceTick();
            }

            yield return null;
        }
    }


    /// <summary>
    /// 행동이 시작되었을 때 호출되는 함수입니다.
    /// 동작 방식을 정의합니다.
    /// </summary>
    /// <returns></returns>
    public abstract IEnumerator OnBehaivorStarted();

    public virtual void OnBehaviorFinished()
    {
        // 하위 노드 종료처리
        childBehavior?.OnBehaviorFinished();

        // 서비스를 실행하고 있는 경우
        if(_ServiceRoutine != null)
        {
            // 서비스 루틴 종료
            behaviorController.StopCoroutine(_ServiceRoutine);
            _ServiceRoutine = null;

            // 서비스 객체 종료처리
            foreach(BehaviorService service in behaviorServices)
                service.OnServiceFinished();

            // 서비스 객체들 비우기
            behaviorServices.Clear();

        }

    }

    public void AddService<TBehaviorService>() where
        TBehaviorService : BehaviorService, new()
        => AddService(() => new TBehaviorService());

    public void AddService(System.Func<BehaviorService> fngetService)
        => _Services.Add(fngetService);

#if UNITY_EDITOR
    public virtual void OnDrawGizmos()
    {

        if(behaviorServices != null)
        {
            foreach(BehaviorService service in behaviorServices)
            {
                service.OnDrawGizmos();
            }
        }

        childBehavior?.OnDrawGizmos();
    }
#endif
}
