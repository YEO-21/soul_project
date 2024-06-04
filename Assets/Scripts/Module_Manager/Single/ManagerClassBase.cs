using UnityEngine;

/// <summary>
/// ���� ��ü�� GameManager �� ����Ʈ�� ��� ���Ͽ� ���Ǵ� ����
/// </summary>
public interface IManagerClass
{ 
    /// <summary>
    /// ���� ��ü�� �ʱ�ȭ�� �� ȣ��Ǵ� �޼����Դϴ�.
    /// </summary>
    void OnManagerInitialized();

}

/// <summary>
/// ���� ��ü�� ��� ����
/// </summary>
/// <typeparam name="T"></typeparam>
public class ManagerClassBase<T> : MonoBehaviour, IManagerClass
     where T : MonoBehaviour, IManagerClass
{
    private static T _ThisInstance;

    public static T instance => _ThisInstance;

    public virtual void OnManagerInitialized() {}
}
