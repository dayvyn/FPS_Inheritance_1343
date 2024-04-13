using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    public Rigidbody Rigidbody { get; private set; }
    Vector3 origin;

    public EnemyStateMachine StateMachine { get; private set; }
    public Idle IdleState { get; private set; }
    public EnemyRoam RoamState { get; private set; }
    public EnemyAttack AttackState { get; private set; }
    public EnemyRecovery RecoveryState { get; private set; }
    public Animator zombieAnimator { get; private set; }
    public EnemyNavigator navScript { get; private set; }
    public EnemyAggro AggroState { get; private set; }
    public PlayerDetection playerDetector { get; private set; }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        zombieAnimator = GetComponent<Animator>();
        navScript = GetComponent<EnemyNavigator>();
        playerDetector = FindObjectOfType<FPSController>().GetComponent<PlayerDetection>();
        StateMachine = new EnemyStateMachine();
        IdleState = new Idle(this, StateMachine, Rigidbody, zombieAnimator, navScript, playerDetector);
        RoamState = new EnemyRoam(this, StateMachine, Rigidbody, zombieAnimator, navScript, playerDetector);
        AttackState = new EnemyAttack(this, StateMachine, Rigidbody, zombieAnimator, navScript, playerDetector);
        AggroState = new EnemyAggro(this, StateMachine, Rigidbody, zombieAnimator, navScript, playerDetector);
        RecoveryState = new EnemyRecovery(this, StateMachine, Rigidbody, zombieAnimator, navScript, playerDetector);
    }

    // Start is called before the first frame update
    void Start()
    { 
        origin = transform.position;
        StateMachine.Initialize(IdleState);
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine.currentState.Do();
        StateMachine.currentState.FixedDo();
    }

    public void ApplyKnockback(Vector3 knockback)
    {
        Rigidbody.AddForce(knockback, ForceMode.Impulse);
    }

    public void Respawn()
    {
        transform.position = origin;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        Debug.Log("You have been hit by a Zombie!");
    }
}
