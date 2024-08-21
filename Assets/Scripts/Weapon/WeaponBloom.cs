using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBloom : MonoBehaviour
{
    [SerializeField] float defaultBloomAngle = 3;
    [SerializeField] float walkBloomMultiplier = 1.5f;
    [SerializeField] float crouchBloomMultiplier = 0.5f;
    [SerializeField] float sprintBloomMultiplier = 2f;
    [SerializeField] float adsBloomMultiplier = 0.5f;

    MovementStateManager movementStateManager;
    AimStateManager aimState;



    float currentBloom;

    private void Start()
    {
        movementStateManager=GetComponentInParent<MovementStateManager>();
        aimState = GetComponentInParent<AimStateManager>();
    }

    public Vector3 BloomAngle(Transform barrelPos)
    {
        if (movementStateManager.currentState == movementStateManager.Idle) currentBloom = defaultBloomAngle;
        else if (movementStateManager.currentState == movementStateManager.Walk) currentBloom = defaultBloomAngle * walkBloomMultiplier;
        else if (movementStateManager.currentState == movementStateManager.Run) currentBloom = defaultBloomAngle * sprintBloomMultiplier;
        else if (movementStateManager.currentState == movementStateManager.Crouch)
        {
            if (movementStateManager.dir.magnitude == 0) currentBloom = defaultBloomAngle * crouchBloomMultiplier;
            else currentBloom = defaultBloomAngle * crouchBloomMultiplier * walkBloomMultiplier;
        }

        if (aimState.currentState == aimState.Aim) currentBloom *= adsBloomMultiplier;

        float randX=Random.Range(-currentBloom, currentBloom);
        float randY=Random.Range(-currentBloom, currentBloom);
        float randZ=Random.Range(-currentBloom, currentBloom);

        Vector3 randomRotation= new Vector3 (randX, randY, randZ);
        return barrelPos.localEulerAngles+randomRotation;
    }
}
