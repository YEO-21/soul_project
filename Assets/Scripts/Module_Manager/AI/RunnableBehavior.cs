

using System.Collections;
/// <summary>
/// Behavior Controller 객체를 통해 실행될 수 있는 객체의 최상위 형태
/// </summary>
public abstract class RunnableBehavior
{
    /// <summary>
    /// 이 행동의 실행 성공 여부를 나타냅니다.
    /// </summary>
    public bool isSucceeded { get; protected set; }


    /// <summary>
    /// 행동이 시작되었을 때 호출되는 함수입니다.
    /// 동작 방식을 정의합니다.
    /// </summary>
    /// <returns></returns>
    public abstract IEnumerator OnBehaivorStarted();


}
