using UnityEngine;

/// <summary>
/// 관리 객체를 GameManager 의 리스트에 담기 위하여 사용되는 형식
/// </summary>
public interface IManagerClass
{ 
    /// <summary>
    /// 관리 객체가 초기화될 때 호출되는 메서드입니다.
    /// </summary>
    void OnManagerInitialized();

}

/// <summary>
/// 관리 객체의 기반 형식
/// </summary>
/// <typeparam name="T"></typeparam>
public class ManagerClassBase<T> : MonoBehaviour, IManagerClass
     where T : MonoBehaviour, IManagerClass
{
    private static T _ThisInstance;

    public static T instance => _ThisInstance;

    public virtual void OnManagerInitialized() {}
}
