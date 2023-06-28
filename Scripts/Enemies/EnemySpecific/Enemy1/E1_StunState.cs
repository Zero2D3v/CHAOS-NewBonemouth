using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//enemy specific stun state
public class E1_StunState : StunState
{
    //declare enemy
    private Enemy1 enemy;
    //inherited constructor with added necessary components
    public E1_StunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_StunState stateData, Enemy1 enemy) : base(entity, stateMachine, animBoolName, stateData)
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
        if (isStunTimeOver)
        {
            if (performCloseRangeAction)
            {
                stateMachine.ChangeState(enemy.meleeAtackState);
            }
            else if (isPlayerInMinAggroRange)
            {
                stateMachine.ChangeState(enemy.chargeState);
            }
            else
            {
                //if hit from behind, immediately turn to face player
                enemy.lookForPlayerState.SetTurnImmediately(true);
                stateMachine.ChangeState(enemy.lookForPlayerState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
