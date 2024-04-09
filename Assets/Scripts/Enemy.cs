using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum States
{
    IDLE,
    WANDER,
    CHASE,
    ATTACK,
    UP,
    DOWN
}


public class Enemy : State
{
    public Rigidbody Rigidbody { get; private set; }
    Vector3 origin;
    StateManager stateMachineClass;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        origin = transform.position;
        stateMachineClass.Update();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void ApplyKnockback(Vector3 knockback)
    {
        Rigidbody.AddForce(knockback, ForceMode.Impulse);
    }

    public void Respawn()
    {
        transform.position = origin;
    }

    public void UpdateIdle()
    {
        Rigidbody.velocity = Vector3.zero;
    }

    public void UpdateDown()
    {
        Rigidbody.velocity = Vector3.down;
    }
}
