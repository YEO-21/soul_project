using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public sealed class PlayerCharacterMovement : MonoBehaviour
{
    [Header("# ĳ���� ���� (�ӽ�)")]
    public float m_GravityMultiplier = 0.1f;

    /// <summary>
    /// �ȱ� �ӷ�
    /// </summary>
    public float m_WalkSpeed = 2.5f;

    /// <summary>
    /// �޸��� �ӷ�
    /// </summary>
    public float m_SprintSpeed = 6.0f;

    /// <summary>
    /// ���� ��
    /// </summary>
    public float m_JumpPower = 30.0f;

    /// <summary>
    /// ���� / ������
    /// </summary>
    public float m_AccelerationBrakingForce = 30.0f;

    /// <summary>
    /// ���� ��ȯ ����
    /// </summary>
    public float m_DirectionHandlingDrag = 5.0f;

    /// <summary>
    /// ȸ�� �ӷ�
    /// </summary>
    public float m_RotationSpeed = 720.0f;


    [Header("# �ٴ� ����")]
    public LayerMask m_FloorLayers;

    /// <summary>
    /// �̵� �Է� ���¸� ��Ÿ���ϴ�.
    /// </summary>
    private bool _IsMovementInput;

    /// <summary>
    /// �̵� �Է� �� ���� ����ϱ� ���� ����
    /// </summary>
    private Vector2 _MovementInputAxis;

    /// <summary>
    /// �Է¿� ���� ĳ���� �̵� ������ ��Ÿ���ϴ�.
    /// ���鿡 ���� ������ �� ������ ������� �ʽ��ϴ�.
    /// </summary>
    private Vector3 _HorizontalDirection;

    /// <summary>
    /// ĳ���Ϳ� ����� ���� �ӵ��� ��Ÿ���ϴ�.
    /// ���鿡 ���� ���⵵ �� ���� ����˴ϴ�.
    /// </summary>
    private Vector3 _HorizontalVelocity;
    
    /// <summary>
    /// ���ϱ� �̵� �� ����� ���� ���� ����
    /// </summary>
    private Vector3 _DodgeWorldDirection;

    /// <summary>
    /// ���ϱ� �̵� �� ����� �ӵ�
    /// </summary>
    private Vector3 _DodgeVelocity;

    /// <summary>
    /// ĳ���Ϳ� ����� ���� �ӵ��� ��Ÿ���ϴ�.
    /// </summary>
    private Vector3 _VerticalVelocity;

    /// <summary>
    /// ����� �̵� �ӷ��Դϴ�.
    /// </summary>
    private float _MoveSpeed;

    /// <summary>
    /// ��ǥ Yaw ȸ�����Դϴ�.
    /// </summary>
    private float _TargetYawRotation;

    /// <summary>
    /// ���� ��û���� ��Ÿ���� ���� ����
    /// </summary>
    private bool _JumpRequested;

    /// <summary>
    /// ���ϱ� ��û���� ��Ÿ���� ���� ����
    /// </summary>
    private bool _DodgeRequested;

    /// <summary>
    /// ���� �ٴ� ���� ���¸� ����ϱ� ���� �����Դϴ�.
    /// ���� ������ �����ϱ� ���Ͽ� ���˴ϴ�.
    /// </summary>
    private bool _PrevGroundedState;


    /// <summary>
    /// CharacterController ������Ʈ�� ��Ÿ���ϴ�.
    /// </summary>
    private CharacterController _CharacterController;


    /// <summary>
    /// CharacterController ������Ʈ�� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public CharacterController characterController => _CharacterController ??
        (_CharacterController = GetComponent<CharacterController>());

    /// <summary>
    /// ���� ���¸� ��Ÿ���ϴ�.
    /// </summary>
    public bool isSprint { get; private set; }

    /// <summary>
    /// ���� ���¸� ��Ÿ���ϴ�.
    /// </summary>
    public bool isJumping { get; private set; }

    /// <summary>
    /// �� ���� ���¸� ��Ÿ���� ������Ƽ�Դϴ�.
    /// ���������� CharacterController �� isGrounded ������Ƽ�� ����մϴ�.
    /// </summary>
    public bool isGrounded { get; private set; }

    /// <summary>
    /// ĳ���Ϳ� ����� �߷¿� ���� �б� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public Vector3 gravity => Physics.gravity * m_GravityMultiplier;

    #region Events
    /// <summary>
    /// ���� �̵��ӷ� ���� �̺�Ʈ
    /// </summary>
    public event System.Action<float> onHorizontalSpeedChanged;

    /// <summary>
    /// ���� ���� �̺�Ʈ
    /// </summary>
    public event System.Action onJumpStarted;

    /// <summary>
    /// ���� ���� �̺�Ʈ
    /// </summary>
    public event System.Action onGrounded;


    #endregion

    #region DEBUG
    private DrawGizmoSphereInfo _GroundCheckGizmoInfo;
    #endregion



    public void Update()
    {
        // �̵� ó��
        Movement();

        // ȸ�� ó��
        Rotation();
    }


    /// <summary>
    /// �̵��� ���õ� ������ ��� �̰����� �����մϴ�.
    /// </summary>
    private void Movement()
    {
        // �̵� �Է��� ���� ���� �������� ��ȯ�մϴ�.
        Vector3 targetWorldDirection = InputToWorldDirection();
        _HorizontalDirection = Vector3.MoveTowards(
            _HorizontalDirection, targetWorldDirection, m_DirectionHandlingDrag * Time.deltaTime);

        // �̵��� ���� ����
        Vector3 moveDirection = _HorizontalDirection;


        // ���� ���� ���� ����
        isGrounded = characterController.isGrounded;

        // �ٴڿ� ���� ������ ����ϴ�.
        bool isFloorDetected = TryGetFloorInfo(
            out bool isWalkableFloor, 
            out bool isOnSlope, 
            out Vector3 floorNormal);

        // ������ ���� ó��
        UpdateDodgetState();


        // �ٴ��� ������ ���
        if (isFloorDetected) 
        {
            // ������ �̵� ���⿡ �����մϴ�.
            ApplySlopeAngleToMoveDirection(
                isWalkableFloor, isOnSlope, floorNormal, ref moveDirection);
        }


        // ���� / �߷��� ����մϴ�.
        CalcualteJumpAndGravity();

        // ����� �ӷ��� ����մϴ�.
        UpdateMoveSpeed();

        // �����ų �ӵ��� ����մϴ�.
        _HorizontalVelocity = moveDirection * _MoveSpeed;

        // �̵� �ӷ� ���� �̺�Ʈ �߻�
        onHorizontalSpeedChanged?.Invoke(_HorizontalVelocity.magnitude);

        // �̵�
        characterController.Move(
            (_HorizontalVelocity * Time.deltaTime) +
            (_VerticalVelocity * Time.deltaTime));
    }

    /// <summary>
    /// �� ��, �� ������ ������ ����ϴ�.
    /// </summary>
    /// <param name="viewForward">���� �� ������ ������ ������ �����մϴ�.</param>
    /// <param name="viewRight">���� ������ ������ ������ ������ �����մϴ�.</param>
    /// <returns></returns>
    private bool TryGetViewDirection(out Vector3 viewForward, out Vector3 viewRight)
    {
        // ���� ī�޶� ����ϴ�.
        Camera mainCamera = Camera.main;

        // ī�޶� ���� ���� ���
        if (mainCamera == null)
        {
            viewForward = viewRight = Vector3.zero;
            return false;
        }

        // ���� ��/�� ������ ����ϴ�.
        viewForward = mainCamera.transform.forward;
        viewRight = mainCamera.transform.right;

        return true;
    }

    /// <summary>
    /// �ٴڿ� ���� ������ ����ϴ�.
    /// </summary>
    /// <param name="isWalkableFloor">�̵� ������ �ٴ� ���� ���θ� ��ȯ</param>
    /// <param name="isOnSlope">���� ���� ���θ� ��ȯ</param>
    /// <param name="floorNormal">������ �ٴ��� ����� ��ȯ</param>
    /// <returns>�ٴ� ���� ���θ� ��ȯ</returns>
    private bool TryGetFloorInfo(
        out bool isWalkableFloor, 
        out bool isOnSlope, 
        out Vector3 floorNormal)
    {
        // ���� ó���� ���� ��ü �߻� ����
        float slopeCheckRadius = characterController.radius;

        // ���� ó���� ���� ��ü ���� �߻� ���� ��ġ
        Vector3 slopeCheckOrgin = transform.position + (Vector3.up * slopeCheckRadius);

        // ���� ó���� ���� ��ü ���� �߻� ����
        Vector3 slopeCheckDirection = Vector3.down;

        // ���� ó���� ���� Ray ����
        Ray slopeRayData = new Ray(slopeCheckOrgin, slopeCheckDirection);

        // ���� ó���� ���� ��ü �߻� ����
        float slopeCheckLength = slopeCheckRadius * 2;

        bool isDetected = PhysicsExt.SphereCast(
            out _GroundCheckGizmoInfo,
            slopeRayData,
            slopeCheckRadius,
            out RaycastHit slopeHit,
            slopeCheckLength,
            m_FloorLayers,
            QueryTriggerInteraction.Ignore);

        // �ٴ��� ������ ���
        if (isDetected)
        {
            // ������ �ٴ��� �������͸� �̿��Ͽ� ������ ����ϴ�.
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);

            // ���� ��� �ʱ�ȭ
            floorNormal = slopeHit.normal;

            // ���� ������ �˻��մϴ�.
            isOnSlope = angle != 0.0f;

            // ���� ���� �� �ִ� �������� Ȯ���մϴ�.
            isWalkableFloor = angle <= characterController.slopeLimit;
        }
        // �ٴ��� �������� ���� ���
        else
        {
            isWalkableFloor = isOnSlope = false;
            floorNormal = Vector3.zero;
        }

        return isDetected;
    }

    /// <summary>
    /// ������ �̵� ���⿡ �����ŵ�ϴ�.
    /// </summary>
    /// <param name="isWalkableFloorDetected">�̵� ������ �ٴ� ���� ���� ����</param>
    /// <param name="isSolpeDetected">���� ���� ���� ����</param>
    /// <param name="floorNormal">�ٴ� ����� ����</param>
    /// <param name="moveDirection">�̵� ������ ����</param>
    private void ApplySlopeAngleToMoveDirection(
        bool isWalkableFloorDetected,
        bool isSolpeDetected,
        in Vector3 floorNormal,
        ref Vector3 moveDirection)
    {
        // ������ ������ ���
        if (isSolpeDetected)
        {
            // �̵� ������ �ٴ��� ������ ���
            if (isWalkableFloorDetected)
            {
                // �̵��� ���� ������ �ٴ��� ������ �������� ������ŵ�ϴ�.
                moveDirection = Vector3.ProjectOnPlane(moveDirection, floorNormal).normalized;
            }
            // ������ ����������, �̵��� �� ���� ������ ��� (���ĸ� ���� ó��)
            else
            {
                // �̵� ������ �ٴ��� �� �������� �����Ͽ� ������ ���ϵ��� �մϴ�.
                moveDirection = floorNormal;

                // �̲����� �� �ֵ��� �� ������ ����մϴ�.
                isGrounded = false;
            }
        }
    }

    /// <summary>
    /// ���ϱ� ���¿� ���� ó���� �����մϴ�.
    /// </summary>
    private void UpdateDodgetState()
    {
        // ���ϱ� ��û�� �����ϴ� ���
        if(_DodgeRequested)
        {

        }
    }


    /// <summary>
    /// �Է°��� ���� �������� ��ȯ�մϴ�.
    /// </summary>
    /// <returns></returns>
    private Vector3 InputToWorldDirection()
    {
        // ���� ��/�� ������ ����ϴ�.
        Vector3 viewForward, viewRight;
        if (!TryGetViewDirection(out viewForward, out viewRight)) return Vector3.zero;

        // �� ������ Y���� �����ϰ� ����մϴ�.
        viewForward.y = 0.0f;
        viewForward.Normalize();

        // �¿����� �Է� �� ���� �����մϴ�.
        viewForward *= _MovementInputAxis.y;
        viewRight *= _MovementInputAxis.x;

        // ������ �����մϴ�.
        Vector3 worldDirection = (viewForward + viewRight);

        // ���� ������ �������ͷ� ��ȯ�մϴ�.
        return worldDirection.normalized;
    }

    /// <summary>
    /// ���� / �߷��� ����մϴ�.
    /// </summary>
    private void CalcualteJumpAndGravity()
    {
        // ���� ���°� �ƴϸ�, ���̻� �ٴ��� �������� ���ϴ� ���(���ϸ� �����ϴ� ���) 
        if (_PrevGroundedState && !isGrounded && !isJumping)
        {
            // ���� �ӵ� �ʱ�ȭ
            _VerticalVelocity = Vector3.zero;

            // ���� ���·� �����Ͽ�, �����ϴ� ���� �������� ���ϵ��� �մϴ�.
            isJumping = true;

            // ���� ���� �̺�Ʈ �߻�
            onJumpStarted?.Invoke();
        }

        // ���� ������ ���
        if (isGrounded)
        {
            if(isJumping)
            {
                // ���� ���� �ʱ�ȭ
                isJumping = false;

                // �� ���� �̺�Ʈ�� �߻��մϴ�.
                onGrounded?.Invoke();
            }

            // �ִ� ���� �ӷ��� �����մϴ�.
            float maxFallSpeed = m_SprintSpeed * -2;

            // ���� �ӷ��� �ִ�ġ�� ���� ���, �ִ� ���� �ӷ����� �����մϴ�.
            if (_VerticalVelocity.y < maxFallSpeed)
                _VerticalVelocity.y = maxFallSpeed;
        }

        // ���� ��û�� �����Ѵٸ�
        if (!isJumping && _JumpRequested)
        {
            // ���� ���·� �����մϴ�.
            isJumping = true;

            // ���� ���� �̺�Ʈ �߻�
            onJumpStarted?.Invoke();

            // ���� �ӵ��� ���� ������ �����Ͽ� ĳ���Ͱ� Ƣ��������� �մϴ�.
            _VerticalVelocity = Vector3.up * m_JumpPower;

            // ���� ��û ���¸� ó�������Ƿ�, ��ҽ�ŵ�ϴ�.
            _JumpRequested = false;
        }

        // ���� �ӵ��� �߷��� ���Ͽ� �����ϵ��� �մϴ�.
        _VerticalVelocity += gravity * Time.deltaTime;

        // ���� �ٴ� ���� ���� ����
        _PrevGroundedState = isGrounded;
    }

    /// <summary>
    /// �̵� �ӷ��� �����մϴ�.
    /// </summary>
    private void UpdateMoveSpeed()
    {
        // ���� ������ ��� �ӷ��� �������� �ʵ��� �մϴ�.
        if (isJumping) return;

        // ��ǥ �ӷ��� �����մϴ�.
        float targetSpeed = isSprint ? m_SprintSpeed : m_WalkSpeed;

        // �̵� �Է��� ������ ���� ��� ��ǥ �ӷ��� 0 ���� �����մϴ�.
        if (!_IsMovementInput) targetSpeed = 0.0f;

        // �̵� �ӷ��� �����մϴ�.
        _MoveSpeed = Mathf.MoveTowards(
            _MoveSpeed, 
            targetSpeed, 
            m_AccelerationBrakingForce * Time.deltaTime);
    }

    /// <summary>
    /// ȸ���� ���õ� ������ ��� �̰����� ó���մϴ�.
    /// </summary>
    private void Rotation()
    {
        RotationToMoveDirection();
    }

    /// <summary>
    /// �� ���⿡ ���� ������Ʈ�� ȸ���ϵ��� �մϴ�.
    /// </summary>
    private void RotationToViewDirection()
    {

    }

    /// <summary>
    /// �̵��ϴ� ���⿡ ���� ������Ʈ�� ȸ���ϵ��� �մϴ�.
    /// </summary>
    private void RotationToMoveDirection()
    {
        _TargetYawRotation = Mathf.Atan2(_HorizontalDirection.x, _HorizontalDirection.z) * Mathf.Rad2Deg;

        // ���� Yaw ȸ����
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
    /// �̵� �Է� �� ȣ��Ǵ� �޼����Դϴ�.
    /// </summary>
    /// <param name="inputAxis">Ű���� �Է� �� ���� ���޵˴ϴ�.</param>
    public void OnMovementInput(Vector2 inputAxis)
    {
        _IsMovementInput = (_MovementInputAxis = inputAxis).sqrMagnitude != 0.0f;
    }

    /// <summary>
    /// ���� �Է� �� ȣ��Ǵ� �޼����Դϴ�.
    /// </summary>
    public void OnJumpInput()
    {
        // ���� ����ִ� ������ ���
        if (isGrounded)
        {
            // ���� ��û
            _JumpRequested = true;
        }
    }

    /// <summary>
    /// ���ϱ� �Է� �� ȣ��Ǵ� �޼����Դϴ�.
    /// </summary>
    public void OnDodgeInput()
    {
        // ���� ��/������ ������ ����ϴ�.
        if(TryGetViewDirection(out Vector3 viewForward, out Vector3 viewRight))
        {
            // �̵� �Է��� �����ϴ� ���
            if (_IsMovementInput)
            {
                Vector3 forwardDirection = viewForward * _MovementInputAxis.y;
                Vector3 rightDirection = viewRight * _MovementInputAxis.x;

                _DodgeWorldDirection = (forwardDirection + rightDirection).normalized;
            }
            // �̵� �Է��� �������� �ʴ� ���
            else
            {
                _DodgeWorldDirection = viewForward * -1.0f;
            }

            // ���ϱ� ��û ���·� �����մϴ�.
            _DodgeRequested = true;
        }

      
    }

    /// <summary>
    /// �޸��� Ű �Է� �� ȣ��Ǵ� �޼����Դϴ�.
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
