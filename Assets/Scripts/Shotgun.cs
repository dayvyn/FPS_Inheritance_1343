using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Shotgun : Gun
{
    Shotgun shotgunScript;
    int _maxAmmo = 5;
    public override UnityAction<Gun> Fired { get => base.Fired;  set => base.Fired = value; }
    public override int maxAmmo { get { return _maxAmmo; } protected set => base.maxAmmo = _maxAmmo; }
    public override UnityAction<Gun> Reload { get => base.Reload; set => base.Reload = value; }
    protected override void Start()
    {
        base.Start();
        shotgunScript = GetComponent<Shotgun>();
    }

    public override bool AttemptFire(InputAction.CallbackContext ctx)
    {
        if (!base.AttemptFire(ctx))
            return false;
        for (int i = 0; i < 8; i++)
        {
            Vector3 spread;
            spread.x = Random.Range(-i, i);
            spread.y = Random.Range(-i, i);
            spread.z = 0;
            Quaternion spreadAngle;
            spreadAngle = Quaternion.Euler(spread);
            var b = Instantiate(bulletPrefab, gunBarrelEnd.transform.position, gunBarrelEnd.transform.rotation * spreadAngle);
            b.GetComponent<ShotgunPellet>().InitializeShotgun(3, 100, 1.5f, 5, null);
        }
        //b.GetComponent<Projectile>().Initialize(1, 100, 2, 5, DoThing); // version with special effect

        anim.SetTrigger("shoot");
        elapsed = 0;
        ammo -= 1;
        Fired.Invoke(shotgunScript);

        return true;
    }

    public override void AddAmmo(int amount)
    {
        base.AddAmmo(amount);
        Reload.Invoke(shotgunScript);
    }
}
