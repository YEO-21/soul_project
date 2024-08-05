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
        // �̹� �ε�� ��� ��ƾ�� �����մϴ�.
        if(_IsLoaded) yield break;

        // �ε� / ��ε� ���°� ���������� ����մϴ�.
        yield return new WaitWhile(() => _IsLoadingOrUnloading);

        _IsLoadingOrUnloading = true;

        // �� �񵿱� �ε带 �����մϴ�.
        AsyncOperation ao = SceneManager.LoadSceneAsync(m_LoadSceneName, LoadSceneMode.Additive);

        // �񵿱� �ε� �۾��� ���� ������ ����մϴ�.
        yield return new WaitUntil(() => ao.isDone);

        // �ε� / ��ε� �۾� ��
        _IsLoadingOrUnloading = false;

        // �ε�� ���·� ����
        _IsLoaded = true;



    }

    private IEnumerator UnloadAsync()
    {
        // �̹� ��ε�� ��� ��ƾ�� �����մϴ�.
        if (!_IsLoaded) yield break;

        // �ε� / ��ε� ���°� ���������� ����մϴ�.
        yield return new WaitWhile(() => _IsLoadingOrUnloading);

        _IsLoadingOrUnloading = true;


        // �� ��ε带 �����մϴ�.
        AsyncOperation ao = SceneManager.UnloadSceneAsync(m_LoadSceneName);

        // �񵿱� ��ε� �۾��� ���� ������ ����մϴ�.
        yield return new WaitUntil(() => ao.isDone);

        // �ε� / ��ε� �۾� ��
        _IsLoadingOrUnloading = false;

        // ��ε� ���·� ����
        _IsLoaded = false;
    }

}
