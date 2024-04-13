using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRecovery : State
{
    public EnemyRecovery(Enemy enemy, EnemyStateMachine enemyStateMachine, Rigidbody enemyRB, Animator enemyAnimator, EnemyNavigator navigatorScript, PlayerDetection playerDetectorScript) : base(enemy, enemyStateMachine, enemyRB, enemyAnimator, navigatorScript, playerDetectorScript)
    {

    }

    //Eepy time
    public override void Do()
    {
        timer += Time.deltaTime;
        if (timer > 2)
        {
            timer = 0;
            enemy.StateMachine.ChangeState(enemy.RoamState);
        }
    }

    public override void Enter()
    {
        enemyAnimator.SetBool("hasAttacked", true);
    }

    public override void Exit()
    {
        enemyAnimator.SetBool("hasAttacked", false);
    }

    public override void FixedDo()
    {
        base.FixedDo();
    }
}
