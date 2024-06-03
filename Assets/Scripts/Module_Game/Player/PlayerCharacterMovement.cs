using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public float m_WalkSpeed = 6.0f;

    /// <summary>
    /// ���� ��
    /// </summary>
    public float m_JumpPower = 30.0f;

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
    /// ĳ���Ϳ� ����� ���� �ӵ��� ��Ÿ���ϴ�.
    /// </summary>
    private Vector3 _VerticalVelocity;

    /// <summary>
    /// ���� ��û���� ��Ÿ���� ���� ����
    /// </summary>
    private bool _JumpRequested;

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
    /// �� ���� ���¸� ��Ÿ���� ������Ƽ�Դϴ�.
    /// ���������� CharacterController �� isGrounded ������Ƽ�� ����մϴ�.
    /// </summary>
    public bool isGrounded { get; private set; }

    /// <summary>
    /// ĳ���Ϳ� ����� �߷¿� ���� �б� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public Vector3 gravity => Physics.gravity * m_GravityMultiplier;

    #region DEBUG
    private DrawGizmoSphereInfo _GroundCheckGizmoInfo;
    #endregion


    public void Update()
    {
        // �̵� ó��
        Movement();
        
    }

    Vector3 _MoveDirection;


    /// <summary>
    /// �̵��� ���õ� ������ ��� �̰����� �����մϴ�.
    /// </summary>
    private void Movement()
    {
        // �̵� �Է��� ���� ���� �������� ��ȯ�մϴ�.
        Vector3 worldDirection = InputToWorldDirection();

        // ���� ���� ���� ����
        isGrounded = characterController.isGrounded;

        // �ٴڿ� ���� ������ ����ϴ�.
        bool isFloorDetected = 
            TryGetFloorInfo(out bool isWalkableFloor, out bool isOnSlope, out Vector3 floorNormal);

        // �ٴ��� ������ ���
        if(isFloorDetected)
        {
            // ������ �̵� ���⿡ �����մϴ�.
            ApplySlopeAngleToMoveDirecion(isWalkableFloor, isOnSlope, floorNormal, ref worldDirection);
        }

        // ������
        _MoveDirection = worldDirection;

        // ���� / �߷��� ����մϴ�.
        CalculateJumpAndGravity();

        // �̵�
        characterController.Move(
            (worldDirection * m_WalkSpeed *Time.deltaTime) + 
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
        if(mainCamera == null)
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
    private bool TryGetFloorInfo(out bool isWalkableFloor, out bool isOnSlope, out Vector3 floorNormal)
    {
        // ���� ó���� ���� ��ü �߻� ����
        float slopeCheckRadius = characterController.radius;

        // ���� ó���� ���� ��ü ���� �߻� ���� ��ġ
        Vector3 slopeCheckOrigin = transform.position + (Vector3.up * slopeCheckRadius);

        // ���� ó���� ���� ��ü ���� �߻� ����
        Vector3 slopeCheckDirection = Vector3.down;


        // ���� ó���� ���� Ray ����
        Ray slopeRayData = new Ray(slopeCheckOrigin, slopeCheckDirection);

        // ���� ó���� ���� ��ü �߻� ����
        float slopeCheckLength = slopeCheckRadius * 2;


        bool isDetected = PhysicsExt.Spherecast(
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
        // �ٴڸ��� �������� ���� ���
        else
        {
            isWalkableFloor = isOnSlope = false;
            floorNormal = Vector3.zero;
        }


        return isDetected;
    }

    /// <summary>
    /// ������ �̵� ���⿡ �����ŵ�ϴ�.
    /// 
    /// </summary>
    /// <param name="isWalkableFloorDetected">�̵� ������ �ٴ� ���� ���� ����</param>
    /// <param name="isSlopeDetected">���� ���� ���θ� ����</param>
    /// <param name="floorNormal">�ٴ� ����� ����</param>
    /// <param name="moveDirection">�̵� ������ ����</param>
    private void ApplySlopeAngleToMoveDirecion(
        bool isWalkableFloorDetected,
        bool isSlopeDetected,
        in Vector3 floorNormal,
        ref Vector3 moveDirection)
    {
        // ������ ������ ���
        if(isSlopeDetected)
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
    /// �Է°��� ���� �������� ��ȯ�մϴ�.
    /// </summary>
    /// <returns></returns>
    /// 
    private Vector3 InputToWorldDirection()
    {
        // ���� ��/�� ������ ����ϴ�.
        Vector3 viewForward, viewRight;
        if(!TryGetViewDirection(out viewForward, out viewRight)) return Vector3.zero;

        // �� ������ Y���� �����ϰ� ����մϴ�.
        viewForward.y = 0.0f;
        viewForward.Normalize();

        // �¿����� �Է� �� ���� �����մϴ�.
        viewForward *= _MovementInputAxis.y;
        viewRight *= _MovementInputAxis.x;

        // ������ �����մϴ�.
        Vector3 worldDirection = (viewForward + viewRight);

        // ���� ������ ��ȯ�մϴ�.
        return worldDirection.normalized; 
    }

    /// <summary>
    /// ���� / �߷��� ����մϴ�.
    /// </summary>
    private void CalculateJumpAndGravity()
    {
        // ���� ��û�� �����Ѵٸ�
        if(_JumpRequested) 
        {
            // ���� �ӵ��� ���� ������ �����Ͽ� ĳ���Ͱ� Ƣ��������� �մϴ�.
            _VerticalVelocity = Vector3.up * m_JumpPower;

            // ���� ��û ���¸� ó�������Ƿ�, ��ҽ�ŵ�ϴ�.
            _JumpRequested = false;

        }

        // ���� �ӵ��� �߷��� ���Ͽ� �����ϵ��� �մϴ�.
        _VerticalVelocity += gravity * Time.deltaTime;
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
        if(isGrounded)
        {
            // ���� ��û
            _JumpRequested = true;
        }
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        PhysicsExt.DrawGizmoSphere(_GroundCheckGizmoInfo);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + _MoveDirection*5);
    }

#endif




}
