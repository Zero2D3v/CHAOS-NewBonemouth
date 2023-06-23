using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State
{
    protected Data_DeadState stateData;

    protected bool isAnimationFinished;

    //protected bool isDissolving = false;
    protected float fade;
    //protected Material material;


    public DeadState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_DeadState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        fade = entity.fade;
        //material = entity.material;
        entity.atsm.deadState = this;
        isAnimationFinished = false;

        entity.SetVelocity(0f);

        //GameObject.Instantiate(stateData.deathBloodParticle, entity.aliveGO.transform.position, stateData.deathBloodParticle.transform.rotation);
       // GameObject.Instantiate(stateData.deathChunkParticle, entity.aliveGO.transform.position, stateData.deathChunkParticle.transform.rotation);

       // entity.gameObject.SetActive(false);
        //disable die in damage
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
        //death animation
        
        entity.aliveGO.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        
    }

    public virtual void FinishDeath()
    {
        isAnimationFinished = true;
        //isDissolving = true;
        //Dissolve();
        //disolve shader

        entity.gameController.CheckLastManStanding();

        if (entity.gameController.lastManStanding)
        {
            entity.DropHealth();
        }
    }

}
