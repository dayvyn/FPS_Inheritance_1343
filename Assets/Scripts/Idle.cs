using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : State
{
    float timeInState;
    public Idle(Enemy enemy, EnemyStateMachine enemyStateMachine, Rigidbody enemyRB, Animator enemyAnimator, EnemyNavigator navigatorScript, PlayerDetection playerDetectorScript) : base(enemy, enemyStateMachine, enemyRB, enemyAnimator, navigatorScript, playerDetectorScript)
    {

    }

    public override void Enter()
    {
        enemyRB.velocity = Vector3.zero;
    }

    public override void Do()
    {
        timeInState += Time.deltaTime;
        if (timeInState > 2)
        {
            enemy.StateMachine.ChangeState(enemy.RoamState);
            timeInState = 0;
        }
    }

    public override void Exit()
    {

    }

    public override void FixedDo()
    {
        enemyRB.velocity = new Vector3(0, enemyRB.velocity.y, 0);
    }
}
