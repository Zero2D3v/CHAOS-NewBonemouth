using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//enemy specific charge state
public class E1_ChargeState : ChargeState
{
    //declare enemy
    private Enemy1 enemy;
    //inherited constructor with added necessary components
    public E1_ChargeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_ChargeState stateData, Enemy1 enemy) : base(entity, stateMachine, animBoolName, stateData)
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

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //changes state based on inherited values
        if (performCloseRangeAction)
        {
            stateMachine.ChangeState(enemy.meleeAtackState);
        }
        else if (!isDetectingLedge || isDetectingWall)
        {
            stateMachine.ChangeState(enemy.lookForPlayerState);
        }

        else if (isChargeTimeOver)
        {
           
            if (isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(enemy.playerDetectedState);
            }
            else
            {
                stateMachine.ChangeState(enemy.lookForPlayerState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
