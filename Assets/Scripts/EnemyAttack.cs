using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : State
{
    NavMeshAgent agent;
    float timer = 0;
    public EnemyAttack(Enemy enemy, EnemyStateMachine enemyStateMachine, Rigidbody enemyRB, Animator enemyAnimator, EnemyNavigator navigatorScript, PlayerDetection playerDetectorScript) : base(enemy, enemyStateMachine, enemyRB, enemyAnimator, navigatorScript, playerDetectorScript)
    {

    }
    public override void Do()
    {
        timer += Time.deltaTime;
        if(timer > 1f)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(agent.transform.position, out hit, 3, NavMesh.AllAreas))
            {
                agent.enabled = true;
                timer = 0;
                enemy.StateMachine.ChangeState(enemy.RecoveryState);
            }
            else
            {
                enemy.Respawn();
                agent.enabled = true;
                timer = 0;
                enemy.StateMachine.ChangeState(enemy.RecoveryState);
            }
        }
    }

    public override void Enter()
    {
        enemyAnimator.SetBool("isAttacking", true);
        agent = navigatorScript.GetComponent<NavMeshAgent>();
        agent.enabled = false;
        navigatorScript.AttackPlayer(playerDetectorScript);
    }

    public override void Exit()
    {
        enemyAnimator.SetBool("isAttacking", false);
    }

    public override void FixedDo()
    {
        
    }
}
