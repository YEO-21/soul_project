using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ �ݰ� ������ �̵� ������ ������ ��ġ�� �̽��ϴ�.
/// </summary>
public class BT_GetRandomPositionInNavigableRadius : RunnableBehavior
{
    /// <summary>
    /// ������ ��ġ�� �̾� �����ų Ű �̸��� ��Ÿ���ϴ�.
    /// </summary>
    private string _ResultKey;

    /// <summary>
    /// �̵� ���� �ݰ濡 ���� Ű �̸��� ��Ÿ���ϴ�.
    /// </summary>
    private string _MaxMoveDistanceKey;

    /// <summary>
    /// ���� ��ġ ��Ÿ���� Ű
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
        // �ִ� �̵� �Ÿ��� ����ϴ�.
        float maxMoveDistance = behaviorController.GetKey<float>(_MaxMoveDistanceKey);

        // ������ �Ÿ�
        float distance = Random.Range(0.0f, maxMoveDistance);

        // ������ ����
        Vector3 direction = Random.insideUnitSphere;
        // insideUnitSphere : �������� 1�� �� ������ ������ ��ġ�� �̾� ��ȯ�մϴ�.
        direction.y = 0.0f;
        direction.Normalize();

        // ���� ��ġ�� ����ϴ�.
        Vector3 originPosition = behaviorController.GetKey<Vector3>(_OriginPositionKey);

        // ���� ��ġ�� �����մϴ�.
        behaviorController.SetKey(_ResultKey, originPosition + (direction * distance));

        // �ൿ ����
        isSucceeded = true;

        yield return null;
    }
}
