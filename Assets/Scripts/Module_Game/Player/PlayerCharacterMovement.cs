using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public sealed class PlayerCharacterMovement : MonoBehaviour
{
    [Header("# 캐릭터 정보 (임시)")]
    public float m_GravityMultiplier = 0.1f;

    /// <summary>
    /// 걷기 속력
    /// </summary>
    public float m_WalkSpeed = 2.5f;

    /// <summary>
    /// 달리기 속력
    /// </summary>
    public float m_SprintSpeed = 6.0f;

    /// <summary>
    /// 점프 힘
    /// </summary>
    public float m_JumpPower = 30.0f;

    /// <summary>
    /// 가속 / 제동력
    /// </summary>
    public float m_AccelerationBrakingForce = 30.0f;

    /// <summary>
    /// 방향 전환 저항
    /// </summary>
    public float m_DirectionHandlingDrag = 5.0f;

    /// <summary>
    /// 회전 속력
    /// </summary>
    public float m_RotationSpeed = 720.0f;


    [Header("# 바닥 관련")]
    public LayerMask m_FloorLayers;

    /// <summary>
    /// 이동 입력 상태를 나타냅니다.
    /// </summary>
    private bool _IsMovementInput;

    /// <summary>
    /// 이동 입력 축 값을 기록하기 위한 변수
    /// </summary>
    private Vector2 _MovementInputAxis;

    /// <summary>
    /// 입력에 따른 캐릭터 이동 방향을 나타냅니다.
    /// 경사면에 따른 방향은 이 변수에 적용되지 않습니다.
    /// </summary>
    private Vector3 _HorizontalDirection;

    /// <summary>
    /// 캐릭터에 적용될 수평 속도를 나타냅니다.
    /// 경사면에 따른 방향도 이 곳에 연산됩니다.
    /// </summary>
    private Vector3 _HorizontalVelocity;
    
    /// <summary>
    /// 피하기 이동 시 적용될 월드 기준 방향
    /// </summary>
    private Vector3 _DodgeWorldDirection;

    /// <summary>
    /// 피하기 이동 시 적용될 속도
    /// </summary>
    private Vector3 _DodgeVelocity;

    /// <summary>
    /// 캐릭터에 적용될 수직 속도를 나타냅니다.
    /// </summary>
    private Vector3 _VerticalVelocity;

    /// <summary>
    /// 적용될 이동 속력입니다.
    /// </summary>
    private float _MoveSpeed;

    /// <summary>
    /// 목표 Yaw 회전값입니다.
    /// </summary>
    private float _TargetYawRotation;

    /// <summary>
    /// 점프 요청됨을 나타내기 위한 변수
    /// </summary>
    private bool _JumpRequested;

    /// <summary>
    /// 피하기 요청됨을 나타내기 위한 변수
    /// </summary>
    private bool _DodgeRequested;

    /// <summary>
    /// 이전 바닥 감지 상태를 기록하기 위한 변수입니다.
    /// 낙하 시작을 감지하기 위하여 사용됩니다.
    /// </summary>
    private bool _PrevGroundedState;


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
    /// 질주 상태를 나타냅니다.
    /// </summary>
    public bool isSprint { get; private set; }

    /// <summary>
    /// 점프 상태를 나타냅니다.
    /// </summary>
    public bool isJumping { get; private set; }

    /// <summary>
    /// 땅 감지 상태를 나타내는 프로퍼티입니다.
    /// 내부적으로 CharacterController 의 isGrounded 프로퍼티를 사용합니다.
    /// </summary>
    public bool isGrounded { get; private set; }

    /// <summary>
    /// 캐릭터에 적용될 중력에 대한 읽기 전용 프로퍼티입니다.
    /// </summary>
    public Vector3 gravity => Physics.gravity * m_GravityMultiplier;

    #region Events
    /// <summary>
    /// 수평 이동속력 변경 이벤트
    /// </summary>
    public event System.Action<float> onHorizontalSpeedChanged;

    /// <summary>
    /// 점프 시작 이벤트
    /// </summary>
    public event System.Action onJumpStarted;

    /// <summary>
    /// 땅에 닿음 이벤트
    /// </summary>
    public event System.Action onGrounded;


    #endregion

    #region DEBUG
    private DrawGizmoSphereInfo _GroundCheckGizmoInfo;
    #endregion



    public void Update()
    {
        // 이동 처리
        Movement();

        // 회전 처리
        Rotation();
    }


    /// <summary>
    /// 이동과 관련된 연산을 모두 이곳에서 진행합니다.
    /// </summary>
    private void Movement()
    {
        // 이동 입력을 월드 기준 방향으로 변환합니다.
        Vector3 targetWorldDirection = InputToWorldDirection();
        _HorizontalDirection = Vector3.MoveTowards(
            _HorizontalDirection, targetWorldDirection, m_DirectionHandlingDrag * Time.deltaTime);

        // 이동에 사용될 방향
        Vector3 moveDirection = _HorizontalDirection;


        // 땅에 닿음 상태 갱신
        isGrounded = characterController.isGrounded;

        // 바닥에 대한 정보를 얻습니다.
        bool isFloorDetected = TryGetFloorInfo(
            out bool isWalkableFloor, 
            out bool isOnSlope, 
            out Vector3 floorNormal);

        // 구르기 상태 처리
        UpdateDodgetState();


        // 바닥을 감지한 경우
        if (isFloorDetected) 
        {
            // 경사면을 이동 방향에 적용합니다.
            ApplySlopeAngleToMoveDirection(
                isWalkableFloor, isOnSlope, floorNormal, ref moveDirection);
        }


        // 점프 / 중력을 계산합니다.
        CalcualteJumpAndGravity();

        // 적용될 속력을 계산합니다.
        UpdateMoveSpeed();

        // 적용시킬 속도를 계산합니다.
        _HorizontalVelocity = moveDirection * _MoveSpeed;

        // 이동 속력 변경 이벤트 발생
        onHorizontalSpeedChanged?.Invoke(_HorizontalVelocity.magnitude);

        // 이동
        characterController.Move(
            (_HorizontalVelocity * Time.deltaTime) +
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
        if (mainCamera == null)
        {
            viewForward = viewRight = Vector3.zero;
            return false;
        }

        // 뷰의 앞/옆 방향을 얻습니다.
        viewForward = mainCamera.transform.forward;
        viewRight = mainCamera.transform.right;

        return true;
    }

    /// <summary>
    /// 바닥에 대한 정보를 얻습니다.
    /// </summary>
    /// <param name="isWalkableFloor">이동 가능한 바닥 감지 여부를 반환</param>
    /// <param name="isOnSlope">경사면 감지 여부를 반환</param>
    /// <param name="floorNormal">감지한 바닥의 노먹을 반환</param>
    /// <returns>바닥 감지 여부를 반환</returns>
    private bool TryGetFloorInfo(
        out bool isWalkableFloor, 
        out bool isOnSlope, 
        out Vector3 floorNormal)
    {
        // 경사면 처리를 위한 구체 발사 길이
        float slopeCheckRadius = characterController.radius;

        // 경사면 처리를 위한 구체 영역 발사 시작 위치
        Vector3 slopeCheckOrgin = transform.position + (Vector3.up * slopeCheckRadius);

        // 경사면 처리를 위한 구체 영역 발사 방향
        Vector3 slopeCheckDirection = Vector3.down;

        // 경사면 처리를 위한 Ray 변수
        Ray slopeRayData = new Ray(slopeCheckOrgin, slopeCheckDirection);

        // 경사면 처리를 위한 구체 발사 길이
        float slopeCheckLength = slopeCheckRadius * 2;

        bool isDetected = PhysicsExt.SphereCast(
            out _GroundCheckGizmoInfo,
            slopeRayData,
            slopeCheckRadius,
            out RaycastHit slopeHit,
            slopeCheckLength,
            m_FloorLayers,
            QueryTriggerInteraction.Ignore);

        // 바닥을 감지한 경우
        if (isDetected)
        {
            // 감지한 바닥의 법선벡터를 이용하여 각도를 얻습니다.
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);

            // 경사면 노멀 초기화
            floorNormal = slopeHit.normal;

            // 경사면 감지를 검사합니다.
            isOnSlope = angle != 0.0f;

            // 현재 오를 수 있는 경사면임을 확인합니다.
            isWalkableFloor = angle <= characterController.slopeLimit;
        }
        // 바닥을 감지하지 못한 경우
        else
        {
            isWalkableFloor = isOnSlope = false;
            floorNormal = Vector3.zero;
        }

        return isDetected;
    }

    /// <summary>
    /// 경사면을 이동 방향에 적용시킵니다.
    /// </summary>
    /// <param name="isWalkableFloorDetected">이동 가능한 바닥 감지 여부 전달</param>
    /// <param name="isSolpeDetected">경사면 감지 여부 전달</param>
    /// <param name="floorNormal">바닥 노멀을 전달</param>
    /// <param name="moveDirection">이동 방향을 전달</param>
    private void ApplySlopeAngleToMoveDirection(
        bool isWalkableFloorDetected,
        bool isSolpeDetected,
        in Vector3 floorNormal,
        ref Vector3 moveDirection)
    {
        // 경사면을 감지한 경우
        if (isSolpeDetected)
        {
            // 이동 가능한 바닥을 감지한 경우
            if (isWalkableFloorDetected)
            {
                // 이동에 사용될 방향을 바닥의 기울어진 방향으로 투영시킵니다.
                moveDirection = Vector3.ProjectOnPlane(moveDirection, floorNormal).normalized;
            }
            // 경사면을 감지했지만, 이동할 수 없는 각도인 경우 (가파른 경사면 처리)
            else
            {
                // 이동 방향을 바닥의 앞 방향으로 지정하여 오르지 못하도록 합니다.
                moveDirection = floorNormal;

                // 미끄러질 수 있도록 땅 감지를 취소합니다.
                isGrounded = false;
            }
        }
    }

    /// <summary>
    /// 피하기 상태에 대한 처리를 진행합니다.
    /// </summary>
    private void UpdateDodgetState()
    {
        // 피하기 요청이 존재하는 경우
        if(_DodgeRequested)
        {

        }
    }


    /// <summary>
    /// 입력값을 월드 방향으로 변환합니다.
    /// </summary>
    /// <returns></returns>
    private Vector3 InputToWorldDirection()
    {
        // 뷰의 앞/옆 방향을 얻습니다.
        Vector3 viewForward, viewRight;
        if (!TryGetViewDirection(out viewForward, out viewRight)) return Vector3.zero;

        // 앞 방향은 Y축을 제외하고 계산합니다.
        viewForward.y = 0.0f;
        viewForward.Normalize();

        // 좌우전후 입력 축 값을 연산합니다.
        viewForward *= _MovementInputAxis.y;
        viewRight *= _MovementInputAxis.x;

        // 방향을 생성합니다.
        Vector3 worldDirection = (viewForward + viewRight);

        // 계산된 방향을 단위벡터로 변환합니다.
        return worldDirection.normalized;
    }

    /// <summary>
    /// 점프 / 중력을 계산합니다.
    /// </summary>
    private void CalcualteJumpAndGravity()
    {
        // 점프 상태가 아니며, 더이상 바닥을 감지하지 못하는 경우(낙하를 시작하는 경우) 
        if (_PrevGroundedState && !isGrounded && !isJumping)
        {
            // 수직 속도 초기화
            _VerticalVelocity = Vector3.zero;

            // 점프 상태로 변경하여, 낙하하는 동안 점프하지 못하도록 합니다.
            isJumping = true;

            // 점프 시작 이벤트 발생
            onJumpStarted?.Invoke();
        }

        // 땅을 감지한 경우
        if (isGrounded)
        {
            if(isJumping)
            {
                // 점프 상태 초기화
                isJumping = false;

                // 땅 감지 이벤트를 발생합니다.
                onGrounded?.Invoke();
            }

            // 최대 낙하 속력을 지정합니다.
            float maxFallSpeed = m_SprintSpeed * -2;

            // 낙하 속력이 최대치를 넘은 경우, 최대 낙하 속력으로 지정합니다.
            if (_VerticalVelocity.y < maxFallSpeed)
                _VerticalVelocity.y = maxFallSpeed;
        }

        // 점프 요청이 존재한다면
        if (!isJumping && _JumpRequested)
        {
            // 점프 상태로 설정합니다.
            isJumping = true;

            // 점프 시작 이벤트 발생
            onJumpStarted?.Invoke();

            // 수직 속도를 점프 힘으로 설정하여 캐릭터가 튀어오르도록 합니다.
            _VerticalVelocity = Vector3.up * m_JumpPower;

            // 점프 요청 상태를 처리했으므로, 취소시킵니다.
            _JumpRequested = false;
        }

        // 수직 속도에 중력을 더하여 가속하도록 합니다.
        _VerticalVelocity += gravity * Time.deltaTime;

        // 이전 바닥 감지 상태 갱신
        _PrevGroundedState = isGrounded;
    }

    /// <summary>
    /// 이동 속력을 갱신합니다.
    /// </summary>
    private void UpdateMoveSpeed()
    {
        // 점프 상태인 경우 속력을 갱신하지 않도록 합니다.
        if (isJumping) return;

        // 목표 속력을 지정합니다.
        float targetSpeed = isSprint ? m_SprintSpeed : m_WalkSpeed;

        // 이동 입력이 들어오지 않은 경우 목표 속력을 0 으로 설정합니다.
        if (!_IsMovementInput) targetSpeed = 0.0f;

        // 이동 속력을 갱신합니다.
        _MoveSpeed = Mathf.MoveTowards(
            _MoveSpeed, 
            targetSpeed, 
            m_AccelerationBrakingForce * Time.deltaTime);
    }

    /// <summary>
    /// 회전과 관련된 연산을 모두 이곳에서 처리합니다.
    /// </summary>
    private void Rotation()
    {
        RotationToMoveDirection();
    }

    /// <summary>
    /// 뷰 방향에 따라 오브젝트가 회전하도록 합니다.
    /// </summary>
    private void RotationToViewDirection()
    {

    }

    /// <summary>
    /// 이동하는 방향에 따라 오브젝트가 회전하도록 합니다.
    /// </summary>
    private void RotationToMoveDirection()
    {
        _TargetYawRotation = Mathf.Atan2(_HorizontalDirection.x, _HorizontalDirection.z) * Mathf.Rad2Deg;

        // 현재 Yaw 회전값
        float currentYawRotation = Mathf.MoveTowardsAngle(
            transform.eulerAngles.y,
            _TargetYawRotation,
            m_RotationSpeed * Time.deltaTime);

        if(_IsMovementInput)
        {
            transform.eulerAngles = Vector3.up * currentYawRotation;
        }

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
        if (isGrounded)
        {
            // 점프 요청
            _JumpRequested = true;
        }
    }

    /// <summary>
    /// 피하기 입력 시 호출되는 메서드입니다.
    /// </summary>
    public void OnDodgeInput()
    {
        // 뷰의 앞/오른쪽 방향을 얻습니다.
        if(TryGetViewDirection(out Vector3 viewForward, out Vector3 viewRight))
        {
            // 이동 입력이 존재하는 경우
            if (_IsMovementInput)
            {
                Vector3 forwardDirection = viewForward * _MovementInputAxis.y;
                Vector3 rightDirection = viewRight * _MovementInputAxis.x;

                _DodgeWorldDirection = (forwardDirection + rightDirection).normalized;
            }
            // 이동 입력이 존재하지 않는 경우
            else
            {
                _DodgeWorldDirection = viewForward * -1.0f;
            }

            // 피하기 요청 상태로 설정합니다.
            _DodgeRequested = true;
        }

      
    }

    /// <summary>
    /// 달리기 키 입력 시 호출되는 메서드입니다.
    /// </summary>
    /// <param name="isPressed"></param>
    public void OnSprintInput(bool isPressed)
    {
        isSprint = isPressed;
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        PhysicsExt.DrawGizmoSphere(_GroundCheckGizmoInfo);
    }
#endif

}
