using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Wait : RunnableBehavior
{
    /// <summary>
    /// ����� �ð��� ��Ÿ���ϴ�.
    /// </summary>
    private float _WaitSeconds;

    public BT_Wait(float waitSeconds)
    {
        _WaitSeconds = waitSeconds;
    }

    public override IEnumerator OnBehaviorStarted()
    {
        yield return new WaitForSeconds(_WaitSeconds);
        isSucceeded = true;
    }
}
