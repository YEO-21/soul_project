using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 애니메이터를 제어하기 위한 컴포넌트입니다.
/// </summary>
public abstract class AnimController : MonoBehaviour
{
    /// <summary>
    /// 이 컴포넌트가 관리하게 될 Animator 컴포넌트
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
