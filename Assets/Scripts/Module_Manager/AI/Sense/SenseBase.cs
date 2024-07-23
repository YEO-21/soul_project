using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 감각 객체를 위한 기반 클래스입니다.
/// </summary>
public abstract class SenseBase
{
    /// <summary>
    /// 행동 제어 객체입니다.
    /// </summary>
    public BehaviorController behaviorController { get; private set; }

    /// <summary>
    /// 감각 객체 초기화됨
    /// </summary>
    /// <param name="behaviorController"></param>
    public virtual void OnSenseInitialized(BehaviorController behaviorController)
    {
        this.behaviorController = behaviorController;
    }

    /// <summary>
    /// 감각 객체 Update 루틴
    /// </summary>
    public virtual void OnSenseUpdated() { }

    public virtual void OnDrawGizmos()
    {

    }
}
