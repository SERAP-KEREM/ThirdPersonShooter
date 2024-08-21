using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementStateManager : MonoBehaviour
{
    #region Movement
    [Header("Movement")]
    public float currentMoveSpeed;
    public float walkSpeed=3,walkBackSpeed=2;
    public float runSpeed=7,runBackSpeed=5;
    public float crouchSpeed=2,crouchBackSpeed=1;
    public float airSpeed=1.5f;




    [HideInInspector] public Vector3 dir;
   [HideInInspector] public float horizontalInput;
    [HideInInspector] public float verticalInput;

    public CharacterController characterController;
    #endregion

    #region GroundCheck
    [Header("Ground Check")]
    [SerializeField] float groundYOfset;
    [SerializeField] LayerMask groundMask;
    Vector3 spherePos;
    #endregion GroundCheck

    #region Gravity
    [Header("Gravity")]
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float jumpForce=10;
    [HideInInspector] public bool jumped;
    Vector3 velocity;

    #endregion Gravity

    #region States
    public MovementBaseState previousState;
    public MovementBaseState currentState;

    public IdleState Idle = new IdleState();
    public WalkState Walk = new WalkState();
    public CrouchState Crouch = new CrouchState();  
    public RunState Run = new RunState();
    public JumpState Jump = new JumpState();
    #endregion States

    [HideInInspector] public Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        SwitchState(Idle);
    }

    private void Update()
    {
        GetDirectionAndMove();
        Gravity();
        Falling();
        anim.SetFloat("hzInput",horizontalInput);
        anim.SetFloat("vlInput",verticalInput);
     

        currentState.UpdateState(this);


    }

    public void SwitchState(MovementBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    void GetDirectionAndMove()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        Vector3 airDir= Vector3.zero;
        if(!IsGrounded()) airDir=transform.forward*verticalInput+transform.right*horizontalInput;
        else dir= transform.forward*verticalInput+transform.right*horizontalInput;


        characterController.Move((dir.normalized * currentMoveSpeed+ airDir.normalized*airSpeed) * Time.deltaTime);
    }

   public bool IsGrounded()
    {
        spherePos =new Vector3(transform.position.x,transform.position.y-groundYOfset,transform.position.z);
        if(Physics.CheckSphere(spherePos,characterController.radius-0.05f,groundMask)) return true;
        return false;
    }

    void Gravity()
    {
        if (!IsGrounded()) velocity.y += gravity * Time.deltaTime;
        else if(velocity.y<0) velocity.y = -2;
        characterController.Move(velocity*Time.deltaTime);

    }

    void Falling() =>anim.SetBool("Falling",!IsGrounded());
    private void OnDrawGizmos()
    {
        Gizmos.color= Color.red;
        Gizmos.DrawWireSphere(spherePos,characterController.radius-0.05f);    
    }

    public void JumpForce() => velocity.y += jumpForce;
    public void Jumped()=> jumped = true;
}
