using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

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
    private DrawGizmoLineInfo _DrawLineInfo;
    #endregion


    public void Update()
    {
        // �̵� ó��
        Movement();
        
    }


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
        TryGetFloorInfo();

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

    public LayerMask m_DetectLayer;
    

    /// <summary>
    /// �ٴڿ� ���� ������ ����ϴ�.
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
       
    }




#endif




}
