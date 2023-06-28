using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//enemy specific dodge state
public class E2_DodgeState : DodgeState
{
    //declare enemy
    private Enemy2 enemy;
    //inherited constructor with added necessary components
    public E2_DodgeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_DodgeState stateData, Enemy2 enemy) : base(entity, stateMachine, animBoolName, stateData)
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
        if (isDodgeOver)
        {
            if(isPlayerInMaxAggroRange && performCloseRangeAction)
            {
                stateMachine.ChangeState(enemy.meleeAttackState);
            }
            else if(isPlayerInMaxAggroRange && !performCloseRangeAction)
            {
                stateMachine.ChangeState(enemy.rangedAttackState);
            }
            else if (!isPlayerInMaxAggroRange)
            {
                stateMachine.ChangeState(enemy.lookForPlayerState);

                //optional:TODO if enemy dodged past player, turn immediately etc
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
