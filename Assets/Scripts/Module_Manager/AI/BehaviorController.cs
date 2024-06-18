using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 등록된 행동들을 관리할 객체입니다.
/// 기본적으로 Task 와 Composite 을 이용하여 행동을 정의할 수 있습니다.
/// Task : 하나의 행동(작업)을 나타냅니다.
/// Composite : 분기가 실행되는 방식의 기본 규칙을 정의합니다.
/// Service : Task 와 Composite 에 추가되어 해당 분기가 
/// 실행중일 때 동시에 실행되며 특정한 데이터를 제공합니다.
/// 
/// BehaviorController : 하나의 행동(Root) 와 연결되며 
/// 행동을 제어하는 기본적인 기능을 제공합니다.
/// </summary>
public class BehaviorController : MonoBehaviour
{
    private void Start()
    {
        StartBehaivor<BehaviorSequencer>();
    }


    /// <summary>
    /// 행동 루틴을 나타냅니다.
    /// </summary>
    private Coroutine _BehaviorRoutine;


    /// <summary>
    /// 행동을 실행시킵니다.
    /// </summary>
    /// <typeparam name="TRunnableBehavior">실행시킬 행동 형식을 전달합니다.</typeparam>
    public void StartBehaivor<TRunnableBehavior>()
        where TRunnableBehavior : RunnableBehavior, new()
    {
        _BehaviorRoutine = StartCoroutine(Run<TRunnableBehavior>());
    }

    /// <summary>
    /// 행동을 중단합니다.
    /// </summary>
    public void StopBehavior()
    {
        // 실행중인 루틴이 존재한다면
        if(_BehaviorRoutine !=null)
        {
            // 루틴 중단
            StopCoroutine(_BehaviorRoutine);
            _BehaviorRoutine = null;

        }
    }

    private IEnumerator Run<TRunnableBehavior>()
        where TRunnableBehavior : RunnableBehavior, new()
    {
        // 행동들을 계속 실행시킵니다.
        while(true)
        {
            // 행동 객체를 생성합니다.
            TRunnableBehavior root = new TRunnableBehavior();


            // 행동을 시작시키고 행동이 끝날 때까지 대기합니다.
            yield return root.OnBehaivorStarted();
        }
    }


    protected virtual void OnDestroy() => StopBehavior();


}
