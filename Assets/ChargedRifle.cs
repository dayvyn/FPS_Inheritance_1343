using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class ChargedRifle : Gun
{
    [SerializeField] int damage = 0;
    [SerializeField] ParticleSystem charge;
    [SerializeField] ParticleSystem smoke;
    ParticleSystem.MainModule ma;
    CinemachineBrain camBrain;
    CinemachineVirtualCamera camActive;
    Color currentColor;
    void Start()
    {
       ma = charge.main;
        camBrain = FindObjectOfType<CinemachineBrain>();
        camActive = camBrain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();

    }

    public override void Update()
    {
        base.Update();
        if (Input.GetButtonUp("Fire1") && GetComponentInParent<FPSController>())
        {
            if (AttemptFire())
            {
                Fire();
            }
        }
    }

    public override bool AttemptFire()
    {
        if (!base.AttemptFire())
        { 
            return false;
        }
        if (damage < 5000)
        {
            damage++;
        }
        switch(damage)
        {
            case 1000:
                ChangeRed();
                CameraShake(damage);
                ParticleIncrease(damage);
                break;
            case 2000:
                ChangeBlue();
                CameraShake(damage);
                ParticleIncrease(damage);
                break;
            case 3000:
                ChangeGreen();
                CameraShake(damage);
                ParticleIncrease(damage);
                break;
            case 4000:
                ChangeYellow();
                CameraShake(damage);
                ParticleIncrease(damage);
                break;
            case 5000:
                ChangeWhite();
                CameraShake(damage);
                ParticleIncrease(damage);
                break;
        }

        return true;
    }

    void Fire()
    {
        var b = Instantiate(bulletPrefab, gunBarrelEnd.transform.position, gunBarrelEnd.rotation);
        b.GetComponent<EnergyBlast>().InitializeBlast(damage/2, 100, 2, damage/10, null, currentColor);
        anim.SetTrigger("shoot");
        elapsed = 0;
        ammo -= 1;
        damage = 0;
        ChangeClear();
        CameraShake(0);
        PlaySmoke();
        ParticleIncrease(0);
    }

    void ChangeRed()
    {
        ma.startColor = Color.red;
        currentColor = Color.red;
    }
    void ChangeBlue()
    {
        ma.startColor = Color.blue;
        currentColor = Color.blue;
    }
    void ChangeGreen()
    {
        ma.startColor = Color.green;
        currentColor = Color.green;
    }
    void ChangeYellow()
    {
        ma.startColor = Color.yellow;
        currentColor = Color.yellow;
    }
    void ChangeWhite()
    {
        ma.startColor = Color.white;
        currentColor = Color.white;
    }
    void ChangeClear()
    {
        ma.startColor = Color.clear;
        currentColor = Color.clear;
    }

    public void CameraShake(int num)
    {
        CinemachineBasicMultiChannelPerlin perlin = camActive.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = num / 1000;
    }
    void PlaySmoke()
    {
        smoke.Play();
    }
    public override void Unequip()
    {
        CinemachineBasicMultiChannelPerlin perlin = camActive.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = 0;
    }
    void ParticleIncrease(int particles)
    {
        ma.maxParticles = particles/10;
    }
}
