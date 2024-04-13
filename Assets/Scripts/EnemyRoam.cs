using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoam : State
{
    public EnemyRoam(Enemy enemy, EnemyStateMachine enemyStateMachine, Rigidbody enemyRB, Animator enemyAnimator, EnemyNavigator navigatorScript, PlayerDetection playerDetectorScript) : base(enemy, enemyStateMachine, enemyRB, enemyAnimator, navigatorScript, playerDetectorScript)
    {

    }

    public override void Do()
    {
        if (playerDetectorScript.withinAttackRange == true)
        {
            enemy.StateMachine.ChangeState(enemy.AttackState);
        }
        else if (playerDetectorScript.withinRange == true)
        {
            enemy.StateMachine.ChangeState(enemy.AggroState);
        }
        else if (navigatorScript.EndOfPath() == true)
        {
            enemy.StateMachine.ChangeState(enemy.IdleState);
        }
    }

    public override void Enter()
    {
        enemyAnimator.SetBool("isRoaming", true);
        navigatorScript.MoveAgent();
    }

    public override void Exit()
    {
        enemyAnimator.SetBool("isRoaming", false);
    }

    public override void FixedDo()
    {
        base.FixedDo();
    }
}
