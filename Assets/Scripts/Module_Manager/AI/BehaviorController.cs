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
   

    /// <summary>
    /// 행동 루틴을 나타냅니다.
    /// </summary>
    private Coroutine _BehaviorRoutine;

    /// <summary>
    /// BehaviorController 에서 실행중인 루트 노드를 나타냅니다.
    /// </summary>
    private RunnableBehavior _RootRunnable;

    /// <summary>
    /// 요청된 행동 재시작 시간을 나타냅니다.
    /// </summary>
    private float _BehaviorRestartRequestTime;

    /// <summary>
    /// 재시작 요청이 있음을 나타냅니다.
    /// </summary>
    private bool _BehaviorRestartRequested;

    /// <summary>
    /// 행동 객체에서 사용하게 될 데이터를 나타냅니다.
    /// </summary>
    protected Dictionary<string, object> m_Keys = new();

    /// <summary>
    /// 감각 객체들을 나타냅니다.
    /// </summary>
    protected List<SenseBase> m_Senses = new();

    /// <summary>
/// m_Keys에 대한 읽기 전용 프로퍼티 입니다.
/// </summary>
    public Dictionary<string, object> keys => m_Keys;



    protected virtual void Update()
    {
        SenseTick();

        // 재시작 요청이 존재한다면
        if (_BehaviorRestartRequested)
        {
            if(_BehaviorRestartRequestTime <= Time.time) 
            {
                // 요청 처리됨
                _BehaviorRestartRequested = false;

                // 행동 재시작
                OnBehaviorRestarted();
            }
        }
    }

    /// <summary>
    /// 감각 객체 Tick
    /// </summary>
    protected virtual void SenseTick()
    {
        foreach(SenseBase senseInstance in m_Senses)
        {
            senseInstance.OnSenseUpdated();
        }
    }

    /// <summary>
    /// 행동이 재시작 될 때 호출됩니다.
    /// </summary>
    protected virtual void OnBehaviorRestarted()
    {

    }

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
        // 행동 종료
        if (_RootRunnable != null)
        {
            _RootRunnable.OnBehaviorFinished();
            _RootRunnable = null;
        }

        // 실행중인 루틴이 존재한다면
        if (_BehaviorRoutine !=null)
        {
            // 루틴 중단
            StopCoroutine(_BehaviorRoutine);
            _BehaviorRoutine = null;

        }
    }

    /// <summary>
    /// 행동 재시작을 요청합니다.
    /// 진행중인 행동은 즉각적으로 중단되며, 강제 재시작이 아닌 경우
    /// 요청된 시간중 가장 늦은 시간에 행동이 재시작됩니다.
    /// </summary>
    /// <param name="startDelay"></param>
    /// <param name="forceRestart"></param>
    public virtual void BehaviorStartRequest(
        float startDelay = 0.0f, bool forceRestart = false)
    {
        Debug.Log("행동 재시작 요청됨");


        // 행동 중단
        StopBehavior();

        // 재시작 요청된 시간을 계산합니다.
        float requestTime = Time.time + startDelay;

        if(forceRestart)
        {
            _BehaviorRestartRequestTime = requestTime;
           
        }

        // 현재 요청이 이전에 실행한 시간보다 빠른 시점일 경우 요청 무시
        else if(_BehaviorRestartRequestTime > requestTime) return;

        _BehaviorRestartRequestTime = requestTime;
        _BehaviorRestartRequested = true;


    }

    /// <summary>
    /// Key를 추가하거나, 설정합니다.
    /// </summary>
    /// <param name="keyName"></param>
    /// <param name="value"></param>
    public void SetKey(string keyName, object value = null)
       => m_Keys[keyName] = value;

    public T GetKey<T>(string keyName) => (T)GetKey(keyName);
    

    public object GetKey(string keyName)
    {
        if(m_Keys.TryGetValue(keyName, out object value))
        {
            return value;
        }

#if UNITY_EDITOR
        Debug.LogError($"[{keyName}] 에 대한 내용을 찾을 수 없습니다.");
#endif
        return null;
    }

    /// <summary>
    /// 감각 객체를 등록합니다.
    /// </summary>
    /// <typeparam name="TSense">감각 객체 형식을 전달합니다.</typeparam>
    /// <returns></returns>
    public TSense RegisterSense<TSense>()
        where TSense : SenseBase, new()
    {
        // 감지 객체 생성
        TSense senseInstance = new TSense();

        // 감각 객체 초기화
        senseInstance.OnSenseIntialized(this);

        // 감각 객체 추가
        m_Senses.Add(senseInstance);

        return senseInstance;

    }


    private IEnumerator Run<TRunnableBehavior>()
        where TRunnableBehavior : RunnableBehavior, new()
    {
        // 행동들을 계속 실행시킵니다.
        while(true)
        {

            // 행동 객체를 생성합니다.
            _RootRunnable = new TRunnableBehavior();

            // 행동 객체 초기화 성공 시
            if (_RootRunnable.OnInitialized(this))
            {
                // 행동을 시작시키고 행동이 끝날 때까지 대기합니다.
                
                yield return _RootRunnable.OnBehaivorStarted();

            }
            else yield return null;

            _RootRunnable = null;
        }
    }


    protected virtual void OnDestroy() => StopBehavior();

#if UNITY_EDITOR
    protected virtual void OnDrawGizmos()
    {
        foreach(SenseBase senseInstance in m_Senses)
        {
            senseInstance.OnDrawGizmos();
        }

        _RootRunnable?.OnDrawGizmos();
    }
#endif


}
