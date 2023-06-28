using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//base dead state inherits from state class
public class DeadState : State
{
    //declare components
    protected Data_DeadState stateData;

    protected bool isAnimationFinished;

    protected float fade;
    //inherited constructor with added necessary components
    public DeadState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_DeadState stateData) : base(entity, stateMachine, animBoolName)
    {
        //this instance to declared data
        this.stateData = stateData;
    }
    //base functionality to override
    public override void DoChecks()
    {
        base.DoChecks();
    }
    //handles dissolve on death shader and animations
    public override void Enter()
    {
        base.Enter();

        fade = entity.fade;
        entity.atsm.deadState = this;
        isAnimationFinished = false;

        entity.SetVelocity(0f);
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

    public virtual void TriggerDeath()
    {
        //rotate enemy dying on its back
        entity.aliveGO.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
    }

    public virtual void FinishDeath()
    {
        //update information on death
        isAnimationFinished = true;
        entity.gameController.CheckLastManStanding();
        //drop health if last enemy of type
        if (entity.gameController.lastManStanding)
        {
            entity.DropHealth();
        }
    }

}
