using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementStateManager : MonoBehaviour
{
    public float moveSpeed = 3;
    [HideInInspector] public Vector3 dir;
    float horizontalInput;
     float verticalInput;
    public CharacterController characterController;

    [SerializeField] float groundYOfset;
    [SerializeField] LayerMask groundMask;
    Vector3 spherePos;

    [SerializeField] float gravity = -9.81f;
    Vector3 velocity;

    [HideInInspector] public Animator anim;

    private void Start()
    {
       characterController = GetComponent<CharacterController>();
        anim=GetComponentInChildren<Animator>();    
    }

    private void Update()
    {
        GetDirectionAndMove();
        Gravity();

        anim.SetFloat("hzInput", horizontalInput);
        anim.SetFloat("vlInput",verticalInput);
    }

    void GetDirectionAndMove()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        dir = transform.forward*verticalInput+transform.right*horizontalInput;

        characterController.Move(dir.normalized * moveSpeed*Time.deltaTime);
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
