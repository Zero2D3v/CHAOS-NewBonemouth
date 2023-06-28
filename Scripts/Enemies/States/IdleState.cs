using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//base idle state inherits from state
public class IdleState : State
{
    //declare fields
    protected Data_IdleState stateData;

    protected bool flipAfterIdle;
    protected bool isIdleTimeOver;
    protected bool isPlayerInMinAgroRange;

    protected float idleTime;
    //inherited constructor with added components
    public IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_IdleState stateData) : base(entity, stateMachine, animBoolName)
    {
        //set instance to declared data
        this.stateData = stateData;
    }
    //base functionality
    public override void DoChecks()
    {
        base.DoChecks();
        //state specific check override
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
    }

    public override void Enter()
    {
        base.Enter();
        //stfop movement
        entity.SetVelocity(0f);
        //record value
        isIdleTimeOver = false;
        //initialise function
        SetRandomIdleTime();
    }

    public override void Exit()
    {
        base.Exit();
        //flip if value true
        if (flipAfterIdle)
        {
            entity.Flip();
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        //idle time random
        if(Time.time >=startTime + idleTime)
        {
            isIdleTimeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

    }

    public void SetFlipAfterIdle(bool flip)
    {
        flipAfterIdle = flip;
    }

    private void SetRandomIdleTime()
    {
        idleTime = Random.Range(stateData.minIdleTime, stateData.maxIdleTime);
    }
}
