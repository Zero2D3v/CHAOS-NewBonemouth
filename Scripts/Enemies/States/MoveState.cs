using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//move state inherits from state
public class MoveState : State
{
    //time save notes for when writing states
    //ctrl + . = generate constructor

    //declare fields
    protected Data_MoveState stateData;

    protected bool isDetectingWall;
    protected bool isDetectingLedge;
    protected bool isPlayerInMinAgroRange;
    //inherited constructor with added components
    public MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_MoveState stateData) : base(entity, stateMachine, animBoolName)
    {
        //set instance to declared data
        this.stateData = stateData;
    }
    
    public override void Enter()
    {
        base.Enter();
        //apply movement using state specifc data declared earlier
        entity.SetVelocity(stateData.movementSpeed);
             
    }

    public override void DoChecks()
    {
        base.DoChecks();

        //state specific checks
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        isDetectingLedge = entity.CheckLedge();
        isDetectingWall = entity.CheckWall();
    }

    public override void Exit()
    {
        base.Exit();
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
