using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Blaster : Gun
{
    int _maxAmmo = 10;
    Gun blasterScript;
    protected override void Start()
    {
        base.Start();
        blasterScript = GetComponent<Blaster>();
        ammo = maxAmmo;
    }

    public override int maxAmmo { get { return _maxAmmo; } protected set => base.maxAmmo = _maxAmmo; }

    public override UnityAction<Gun> Fired { get => base.Fired; set => base.Fired = value; }

    public override UnityAction<Gun> Reload { get => base.Reload; set => base.Reload = value; }

    public override bool AttemptFire(InputAction.CallbackContext ctx)
    {
        if (!base.AttemptFire(ctx))
            return false;

        var b = Instantiate(bulletPrefab, gunBarrelEnd.transform.position, gunBarrelEnd.rotation);
        b.GetComponent<Projectile>().Initialize(3, 100, 2, 5, null); // version without special effect
        //b.GetComponent<Projectile>().Initialize(1, 100, 2, 5, DoThing); // version with special effect

        anim.SetTrigger("shoot");
        elapsed = 0;
        ammo -= 1;
        Fired.Invoke(blasterScript);

        return true;
    }

    public override void AddAmmo(int amount)
    {
        base.AddAmmo(amount);
        Reload.Invoke(blasterScript);
    }

    // example function, make hit enemy fly upward
    void DoThing(HitData data)
    {
        Vector3 impactLocation = data.location;

        var colliders = Physics.OverlapSphere(impactLocation, 1);
        foreach(var c in colliders)
        {
            if(c.GetComponent<Rigidbody>())
            {
                c.GetComponent<Rigidbody>().AddForce(Vector3.up * 20, ForceMode.Impulse);
            }
        }
    }
}
