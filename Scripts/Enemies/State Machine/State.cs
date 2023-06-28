using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//base sate class provides basic shared functionality for different states
public class State
{
    //declare base entity class that enemys will inherit from, and state machine script
    protected FiniteStateMachine stateMachine;
    protected Entity entity;
    //declare fields
    public float startTime { get; protected set; }

    protected string animBoolName;
    //base constructor that states will inherit from
    public State (Entity entity, FiniteStateMachine stateMachine, string animBoolName)
    {
        //set component references to this instance on object
        this.entity = entity;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        //on enter record start time of state and start animation relevant to state
        startTime = Time.time;
        entity.anim.SetBool(animBoolName, true);
        //do state relevant checks
        DoChecks();
    }

    public virtual void Exit()
    {
        //on exit of state stop current state specififc animation ready for next state
        entity.anim.SetBool(animBoolName, false);
    }
    //base functionality to be added overrrides
    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    public virtual void DoChecks()
    {

    }
}
