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

    /// <summary>
    /// �̵��� ���������� �ൿ�� ����ŵ�ϴ�.
    /// </summary>
    private bool _WaitMoveFinish;



    public BT_MoveTo(string targetPositionKey, bool waitMoveFinish = true)
    {
        _TargetPositionKey = targetPositionKey;
        _WaitMoveFinish = waitMoveFinish;
    }



    public override IEnumerator OnBehaivorStarted()
    {

        // ��ã�� ����� �����ϴ� ������Ʈ�� ����ϴ�.
        NavMeshAgent agent = (behaviorController as EnemyBehaviorController).agent;

        agent.enabled = true;

        // ��ǥ ��ġ�� ����ϴ�.
        Vector3 targetPosition = behaviorController.GetKey<Vector3>(_TargetPositionKey);

        // ��ǥ ��ġ�� �̵���ŵ�ϴ�.
        agent.SetDestination(targetPosition);

        // ������ ��ǥ ��ġ�� ����ϴ�.
        Vector3 destination = agent.destination;

        if(_WaitMoveFinish)
        {
            yield return new WaitUntil(() =>
             Vector3.Distance(behaviorController.transform.position, destination) <= agent.stoppingDistance);
        }


        isSucceeded = true;

    }
}
