using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggro : State
{
    public EnemyAggro(Enemy enemy, EnemyStateMachine enemyStateMachine, Rigidbody enemyRB, Animator enemyAnimator, EnemyNavigator navigatorScript, PlayerDetection playerDetectorScript) : base(enemy, enemyStateMachine, enemyRB, enemyAnimator, navigatorScript, playerDetectorScript)
    {

    }

    public override void Do()
    {
        if (playerDetectorScript.withinRange == true && playerDetectorScript.withinAttackRange == true)
        {
            enemy.StateMachine.ChangeState(enemy.AttackState);
        }
        else if (playerDetectorScript.withinRange == true)
        {
            navigatorScript.ChasePlayer(playerDetectorScript);
        }
        else
        {
            enemy.StateMachine.ChangeState(enemy.RoamState);
        }
    }

    public override void Enter()
    {
        enemyAnimator.SetBool("foundPlayer", true);
    }

    public override void Exit()
    {
        enemyAnimator.SetBool("foundPlayer", false);
    }

    public override void FixedDo()
    {
       
    }
}
