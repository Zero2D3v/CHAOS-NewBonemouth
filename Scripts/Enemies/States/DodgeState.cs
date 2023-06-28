using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//dodge state inherits from state
public class DodgeState : State
{
    //declare fields 
    protected Data_DodgeState stateData;

    protected bool performCloseRangeAction;
    protected bool isPlayerInMaxAggroRange;
    protected bool isGrounded;
    protected bool isDodgeOver;
    //inherited constructor with added components
    public DodgeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_DodgeState stateData) : base(entity, stateMachine, animBoolName)
    {
        //set instance to declared data
        this.stateData = stateData;
    }
    //base functionality
    public override void DoChecks()
    {
        base.DoChecks();
        //state specific checks override
        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
        isPlayerInMaxAggroRange = entity.CheckPlayerInMaxAgroRange();
        isGrounded = entity.CheckGround();

    }

    public override void Enter()
    {
        base.Enter();
        //set value
        isDodgeOver = false;
        //dodge
        entity.SetVelocity(stateData.dodgeSpeed, stateData.dodgeAngle, -entity.facingDirection);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        //update value for state change
        if(Time.time >= startTime + stateData.dodgeTime && isGrounded)
        {
            isDodgeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
