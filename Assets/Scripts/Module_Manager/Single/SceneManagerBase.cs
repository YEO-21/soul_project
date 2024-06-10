using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagerBase : ManagerClassBase<SceneManagerBase>
{
    /// <summary>
    /// ���� ������ ������� �� ��ü�� ��Ÿ���ϴ�.
    /// ���� ����� ������ ������� ���� �� ��ü�� �����մϴ�.
    /// </summary>
    public SceneInstance sceneInstance { get; private set; }

    public override void OnManagerInitialized()
    {
        base.OnManagerInitialized();

        // ���� ��ü�� ������ �߻��ϴ� �̺�Ʈ�� ����մϴ�.
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged += CALLBACK_OnSceneChanged;
    }

    /// <summary>
    /// ���� ����� ������ ȣ��Ǵ� �ݹ�
    /// </summary>
    /// <param name="prevScene"></param>
    /// <param name="nextScene"></param>
    private void CALLBACK_OnSceneChanged(
        UnityEngine.SceneManagement.Scene prevScene,
        UnityEngine.SceneManagement.Scene nextScene)
    {
        // ���� �ε� �Ϸ�� ���
        if (nextScene.isLoaded)
        {
            sceneInstance = GetSceneInstance<SceneInstance>();
        }
    }

    /// <summary>
    /// T ������ �� ��ü�� ����ϴ�.
    /// </summary>
    /// <typeparam name="T">���� �� ��ü ������ �����մϴ�.</typeparam>
    /// <returns>T ������ �� ��ü�� ��ȯ�մϴ�.</returns>
    public T GetSceneInstance<T>() where T : SceneInstance
        => (FindObjectOfType<SceneInstance>() ??
            new GameObject("Single_DefaultSceneInstance").AddComponent<SceneInstance>()) as T;
}
