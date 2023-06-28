using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//base player detected state, inherits from base state class
public class PlayerDetectedState : State
{
    //reference to scriptable object data
    protected Data_PlayerDetected stateData;
    //bools for in class references to entity bools
    protected bool isPlayerInMinAgroRange;
    protected bool isPlayerInMaxAgroRange;
    protected bool peformLongRangeAction;
    protected bool performCloseRangeAction;
    //constructor for state with necessary components
    public PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_PlayerDetected stateData) : base(entity, stateMachine, animBoolName)
    {
        //sets state data reference
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        //call state relevent checks
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();

        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
        
    }

    public override void Enter()
    {
        base.Enter();

        //if in player detected state then not charging
        peformLongRangeAction = false;
        //stop movement
        entity.SetVelocity(0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //if in state for long enough, set conditions to charge
        if (Time.time >= startTime + stateData.longRangeActionTime)
        {
            peformLongRangeAction = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

    }
}
