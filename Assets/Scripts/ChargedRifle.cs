using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ChargedRifle : Gun
{
    [SerializeField] int damage = 0;
    [SerializeField] ParticleSystem charge;
    [SerializeField] ParticleSystem smoke;
    ParticleSystem.MainModule ma;
    CinemachineBrain camBrain;
    CinemachineVirtualCamera camActive;
    Color currentColor;
    Coroutine FireCoroutine = null;
    WaitForEndOfFrame _waitForEndOfFrame;
    ChargedRifle chargedRifleScript;
    int _maxAmmo = 1;

    public override UnityAction<Gun> Fired { get => base.Fired; set => base.Fired = value; }
    public override int maxAmmo { get { return _maxAmmo; } protected set => base.maxAmmo = _maxAmmo; }
    public override UnityAction<Gun> Reload { get => base.Reload; set => base.Reload = value; }

    protected override void Start()
    {
        base.Start();
        ma = charge.main;
        camBrain = FindObjectOfType<CinemachineBrain>();
        camActive = camBrain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        _waitForEndOfFrame = new WaitForEndOfFrame();
        chargedRifleScript = GetComponent<ChargedRifle>();
    }
    public override void Update()
    {
        base.Update();
    }

    private void OnEnable()
    {
        if(GetComponentInParent<FPSController>())
        ColorChange();
    }

    private void OnDisable()
    {
        FireCoroutine = null;
    }

    public override bool AttemptFire(InputAction.CallbackContext ctx)
    {
        if (!base.AttemptFire(ctx))
        { 
            return false;
        }
        OnHoldFire(ctx);
        return true;
    }

    public override void AddAmmo(int amount)
    {
        base.AddAmmo(amount);
        Reload.Invoke(chargedRifleScript);
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
        Fired.Invoke(chargedRifleScript);
        FireCoroutine = null;
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

    void OnHoldFire(InputAction.CallbackContext ctx)
    {
        if (ctx.started && FireCoroutine == null)
        {
            FireCoroutine = StartCoroutine(ChargeAttack());
        }
        return;

        IEnumerator ChargeAttack()
        {
            Charge();
            yield return _waitForEndOfFrame;
            if (ctx.canceled)
            {
                Fire();
                FireCoroutine = null;
                yield break;
            }
            yield return new WaitUntil(ctx.ReadValueAsButton);
            while (ctx.ReadValueAsButton())
            {
                Charge();
                yield return _waitForEndOfFrame;
            }
            yield return _waitForEndOfFrame;
            Fire();
            yield return _waitForEndOfFrame;

        }
    }
    void Charge()
    {
        if (damage < 5000)
        {
            damage++;
        }
        ColorChange();
    }

    void ColorChange()
    {
        switch (damage)
        {
            case 1000:
                ChangeRed();
                break;
            case 2000:
                ChangeBlue();
                break;
            case 3000:
                ChangeGreen();
                break;
            case 4000:
                ChangeYellow();
                break;
            case 5000:
                ChangeWhite();
                break;
        }
        CameraShake(damage);
        ParticleIncrease(damage);
    }
}
