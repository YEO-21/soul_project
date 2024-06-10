using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerBase : MonoBehaviour
{
    /// <summary>
    /// �� ������ ��ü�� ��Ÿ���ϴ�.
    /// </summary>
    private static GameManagerBase _ThisInstance;

    /// <summary>
    /// GameManager ������ �����ϴ� ���� ��ü���� ��Ÿ���ϴ�.
    /// </summary>
    private List<IManagerClass> _Managers = new();

    public static GameManagerBase Get() => Get<GameManagerBase>();


    public static T Get<T>() where T : GameManagerBase
    {
        if (!_ThisInstance)
        {
            // ���忡�� GameManagerBase ������ ��ü(������Ʈ)�� ã���ϴ�.
            _ThisInstance = FindObjectOfType<GameManagerBase>();

            // GameManager ��ü �ʱ�ȭ
            _ThisInstance.OnGameManagerInitialized();

        }

        // GameManager ��ü�� ��ȯ�մϴ�.
        return _ThisInstance as T;
    }

    protected virtual void Awake()
    {
        GameManagerBase gameManager = Get();

        if (gameManager != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// ���� ��ü�� ã���ϴ�.
    /// </summary>
    /// <typeparam name="T">ã���� �ϴ� ���� ��ü ������ �����մϴ�.</typeparam>
    /// <returns>T ������ ���� ��ü�� ��ȯ�մϴ�.</returns>
    public T GetManager<T>() where T : MonoBehaviour, IManagerClass
        => _Managers.Find((IManagerClass type) => type.GetType() == typeof(T)) as T;

    /// <summary>
    /// ���� ��ü�� ����մϴ�.
    /// </summary>
    /// <typeparam name="T">����ϰ��� �ϴ� ���� ��ü�� ������ �����մϴ�.</typeparam>
    protected void RegisterManager<T>() where T : MonoBehaviour, IManagerClass
    {
        // GameManager ��ü �������� T ������ ���� ��ü ������Ʈ�� ã���ϴ�.
        T manager = GetComponentInChildren<T>();

        // ã�� ���� ���
        if (!manager)
        {
            // �� ������Ʈ�� �����ϰ�, GameManager ���� ������Ʈ�� �����մϴ�.
            GameObject newManagerObject = new GameObject(typeof(T).Name);
            newManagerObject.transform.SetParent(transform);

            // T ������ ���� ��ü ������Ʈ�� �߰��մϴ�.
            manager = newManagerObject.AddComponent<T>();
        }

        // ���� ��ü�� ����Ʈ�� �߰��մϴ�.
        _Managers.Add(manager);

        // ���� ��ü �ʱ�ȭ
        manager.OnManagerInitialized();
    }

    /// <summary>
    /// GameManager ��ü �ʱ�ȭ
    /// </summary>
    protected virtual void OnGameManagerInitialized()
    {
        RegisterManager<SceneManagerBase>();

    }

}
