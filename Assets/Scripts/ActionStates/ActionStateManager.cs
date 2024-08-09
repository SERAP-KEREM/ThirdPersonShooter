using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ActionStateManager : MonoBehaviour
{
    [HideInInspector] public ActionBaseState currentState;

    public ReloadState Reload =new ReloadState();
    public DefaultState Default=new DefaultState();

    public GameObject currentWeapon;
    [HideInInspector] public WeaponAmmo weaponAmmo;
    AudioSource audioSource;

    [HideInInspector] public Animator anim;

    public MultiAimConstraint rHandAim;
    public TwoBoneIKConstraint lHandIK;

    private void Start()
    {
        SwitchState(Default);
        weaponAmmo=currentWeapon.GetComponent<WeaponAmmo>();
        audioSource=currentWeapon.GetComponent<AudioSource>();
        anim=GetComponent<Animator>();  
    }
    private void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(ActionBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
    public void WeaponReloaded()
    {
        weaponAmmo.Reload();
        SwitchState(Default);
    }

    public void MagOut()
    {
        audioSource.PlayOneShot(weaponAmmo.magOutSound);
    }

    public void MagIn() { 
        audioSource.PlayOneShot(weaponAmmo.magInSound);
    }

    public void ReleaseSlide()
    {
        audioSource.PlayOneShot(weaponAmmo.releaseSlideSound);

    }

}
