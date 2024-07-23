using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerBase : MonoBehaviour
{
    /// <summary>
    /// 이 형태의 객체를 나타냅니다.
    /// </summary>
    private static GameManagerBase _ThisInstance;

    /// <summary>
    /// GameManager 하위에 존재하는 관리 객체들을 나타냅니다.
    /// </summary>
    private List<IManagerClass> _Managers = new();

    public static GameManagerBase Get() => Get<GameManagerBase>();


    public static T Get<T>() where T : GameManagerBase
    {
        if (!_ThisInstance)
        {
            // 월드에서 GameManagerBase 형태의 객체(컴포넌트)를 찾습니다.
            _ThisInstance = FindObjectOfType<GameManagerBase>();

            // GameManager 객체 초기화
            _ThisInstance.OnGameManagerInitialized();

        }

        // GameManager 객체를 반환합니다.
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
    /// 관리 객체를 찾습니다.
    /// </summary>
    /// <typeparam name="T">찾고자 하는 관리 객체 형식을 전달합니다.</typeparam>
    /// <returns>T 형식의 관리 객체를 반환합니다.</returns>
    public T GetManager<T>() where T : MonoBehaviour, IManagerClass
        => _Managers.Find((IManagerClass type) => type.GetType() == typeof(T)) as T;

    /// <summary>
    /// 관리 객체를 등록합니다.
    /// </summary>
    /// <typeparam name="T">등록하고자 하는 관리 객체의 형식을 전달합니다.</typeparam>
    protected void RegisterManager<T>() where T : MonoBehaviour, IManagerClass
    {
        // GameManager 객체 하위에서 T 형식의 관리 객체 컴포넌트를 찾습니다.
        T manager = GetComponentInChildren<T>();

        // 찾지 못한 경우
        if (!manager)
        {
            // 빈 오브젝트를 생성하고, GameManager 하위 오브젝트로 설정합니다.
            GameObject newManagerObject = new GameObject(typeof(T).Name);
            newManagerObject.transform.SetParent(transform);

            // T 형식의 관리 객체 컴포넌트를 추가합니다.
            manager = newManagerObject.AddComponent<T>();
        }

        // 관리 객체를 리스트에 추가합니다.
        _Managers.Add(manager);

        // 관리 객체 초기화
        manager.OnManagerInitialized();
    }

    /// <summary>
    /// GameManager 객체 초기화
    /// </summary>
    protected virtual void OnGameManagerInitialized()
    {
        RegisterManager<SceneManagerBase>();

    }

}
