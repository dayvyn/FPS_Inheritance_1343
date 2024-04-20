using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

// gun base class
public class Gun : MonoBehaviour
{
    protected FPSController player;

    // references
    [SerializeField] protected Transform gunBarrelEnd;
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected Animator anim;

    // stats
    public virtual int maxAmmo { get; protected set; }
    [SerializeField] protected float timeBetweenShots = 0.1f;
    [SerializeField] protected bool isAutomatic = false;

    // private variables
    public int ammo { get; protected set; }
    protected float elapsed = 0;
    public virtual UnityAction<Gun> Fired { get; set; }
    public virtual UnityAction<Gun> Reload { get; set; }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        ammo = maxAmmo;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        elapsed += Time.deltaTime;
    }

    public virtual void Equip(FPSController p)
    {
        player = p;
    }

    public virtual void Unequip() { }

    public bool AttemptAutomaticFire()
    {
        if (!isAutomatic)
            return false;

        return true;
    }

    public virtual bool AttemptFire(InputAction.CallbackContext ctx)
    {
        if (ammo <= 0)
        {
            return false;
        }

        if(elapsed < timeBetweenShots)
        {
            return false;
        }

        return true;
    }

    public virtual bool AttemptAltFire()
    {
        return false;
    }

    public virtual void AddAmmo(int amount)
    {
        ammo += amount;
        if (ammo > maxAmmo)
            ammo = maxAmmo;
    }
    public void SetAmmo(int value)
    {
        ammo = value;
    }
}
