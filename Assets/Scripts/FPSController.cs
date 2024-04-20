using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class FPSController : MonoBehaviour
{
    // references
    CharacterController controller;
    [SerializeField] GameObject cam;
    [SerializeField] Transform gunHold;
    [SerializeField] Gun initialGun;

    // stats
    [SerializeField] float movementSpeed = 2.0f;
    [SerializeField] float lookSensitivityX = 1.0f;
    [SerializeField] float lookSensitivityY = 1.0f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float jumpForce = 10;
    Vector2 movement;

    // private variables
    Vector3 origin;
    Vector3 velocity;
    bool grounded;
    float xRotation;
    List<Gun> equippedGuns = new List<Gun>();
    int gunIndex = 0;
    public float health { get; private set; }

    PlayerInputHandler ih;
    
    bool damaged = false;

    public UnityAction<Gun> WeaponSwapID;
    public UnityAction<float> Damaged;
    public UnityAction<float> HealthLoaded;

    // properties
    public GameObject Cam { get { return cam; } }
    public Gun currentGun { get; private set; }

    private void Awake()
    {
       ih = GetComponent<PlayerInputHandler>();
    }
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        // start with a gun
        if(initialGun != null)
            AddGun(initialGun);

        origin = transform.position;
        health = 1;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        if (ih.controls != null)
        {
            Look();
        }

        // always go back to "no velocity"
        // "velocity" is for movement speed that we gain in addition to our movement (falling, knockback, etc.)
        Vector3 noVelocity = new Vector3(0, velocity.y, 0);
        velocity = Vector3.Lerp(velocity, noVelocity, 5 * Time.deltaTime);
    }

    private void OnEnable()
    {
        if (ih.controls != null)
        {
            ih.controls.Player.Jump.performed += OnJump;
            ih.controls.Player.Fire.started += FireGun;
            ih.controls.Player.SwitchGun.performed += HandleSwitchGun;
            ih.controls.Player.Reload.performed += ReloadGun;
        }
    }

    private void OnDisable()
    {
        if (ih.controls != null)
        {
            ih.controls.Player.Jump.performed -= OnJump;
            ih.controls.Player.Fire.started -= FireGun;
            ih.controls.Player.SwitchGun.performed -= HandleSwitchGun;
            ih.controls.Player.Reload.performed -= ReloadGun;
        }
    }

    void Movement()
    {
        grounded = controller.isGrounded;

        if(grounded && velocity.y < 0)
        {
            velocity.y = -1;// -0.5f;
        }
        if (ih.controls != null)
        {
            movement = ih.controls.Player.Move.ReadValue<Vector2>();
        }

        Vector3 move = transform.right * movement.x + transform.forward * movement.y;
        controller.Move(move * movementSpeed * (GetSprint() ? 2 : 1) * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void Look()
    {
        Vector2 looking = ih.controls.Player.Look.ReadValue<Vector2>();

        float lookX = looking.x * lookSensitivityX * Time.deltaTime;
        float lookY = looking.y * lookSensitivityY * Time.deltaTime;

        xRotation -= lookY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * lookX);
    }

    void HandleSwitchGun(InputAction.CallbackContext ctx)
    {
        if (equippedGuns.Count == 0)
            return;

        float posOrNeg = ctx.ReadValue<float>();

        if(posOrNeg > 0)
        {
            gunIndex++;
            if (gunIndex > equippedGuns.Count - 1)
                gunIndex = 0;

            EquipGun(equippedGuns[gunIndex]);
            WeaponSwapID.Invoke(currentGun);
        }

        else if (posOrNeg < 0)
        {
            gunIndex--;
            if (gunIndex < 0)
                gunIndex = equippedGuns.Count - 1;

            EquipGun(equippedGuns[gunIndex]);
            WeaponSwapID.Invoke(currentGun);
        }
    }

    void FireGun(InputAction.CallbackContext ctx)
    {
        // don't fire if we don't have a gun
        if (currentGun == null)
        {
            return;
        }
        currentGun?.AttemptFire(ctx);
    }

    public void EquipGun(Gun g)
    {
        // disable current gun, if there is one
        currentGun?.Unequip();
        currentGun?.gameObject.SetActive(false);

        // enable the new gun
        g.gameObject.SetActive(true);
        g.transform.parent = gunHold;
        g.transform.localPosition = Vector3.zero;
        currentGun = g;
        WeaponSwapID.Invoke(currentGun);

        g.Equip(this);
    }

    // public methods

    public void AddGun(Gun g)
    {
        // add new gun to the list
        equippedGuns.Add(g);

        // our index is the last one/new one
        gunIndex = equippedGuns.Count - 1;

        // put gun in the right place
        EquipGun(g);
    }

    public void IncreaseAmmo(int amount)
    {
        currentGun.AddAmmo(amount);
    }

    public void Respawn()
    {
        transform.position = origin;
    }

    // Input methods

    public bool GetHoldFire()
    {
        return Input.GetButton("Fire1");
    }

    public bool GetPressAltFire()
    {
        return Input.GetButtonDown("Fire2");
    }

    Vector2 GetPlayerMovementVector()
    {
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    Vector2 GetPlayerLook()
    {
        return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    bool GetSprint()
    {
        return Input.GetButton("Sprint");
    }

    // Collision methods

    // Character Controller can't use OnCollisionEnter :D thanks Unity
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.layer == 7 && !damaged)
        {
            DecreasePlayerHealth();
            var collisionPoint = hit.collider.ClosestPoint(transform.position);
            var knockbackAngle = (transform.position - collisionPoint).normalized;
            velocity = (20 * knockbackAngle);
        }

        if (hit.gameObject.GetComponent <KillZone>())
        {
            Respawn();
        }
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (grounded)
        {
            velocity.y += Mathf.Sqrt(jumpForce * -1 * gravity);
        }
    }
    public void ReloadGun(InputAction.CallbackContext ctx)
    {
        currentGun?.AddAmmo(currentGun.maxAmmo);
    }

    void DecreasePlayerHealth()
    {
        health -= .1f;
        Damaged.Invoke(health);
        StartCoroutine(CanBeDamaged());
    }
    IEnumerator CanBeDamaged()
    {
        if (!damaged)
        {
            damaged = true;
            yield return new WaitForSeconds(.5f);
            damaged = false;
        }
    }

    public void SetHealth(float amount)
    {
        HealthLoaded.Invoke(amount);
    }
}
