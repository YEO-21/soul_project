using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
///  ������ ��ġ�� �̵��մϴ�.
/// </summary>
public class BT_MoveTo : RunnableBehavior
{
    /// <summary>
    /// ��ǥ ��ġ Ű
    /// </summary>
    private string _TargetPositionKey;



    public BT_MoveTo(string targetPositionKey)
    {
        _TargetPositionKey = targetPositionKey;
    }



    public override IEnumerator OnBehaivorStarted()
    {
        // ��ã�� ����� �����ϴ� ������Ʈ�� ����ϴ�.
        NavMeshAgent agent = (behaviorController as EnemyBehaviorController).agent;

        // ��ǥ ��ġ�� ����ϴ�.
        Vector3 targetPosition = behaviorController.GetKey<Vector3>(_TargetPositionKey);

        // ��ǥ ��ġ�� �̵���ŵ�ϴ�.
        agent.SetDestination(targetPosition);

        // ������ ��ǥ ��ġ�� ����ϴ�.
        Vector3 destination = agent.destination;

        yield return new WaitUntil(() => 
        Vector3.Distance(behaviorController.transform.position , destination) < 0.1f);

        isSucceeded = true;

    }
}
