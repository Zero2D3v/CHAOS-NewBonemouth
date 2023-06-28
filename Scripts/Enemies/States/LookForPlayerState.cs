using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//look for player state inherits from state
public class LookForPlayerState : State
{
    //declare fields 
    protected Data_LookForPlayerState stateData;

    protected bool turnImmediately;
    protected bool isPlayerInMinAggroRange;
    protected bool isAllTurnsDone;
    protected bool isAllTurnsTimeDone;

    protected float lastTurnTime;

    protected int amountOfTurnsDone;
    //inherited constructor with added components
    public LookForPlayerState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_LookForPlayerState stateData) : base(entity, stateMachine, animBoolName)
    {
        //set instance to declared data
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        //state specific checks override
        isPlayerInMinAggroRange = entity.CheckPlayerInMinAgroRange();
    }

    public override void Enter()
    {
        base.Enter();
        //set values
        isAllTurnsDone = false;
        isAllTurnsTimeDone = false;

        lastTurnTime = startTime;
        amountOfTurnsDone = 0;
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

        //look around if value set to true
        if (turnImmediately)
        {
            entity.Flip();
            lastTurnTime = Time.time;
            amountOfTurnsDone++;
            turnImmediately = false;
        }
        //keep looking if still time left (in data)
        else if (Time.time >= lastTurnTime + stateData.timeBetweenTurns && !isAllTurnsDone)
        {
            entity.Flip();
            lastTurnTime = Time.time;
            amountOfTurnsDone++;
        }
        //check against value based on state data
        if (amountOfTurnsDone >= stateData.amountOfTurns)
        {
            isAllTurnsDone = true;
        }
        //set value
        if (Time.time >= lastTurnTime + stateData.timeBetweenTurns && isAllTurnsDone)
        {
            isAllTurnsTimeDone = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    //bool for turn immediately behaviour
    public void SetTurnImmediately(bool flip)
    {
        turnImmediately = flip;
    }
}
