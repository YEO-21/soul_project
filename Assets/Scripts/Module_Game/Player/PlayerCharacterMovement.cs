using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public sealed class PlayerCharacterMovement : MonoBehaviour
{
    [Header("# 캐릭터 정보 (임시)")]
    public float m_GravityMultiplier = 0.1f;

    /// <summary>
    /// 걷기 속력
    /// </summary>
    public float m_WalkSpeed = 6.0f;

    /// <summary>
    /// 점프 힘
    /// </summary>
    public float m_JumpPower = 30.0f;

    /// <summary>
    /// 이동 입력 상태를 나타냅니다.
    /// </summary>
    private bool _IsMovementInput;

    /// <summary>
    /// 이동 입력 축 값을 기록하기 위한 변수
    /// </summary>
    private Vector2 _MovementInputAxis;


    /// <summary>
    /// 캐릭터에 적용될 수직 속도를 나타냅니다.
    /// </summary>
    private Vector3 _VerticalVelocity;

    /// <summary>
    /// 점프 요청됨을 나타내기 위한 변수
    /// </summary>
    private bool _JumpRequested;

    /// <summary>
    /// CharacterController 컴포넌트를 나타냅니다.
    /// </summary>
    private CharacterController _CharacterController;

    /// <summary>
    /// CharacterController 컴포넌트에 대한 프로퍼티입니다.
    /// </summary>
    public CharacterController characterController => _CharacterController ??
        (_CharacterController = GetComponent<CharacterController>());


    /// <summary>
    /// 땅 감지 상태를 나타내는 프로퍼티입니다.
    /// 내부적으로 CharacterController 의 isGrounded 프로퍼티를 사용합니다.
    /// </summary>
    public bool isGrounded { get; private set; }

    /// <summary>
    /// 캐릭터에 적용될 중력에 대한 읽기 전용 프로퍼티입니다.
    /// </summary>
    public Vector3 gravity => Physics.gravity * m_GravityMultiplier;

    #region DEBUG
    private DrawGizmoLineInfo _DrawLineInfo;
    #endregion


    public void Update()
    {
        // 이동 처리
        Movement();
        
    }


    /// <summary>
    /// 이동과 관련된 연산을 모두 이곳에서 진행합니다.
    /// </summary>
    private void Movement()
    {
        // 이동 입력을 월드 기준 방향으로 변환합니다.
        Vector3 worldDirection = InputToWorldDirection();

        // 땅에 닿음 상태 갱신
        isGrounded = characterController.isGrounded;

        // 바닥에 대한 정보를 얻습니다.
        TryGetFloorInfo();

        // 점프 / 중력을 계산합니다.
        CalculateJumpAndGravity();

        // 이동
        characterController.Move(
            (worldDirection * m_WalkSpeed *Time.deltaTime) + 
            (_VerticalVelocity * Time.deltaTime));

    }

    /// <summary>
    /// 뷰 앞, 뷰 오른쪽 방향을 얻습니다.
    /// </summary>
    /// <param name="viewForward">뷰의 앞 방향을 저장할 변수를 전달합니다.</param>
    /// <param name="viewRight">뷰의 오른쪽 방향을 저장할 변수를 전달합니다.</param>
    /// <returns></returns>
    private bool TryGetViewDirection(out Vector3 viewForward, out Vector3 viewRight)
    {
        // 메인 카메라를 얻습니다.
        Camera mainCamera = Camera.main;

        // 카메라를 얻지 못한 경우
        if(mainCamera == null)
        {
            viewForward = viewRight = Vector3.zero;
            return false;
        }

        // 뷰의 앞/옆 방향을 얻습니다.
        viewForward = mainCamera.transform.forward;
        viewRight = mainCamera.transform.right;

        return true;
    }

    public LayerMask m_DetectLayer;
    

    /// <summary>
    /// 바닥에 대한 정보를 얻습니다.
    /// </summary>
    /// <returns></returns>
    private bool TryGetFloorInfo()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

       if (PhysicsExt.Raycast(out _DrawLineInfo, ray, out RaycastHit hitInfo, 50.0f,
           m_DetectLayer, QueryTriggerInteraction.Ignore))
       {
            
       }

        return true;
    }

    /// <summary>
    /// 입력값을 월드 방향으로 변환합니다.
    /// </summary>
    /// <returns></returns>
    /// 
    private Vector3 InputToWorldDirection()
    {
        // 뷰의 앞/옆 방향을 얻습니다.
        Vector3 viewForward, viewRight;
        if(!TryGetViewDirection(out viewForward, out viewRight)) return Vector3.zero;

        // 앞 방향은 Y축을 제외하고 계산합니다.
        viewForward.y = 0.0f;
        viewForward.Normalize();

        // 좌우전후 입력 축 값을 연산합니다.
        viewForward *= _MovementInputAxis.y;
        viewRight *= _MovementInputAxis.x;

        // 방향을 생성합니다.
        Vector3 worldDirection = (viewForward + viewRight);

        // 계산된 방향을 반환합니다.
        return worldDirection.normalized; 
    }

    /// <summary>
    /// 점프 / 중력을 계산합니다.
    /// </summary>
    private void CalculateJumpAndGravity()
    {
        // 점프 요청이 존재한다면
        if(_JumpRequested) 
        {
            // 수직 속도를 점프 힘으로 설정하여 캐릭터가 튀어오르도록 합니다.
            _VerticalVelocity = Vector3.up * m_JumpPower;

            // 점프 요청 상태를 처리했으므로, 취소시킵니다.
            _JumpRequested = false;

        }

        // 수직 속도에 중력을 더하여 가속하도록 합니다.
        _VerticalVelocity += gravity * Time.deltaTime;
    }


    /// <summary>
    /// 이동 입력 시 호출되는 메서드입니다.
    /// </summary>
    /// <param name="inputAxis">키보드 입력 축 값이 전달됩니다.</param>
    public void OnMovementInput(Vector2 inputAxis)
    {
        _IsMovementInput = (_MovementInputAxis = inputAxis).sqrMagnitude != 0.0f;

    }

    /// <summary>
    /// 점프 입력 시 호출되는 메서드입니다.
    /// </summary>
    public void OnJumpInput()
    {
        // 땅에 닿아있는 상태인 경우
        if(isGrounded)
        {
            // 점프 요청
            _JumpRequested = true;
        }
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
       
    }




#endif




}
