using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Fire Rate")]
    [SerializeField] float fireRate;
    float fireRateTimer;
    [SerializeField] bool semiAuto;

    [Header("Bullet Properties")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform barrelPos;
    [SerializeField] float bulletVelocity;
    [SerializeField] float bulletsPerShot;
    AimStateManager aim;

    [SerializeField] AudioClip gunShot;
    AudioSource audioSource;
    WeaponAmmo weaponAmmo;
    WeaponBloom weaponBloom;
    ActionStateManager actions;
    WeaponRecoil weaponRecoil;

    Light muzzleFlashLight;
    ParticleSystem muzzleFlashParticles;
    float lightIntensity;
    [SerializeField] float lightReturnSpeed =2;

    private void Start()
    {
        weaponRecoil=GetComponent<WeaponRecoil>();
        audioSource = GetComponent<AudioSource>();
        aim= GetComponentInParent<AimStateManager>();
        weaponAmmo=GetComponent<WeaponAmmo>();
        weaponBloom=GetComponent<WeaponBloom>();
        actions = GetComponentInParent<ActionStateManager>();   
        muzzleFlashLight = GetComponentInChildren<Light>();
        lightIntensity=muzzleFlashLight.intensity;
        muzzleFlashLight.intensity = 0;
        muzzleFlashParticles = GetComponentInChildren<ParticleSystem>();
        fireRateTimer=fireRate;

    }
    private void Update()
    {
        if (ShouldFire()) Fire();
        muzzleFlashLight.intensity=Mathf.Lerp(muzzleFlashLight.intensity,0,lightReturnSpeed*Time.deltaTime);
        
    }
     
    bool ShouldFire()
    {
        fireRateTimer += Time.deltaTime;
        if (fireRateTimer < fireRate) return false;
        if(weaponAmmo.currentAmmo==0)return false;
        if (actions.currentState == actions.Reload) return false;
        if(semiAuto && Input.GetKeyDown(KeyCode.Mouse0)) return true;
        if(!semiAuto && Input.GetKey(KeyCode.Mouse0)) return true;
        return false;
       
    }

    void Fire()
    {
        fireRateTimer = 0;
        weaponAmmo.currentAmmo--;

        barrelPos.LookAt(aim.aimPos);
        barrelPos.localEulerAngles = weaponBloom.BloomAngle(barrelPos);

        audioSource.PlayOneShot(gunShot);
        weaponRecoil.TriggerRecoil();
        TriggerMuzzleFlash();
 
        for (int i = 0; i < bulletVelocity; i++)
        {
            GameObject currentBullet = Instantiate(bullet, barrelPos.position, barrelPos.rotation);
            Rigidbody rb = currentBullet.GetComponent<Rigidbody>();
            rb.AddForce(barrelPos.forward * bulletVelocity, ForceMode.Impulse);
        }
    }
    void TriggerMuzzleFlash()
    {
        muzzleFlashParticles.Play();
        muzzleFlashLight.intensity=lightIntensity;
    }

}
