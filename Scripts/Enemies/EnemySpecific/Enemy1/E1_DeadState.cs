using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//enemy specific dead state
public class E1_DeadState : DeadState
{
    //declare enemy
    protected Enemy1 enemy;
    //inherited constructor with added necessary components
    public E1_DeadState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_DeadState stateData, Enemy1 enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        //this instance set to declared enemy
        this.enemy = enemy;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FinishDeath()
    {
        base.FinishDeath();
        //reduce enemy type 1 count
        enemy.gameController.monoRemaining -=1;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
