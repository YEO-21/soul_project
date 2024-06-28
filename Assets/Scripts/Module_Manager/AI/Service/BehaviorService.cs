using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 서비스를 나타내기 위한 클래스
/// </summary>
public abstract class BehaviorService
{
    public BehaviorController behaviorController { get; private set; }

    public virtual void OnServiceStarted(BehaviorController behaviorController)
    {
        this.behaviorController = behaviorController;
    }

    public abstract void ServiceTick();

    public virtual void OnServiceFinished()
    {

    }
}
