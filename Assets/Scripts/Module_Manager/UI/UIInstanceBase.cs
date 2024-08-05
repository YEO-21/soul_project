using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI객체 하나를 나타내기 위한 클래스입니다.
/// </summary>
public abstract class UIInstanceBase : MonoBehaviour
{
    /// <summary>
    /// UI 객체를 초기화합니다.
    /// </summary>
    /// <param name="playerController"></param>
    public abstract void InitializeUI(PlayerControllerBase playerController);
}
