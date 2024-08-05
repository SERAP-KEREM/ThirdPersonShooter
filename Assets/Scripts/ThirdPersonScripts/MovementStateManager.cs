using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementStateManager : MonoBehaviour
{
    [Header("Movement")]
    public float currentMoveSpeed;
    public float walkSpeed=3,walkBackSpeed=2;
    public float runSpeed=7,runBackSpeed=5;
    public float crouchSpeed=2,crouchBackSpeed=1;




    [HideInInspector] public Vector3 dir;
   [HideInInspector] public float horizontalInput;
    [HideInInspector] public float verticalInput;
    public CharacterController characterController;

    [Header("Ground Check")]
    [SerializeField] float groundYOfset;
    [SerializeField] LayerMask groundMask;
    Vector3 spherePos;

    [Header("Gravity")]
    [SerializeField] float gravity = -9.81f;
    Vector3 velocity;

    

    MovementBaseState currentState;

    public IdleState Idle = new IdleState();
    public WalkState Walk = new WalkState();
    public CrouchState Crouch = new CrouchState();  
    public RunState Run = new RunState();

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
        anim.SetFloat("hzInput",horizontalInput);
        anim.SetFloat("vlInput",verticalInput);

        
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

        dir = transform.forward*verticalInput+transform.right*horizontalInput;

        characterController.Move(dir.normalized * currentMoveSpeed*Time.deltaTime);
    }

    bool IsGrounded()
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
    private void OnDrawGizmos()
    {
        Gizmos.color= Color.red;
        Gizmos.DrawWireSphere(spherePos,characterController.radius-0.05f);    
    }
}
