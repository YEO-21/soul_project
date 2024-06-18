using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagerBase : ManagerClassBase<SceneManagerBase>
{
    /// <summary>
    /// 현재 씬에서 사용중인 씬 객체를 나타냅니다.
    /// 씬이 변경될 때마다 사용중인 씬의 씬 객체를 참조합니다.
    /// </summary>
    public SceneInstance sceneInstance { get; private set; }

    public override void OnManagerInitialized()
    {
        base.OnManagerInitialized();

        // 씬이 교체될 때마다 발생하는 이벤트를 등록합니다.
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged += CALLBACK_OnSceneChanged;
    }

    /// <summary>
    /// 씬이 변경될 때마다 호출되는 콜백
    /// </summary>
    /// <param name="prevScene"></param>
    /// <param name="nextScene"></param>
    private void CALLBACK_OnSceneChanged(
        UnityEngine.SceneManagement.Scene prevScene,
        UnityEngine.SceneManagement.Scene nextScene)
    {
        // 씬이 로드 완료된 경우
        if (nextScene.isLoaded)
        {
            sceneInstance = GetSceneInstance<SceneInstance>();
        }
    }

    /// <summary>
    /// T 형식의 씬 객체를 얻습니다.
    /// </summary>
    /// <typeparam name="T">얻을 씬 객체 형식을 전달합니다.</typeparam>
    /// <returns>T 형식의 씬 객체를 반환합니다.</returns>
    public T GetSceneInstance<T>() where T : SceneInstance
        => (FindObjectOfType<SceneInstance>() ??
            new GameObject("Single_DefaultSceneInstance").AddComponent<SceneInstance>()) as T;
}
