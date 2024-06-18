

using System.Collections;
/// <summary>
/// Behavior Controller ��ü�� ���� ����� �� �ִ� ��ü�� �ֻ��� ����
/// </summary>
public abstract class RunnableBehavior
{
    /// <summary>
    /// �� �ൿ�� ���� ���� ���θ� ��Ÿ���ϴ�.
    /// </summary>
    public bool isSucceeded { get; protected set; }


    /// <summary>
    /// �ൿ�� ���۵Ǿ��� �� ȣ��Ǵ� �Լ��Դϴ�.
    /// ���� ����� �����մϴ�.
    /// </summary>
    /// <returns></returns>
    public abstract IEnumerator OnBehaivorStarted();


}
