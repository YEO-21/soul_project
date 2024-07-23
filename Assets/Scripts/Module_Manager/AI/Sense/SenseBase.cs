using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ��ü�� ���� ��� Ŭ�����Դϴ�.
/// </summary>
public abstract class SenseBase
{
    /// <summary>
    /// �ൿ ���� ��ü�Դϴ�.
    /// </summary>
    public BehaviorController behaviorController { get; private set; }

    /// <summary>
    /// ���� ��ü �ʱ�ȭ��
    /// </summary>
    /// <param name="behaviorController"></param>
    public virtual void OnSenseInitialized(BehaviorController behaviorController)
    {
        this.behaviorController = behaviorController;
    }

    /// <summary>
    /// ���� ��ü Update ��ƾ
    /// </summary>
    public virtual void OnSenseUpdated() { }

    public virtual void OnDrawGizmos()
    {

    }
}
