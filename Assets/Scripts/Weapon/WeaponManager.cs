﻿using System.Collections;
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
    ActionStateManager actions;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        aim= GetComponentInParent<AimStateManager>();
        weaponAmmo=GetComponent<WeaponAmmo>();
        actions = GetComponentInParent<ActionStateManager>();   
        fireRateTimer=fireRate;

    }
    private void Update()
    {
        if (ShouldFire()) Fire();
     //   Debug.Log(weaponAmmo.currentAmmo);
        
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
        barrelPos.LookAt(aim.aimPos);
        audioSource.PlayOneShot(gunShot);
        weaponAmmo.currentAmmo--;
        for (int i = 0; i < bulletVelocity; i++)
        {
            GameObject currentBullet = Instantiate(bullet, barrelPos.position, barrelPos.rotation);
            Rigidbody rb = currentBullet.GetComponent<Rigidbody>();
            rb.AddForce(barrelPos.forward * bulletVelocity, ForceMode.Impulse);
        }
    }

}
