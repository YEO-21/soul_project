using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ظ� ���� �� �ִ� ��ü���� �����ϴ� �������̽��Դϴ�.
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// ���ظ� �Ծ��� ��� ȣ��˴ϴ�.
    /// </summary>
    /// <param name="damageInstance"></param>
    void OnHit(DamageBase damageInstance);


}
