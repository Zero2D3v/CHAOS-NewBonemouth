using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//enemy 1 player detected state
public class E1_PlayerDetectedState : PlayerDetectedState
{
    //reference enemy 1 script 
    private Enemy1 enemy;

    //constructor for state with necessary component references
    public E1_PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_PlayerDetected stateData, Enemy1 enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }
    //generated overrides so there if needed to add functionality in future
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
    //state machine logic override
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //if in range for close range action then change state to melee attck state
        if (performCloseRangeAction)
        {
            stateMachine.ChangeState(enemy.meleeAtackState);
        }
        //if in range for long range action then change state to charge state
        else if (peformLongRangeAction)
        {
            stateMachine.ChangeState(enemy.chargeState);
        }
        //if player out of maximum detection range then change state to look for player state
        else if (!isPlayerInMaxAgroRange)
        {
            stateMachine.ChangeState(enemy.lookForPlayerState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
