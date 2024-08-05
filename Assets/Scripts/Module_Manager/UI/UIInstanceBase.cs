using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI��ü �ϳ��� ��Ÿ���� ���� Ŭ�����Դϴ�.
/// </summary>
public abstract class UIInstanceBase : MonoBehaviour
{
    /// <summary>
    /// UI ��ü�� �ʱ�ȭ�մϴ�.
    /// </summary>
    /// <param name="playerController"></param>
    public abstract void InitializeUI(PlayerControllerBase playerController);
}
