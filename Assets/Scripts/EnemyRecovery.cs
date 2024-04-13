using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRecovery : State
{
    float time = 0;
    public EnemyRecovery(Enemy enemy, EnemyStateMachine enemyStateMachine, Rigidbody enemyRB, Animator enemyAnimator, EnemyNavigator navigatorScript, PlayerDetection playerDetectorScript) : base(enemy, enemyStateMachine, enemyRB, enemyAnimator, navigatorScript, playerDetectorScript)
    {

    }

    public override void Do()
    {
        time += Time.deltaTime;
        if (time > 2)
        {
            time = 0;
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
