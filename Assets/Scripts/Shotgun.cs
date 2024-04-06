using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Shotgun : Gun
{
    public UnityAction active;

    public override bool AttemptFire()
    {
        if (!base.AttemptFire())
            return false;
        for (int i = 0; i < 5; i++)
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

        return true;
    }
}
