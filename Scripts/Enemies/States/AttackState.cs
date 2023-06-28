using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//base attack state inherits from base state class
public class AttackState : State
{
    //declare fields
    protected Transform attackPosition;
    protected bool isAnimationFinished;
    protected bool isPlayerInMinAggroRange;
    //constructor with necessary components
    public AttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition) : base(entity, stateMachine, animBoolName)
    {
        //set transform in constructor to above transform declared
        this.attackPosition = attackPosition;
    }
//state relevant checks
    public override void DoChecks()
    {
        base.DoChecks();
        isPlayerInMinAggroRange = entity.CheckPlayerInMinAgroRange();
    }
    //enter state functionality
    public override void Enter()
    {
        base.Enter();
        //set animation to state machine script
        entity.atsm.attackState = this;
        isAnimationFinished = false;
        entity.SetVelocity(0f);
    }
    //state functionality for if overrides needed
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

    public virtual void TriggerAttack()
    {

    }
    //called by animation event
    public virtual void FinishAttack()
    {
        isAnimationFinished = true;

    }
}
