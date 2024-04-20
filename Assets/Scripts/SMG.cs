using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class SMG : Gun
{
    Coroutine FireCoroutine;
    WaitForSeconds _waitForSeconds;
    SMG smgScript;
    int _maxAmmo = 30; 

    public override int maxAmmo { get { return _maxAmmo; } protected set => base.maxAmmo = _maxAmmo; }
    public override UnityAction<Gun> Reload { get => base.Reload; set => base.Reload = value; }
    protected override void Start()
    {
        base.Start();
        _waitForSeconds = new WaitForSeconds(.1f);
        smgScript = GetComponent<SMG>();
    }

    public override bool AttemptFire(InputAction.CallbackContext ctx)
    {
        if (!base.AttemptFire(ctx))
            return false;

        OnHoldFire(ctx);
        return true;
    }

    public override UnityAction<Gun> Fired { get => base.Fired; set => base.Fired = value; }
    void OnHoldFire(InputAction.CallbackContext ctx)
    {
        if (ctx.started && FireCoroutine == null)
        {
            FireCoroutine = StartCoroutine(AutomaticAttack());
        }
        return;

        IEnumerator AutomaticAttack()
        {
            Shoot();
            yield return _waitForSeconds; 
            if (ctx.canceled)
            {
                FireCoroutine = null;
                yield break;
            }
            yield return new WaitUntil(ctx.ReadValueAsButton);
            while (ctx.ReadValueAsButton() && ammo != 0)
            {
                Shoot();
                yield return _waitForSeconds;
            }
            yield return _waitForSeconds;
            FireCoroutine = null;
        }
    }

    public override void AddAmmo(int amount)
    {
        base.AddAmmo(amount);
        Reload.Invoke(smgScript);
    }

    void Shoot()
    {
        var b = Instantiate(bulletPrefab, gunBarrelEnd.transform.position, gunBarrelEnd.rotation);
        b.GetComponent<Projectile>().Initialize(1, 100, 2, 5, null); // version without special effect
        //b.GetComponent<Projectile>().Initialize(1, 100, 2, 5, DoThing); // version with special effect
        anim.SetTrigger("shoot");
        elapsed = 0;
        ammo -= 1;
        Fired.Invoke(smgScript);
    }
}
