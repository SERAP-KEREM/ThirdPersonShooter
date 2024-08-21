using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AimStateManager : MonoBehaviour
{
    public AimBaseState currentState;
    public HipFireState Hip = new HipFireState();
    public AimState Aim = new AimState();

    [SerializeField] float mouseSense = 1;
    public float xAxis, yAxis;
    [SerializeField] Transform camFollowPos;

    [HideInInspector] public Animator anim;
    [HideInInspector] public CinemachineVirtualCamera virtualCamera;
    public float adsFov = 40;
    [HideInInspector] public float hipFov;
    [HideInInspector] public float currentFov;
    public float fovSmoothSpeed = 10;

    public Transform aimPos;
    [HideInInspector] public Vector3 actualAimPos;
    [SerializeField] float aimSmoothSpeed = 20;
    [SerializeField] LayerMask aimMask;

    float xFollowPos;
    float yFollowPos, ogYPos;
    [SerializeField] float crouchCamHeight=1f;
    [SerializeField] float shouldSwapSpeed = 5;
    MovementStateManager movementStateManager;



    private void Start()
    {
        movementStateManager=GetComponent<MovementStateManager>();
        xFollowPos = camFollowPos.localPosition.x;
        ogYPos=camFollowPos.localPosition.y;
        yFollowPos = ogYPos;
        virtualCamera= GetComponentInChildren<CinemachineVirtualCamera>();
        hipFov = virtualCamera.m_Lens.FieldOfView;
        anim=GetComponent<Animator>();
        SwitchState(Hip);
    }
    private void Update()
    {
        xAxis += Input.GetAxisRaw("Mouse X") * mouseSense; 
        yAxis -= Input.GetAxisRaw("Mouse Y") * mouseSense; 
        yAxis=Mathf.Clamp(yAxis, -80, 80);

        virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(virtualCamera.m_Lens.FieldOfView,currentFov, fovSmoothSpeed * Time.deltaTime);

        Vector2 screenCenter =new Vector2(Screen.width/2, Screen.height/2);
        Ray ray =Camera.main.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, aimMask))
            aimPos.position = Vector3.Lerp(aimPos.position, hit.point, aimSmoothSpeed * Time.deltaTime);

        MoveCamera();

        currentState.UpdateState(this);

    }
    private void LateUpdate()
    {
        camFollowPos.localEulerAngles=new Vector3(yAxis,camFollowPos.localEulerAngles.y,camFollowPos.localEulerAngles.z);
        transform.eulerAngles= new Vector3(transform.eulerAngles.x,xAxis,transform.eulerAngles.z);
    }

    public void SwitchState(AimBaseState state)
    {
        currentState= state;
        currentState.EnterState(this);
    }

    void MoveCamera()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt)) xFollowPos = -xFollowPos;
        if (movementStateManager.currentState == movementStateManager.Crouch) yFollowPos = crouchCamHeight;
        else yFollowPos = ogYPos;

        Vector3 newFollowPos= new Vector3(xFollowPos,yFollowPos,camFollowPos.localPosition.z);
        camFollowPos.localPosition = Vector3.Lerp(camFollowPos.localPosition, newFollowPos, shouldSwapSpeed * Time.deltaTime);

    }
}
