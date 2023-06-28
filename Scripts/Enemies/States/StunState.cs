using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ranged attack state inherits from state
public class StunState : State
{
    //declare fields
    protected Data_StunState stateData;

    protected bool isStunTimeOver;
    protected bool isGrounded;
    protected bool isMovementStopped;
    protected bool performCloseRangeAction;
    protected bool isPlayerInMinAggroRange;
    //inherited constructor with added components
    public StunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_StunState stateData) : base(entity, stateMachine, animBoolName)
    {
        //set instance to declared data
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        //state specific checks
        isGrounded = entity.CheckGround();
        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
        isPlayerInMinAggroRange = entity.CheckPlayerInMinAgroRange();
    }

    public override void Enter()
    {
        base.Enter();
        //set values
        isStunTimeOver = false;
        isMovementStopped = false;
        //check for player crit and apply increased knockback if so
        if (entity.PRN.RadNum != 20)
        {
            entity.SetVelocity(stateData.stunKnockbackSpeed, stateData.stunKnockbackAngle, entity.lastDamageDirection);
        }
        else
        {
            entity.SetVelocity(stateData.stunCritKnocbackSpeed, stateData.stunKnockbackAngle, entity.lastDamageDirection);
        }
        
    }

    public override void Exit()
    {
        base.Exit();

        //reset value
        entity.ResetStunResistance();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //set value used for state change
        if(Time.time >= startTime + stateData.stunTime)
        {
            isStunTimeOver = true;
        }
        //stop knockback velocity if knockback timer finished
        if(isGrounded && Time.time >= startTime + stateData.stunKnockbackTime && !isMovementStopped)
        {
            isMovementStopped = true;
            entity.SetVelocity(0f);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
