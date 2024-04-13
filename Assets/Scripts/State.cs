using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    protected Enemy enemy;
    protected EnemyStateMachine enemyStateMachine;
    protected Rigidbody enemyRB;
    protected Animator enemyAnimator;
    protected EnemyNavigator navigatorScript;
    protected PlayerDetection playerDetectorScript;

    public virtual void Enter() { }

    public virtual void Do() { }

    public virtual void FixedDo() { }

    public virtual void Exit() { }


    public State(Enemy enemy, EnemyStateMachine enemyStateMachine, Rigidbody enemyRB, Animator enemyAnimator, EnemyNavigator navigatorScript, PlayerDetection playerDetectorScript)
    {
        this.enemy = enemy;
        this.enemyStateMachine = enemyStateMachine;
        this.enemyRB = enemyRB;
        this.enemyAnimator = enemyAnimator;
        this.navigatorScript = navigatorScript;
        this.playerDetectorScript = playerDetectorScript;
    }
}
