using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �� ��ü�� ��Ÿ���� ���� Ŭ�����Դϴ�.
/// </summary>
public abstract class EnemyCharacterBase : MonoBehaviour
{
    /// <summary>
    /// �� ĳ���Ͱ� ���ظ� ���� ��� ȣ��Ǵ� �޼����Դϴ�.
    /// </summary>
    /// <param name="damage"></param>
    public virtual void OnHit(DamageBase damage)
    {
        Debug.Log("damage = " + damage);
    }



}
