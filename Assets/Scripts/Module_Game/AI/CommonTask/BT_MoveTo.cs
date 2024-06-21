using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
///  지정한 위치로 이동합니다.
/// </summary>
public class BT_MoveTo : RunnableBehavior
{
    /// <summary>
    /// 목표 위치 키
    /// </summary>
    private string _TargetPositionKey;



    public BT_MoveTo(string targetPositionKey)
    {
        _TargetPositionKey = targetPositionKey;
    }



    public override IEnumerator OnBehaivorStarted()
    {
        // 길찾기 기능을 제공하는 컴포넌트를 얻습니다.
        NavMeshAgent agent = (behaviorController as EnemyBehaviorController).agent;

        // 목표 위치를 얻습니다.
        Vector3 targetPosition = behaviorController.GetKey<Vector3>(_TargetPositionKey);

        // 목표 위치로 이동시킵니다.
        agent.SetDestination(targetPosition);

        // 설정된 목표 위치를 얻습니다.
        Vector3 destination = agent.destination;

        yield return new WaitUntil(() => 
        Vector3.Distance(behaviorController.transform.position , destination) < 0.1f);

        isSucceeded = true;

    }
}
