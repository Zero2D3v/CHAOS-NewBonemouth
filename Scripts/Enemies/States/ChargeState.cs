using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//charge state inherits from state
public class ChargeState : State
{
    //declare fields 
    protected Data_ChargeState stateData;

    protected bool isPlayerInMinAgroRange;
    protected bool isDetectingLedge;
    protected bool isDetectingWall;
    protected bool isChargeTimeOver;
    protected bool performCloseRangeAction;
    //inherited constructor with added components
    public ChargeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_ChargeState stateData) : base(entity, stateMachine, animBoolName)
    {
        //set instance to declared data
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        //state specific checks
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        isDetectingLedge = entity.CheckLedge();
        isDetectingWall = entity.CheckWall();

        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
    }

    public override void Enter()
    {
        base.Enter();

        //set values
        isChargeTimeOver = false;
        //apply charge movement using state specific data
        entity.SetVelocity(stateData.chargeSpeed);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //set value used for changing state
        if (Time.time >= startTime + stateData.chargeTime)
        {
            isChargeTimeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

    }
}
