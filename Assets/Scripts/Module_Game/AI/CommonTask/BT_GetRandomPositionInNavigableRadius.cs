using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 지정한 반경 내에서 이동 가능한 랜덤한 위치를 뽑습니다.
/// </summary>
public class BT_GetRandomPositionInNavigableRadius : RunnableBehavior
{
    /// <summary>
    /// 랜덤한 위치를 뽑아 저장시킬 키 이름을 나타냅니다.
    /// </summary>
    private string _ResultKey;

    /// <summary>
    /// 이동 가능 반경에 대한 키 이름을 나타냅니다.
    /// </summary>
    private string _MaxMoveDistanceKey;

    /// <summary>
    /// 기준 위치 나타내는 키
    /// </summary>
    private string _OriginPositionKey;
    


    public BT_GetRandomPositionInNavigableRadius(
        string resultKey,
        string maxMoveDistanceKey,
        string originPositionKey)
    {
        _ResultKey = resultKey;
        _MaxMoveDistanceKey = maxMoveDistanceKey;
        _OriginPositionKey = originPositionKey;
    }

    public override IEnumerator OnBehaviorStarted()
    {
        // 최대 이동 거리를 얻습니다.
        float maxMoveDistance = behaviorController.GetKey<float>(_MaxMoveDistanceKey);

        // 랜덤한 거리
        float distance = Random.Range(0.0f, maxMoveDistance);

        // 랜덤한 방향
        Vector3 direction = Random.insideUnitSphere;
        // insideUnitSphere : 반지름이 1인 구 내에서 랜더함 위치를 뽑아 반환합니다.
        direction.y = 0.0f;
        direction.Normalize();

        // 기준 위치를 얻습니다.
        Vector3 originPosition = behaviorController.GetKey<Vector3>(_OriginPositionKey);

        // 다음 위치를 설정합니다.
        behaviorController.SetKey(_ResultKey, originPosition + (direction * distance));

        // 행동 성공
        isSucceeded = true;

        yield return null;
    }
}
