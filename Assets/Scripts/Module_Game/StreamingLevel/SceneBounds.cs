using GameModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Collider))]
public class SceneBounds : MonoBehaviour
{
    public string m_LoadSceneName;

    private Collider _Bound;

    private bool _IsLoaded;
    private bool _IsLoadingOrUnloading;


    private void Awake()
    {
        _Bound = GetComponent<Collider>();
        _Bound.isTrigger = true;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.TAG_PLAYER))
            StartCoroutine(LoadAsync());
    }    

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constants.TAG_PLAYER))
            StartCoroutine(UnloadAsync());
    }

    private IEnumerator LoadAsync()
    {
        // 이미 로드된 경우 루틴을 종료합니다.
        if(_IsLoaded) yield break;

        // 로딩 / 언로딩 상태가 끝날때까지 대기합니다.
        yield return new WaitWhile(() => _IsLoadingOrUnloading);

        _IsLoadingOrUnloading = true;

        // 씬 비동기 로드를 시작합니다.
        AsyncOperation ao = SceneManager.LoadSceneAsync(m_LoadSceneName, LoadSceneMode.Additive);

        // 비동기 로드 작업이 끝날 때까지 대기합니다.
        yield return new WaitUntil(() => ao.isDone);

        // 로딩 / 언로딩 작업 끝
        _IsLoadingOrUnloading = false;

        // 로드됨 상태로 설정
        _IsLoaded = true;



    }

    private IEnumerator UnloadAsync()
    {
        // 이미 언로드된 경우 루틴을 종료합니다.
        if (!_IsLoaded) yield break;

        // 로딩 / 언로딩 상태가 끝날때까지 대기합니다.
        yield return new WaitWhile(() => _IsLoadingOrUnloading);

        _IsLoadingOrUnloading = true;


        // 씬 언로드를 진행합니다.
        AsyncOperation ao = SceneManager.UnloadSceneAsync(m_LoadSceneName);

        // 비동기 언로드 작업이 끝날 때까지 대기합니다.
        yield return new WaitUntil(() => ao.isDone);

        // 로딩 / 언로딩 작업 끝
        _IsLoadingOrUnloading = false;

        // 언로드 상태로 설정
        _IsLoaded = false;
    }

}
