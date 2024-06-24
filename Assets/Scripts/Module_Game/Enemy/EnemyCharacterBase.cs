using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �� ��ü�� ��Ÿ���� ���� Ŭ�����Դϴ�.
/// </summary>
public abstract class EnemyCharacterBase : MonoBehaviour,
    IDamageable

{
    /// <summary>
    /// �� ĳ���Ͱ� ���ظ� ���� ��� ȣ��Ǵ� �޼����Դϴ�.
    /// </summary>
    /// <param name="damage"></param>
    public virtual void OnHit(DamageBase damageInstance)
    {
        Debug.Log("���ط� : " + damageInstance.damage);
        Debug.Log("������ : " + damageInstance.from.gameObject.name);
    }



}
