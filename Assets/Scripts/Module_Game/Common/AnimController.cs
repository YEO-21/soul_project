using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ִϸ����͸� �����ϱ� ���� ������Ʈ�Դϴ�.
/// </summary>
public abstract class AnimController : MonoBehaviour
{
    /// <summary>
    /// �� ������Ʈ�� �����ϰ� �� Animator ������Ʈ
    /// </summary>
    private Animator _Animator;

    public Animator animator => _Animator ?? (_Animator = GetComponent<Animator>());

    protected void SetParam(string paramName, bool value)
        => animator.SetBool(paramName, value);
    protected void SetParam(string paramName, float value)
        => animator.SetFloat(paramName, value);
    protected void SetParam(string paramName, int value)
        => animator.SetInteger(paramName, value);
    protected void SetParam(string paramName)
        => animator.SetTrigger(paramName);
}
