using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���񽺸� ��Ÿ���� ���� Ŭ����
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
