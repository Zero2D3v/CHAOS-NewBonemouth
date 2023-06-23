using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Enemy2 : Entity
{
    public E2_MoveState moveState { get; private set; }
    public E2_IdleState idleState { get; private set; }
    public E2_PlayerDetectedState playerDetectedState { get; private set; }
    public E2_MeleeAttackState meleeAttackState { get; private set; }
    public E2_LookForPlayerState lookForPlayerState { get; private set; }
    public E2_StunState stunState { get; private set; }
    public E2_DeadState deadState { get; private set; }
    public E2_RangedAttackState rangedAttackState { get; private set; }

    public E2_DodgeState dodgeState { get; private set; }

    [SerializeField]
    private Data_MoveState moveStateData;
    [SerializeField]
    private Data_IdleState idelStateData;
    [SerializeField]
    private Data_PlayerDetected playerDetectedStateData;
    [SerializeField]
    private Data_MeleeAttackState meleeAttackStateData;
    [SerializeField]
    private Data_LookForPlayerState lookForPlayerStateData;
    [SerializeField]
    private Data_StunState stunStateData;
    [SerializeField]
    private Data_DeadState deadStateData;
    [SerializeField]
    public Data_DodgeState dodgeStateData;
    [SerializeField]
    private Data_RangedAttackState rangedAttackStateData;

    [SerializeField]
    private Transform meleeAttackPosition;
    [SerializeField]
    private Transform rangedAttackPosition;


        public override void Start()
        {
            base.Start();

        moveState = new E2_MoveState(this, stateMachine, "move", moveStateData, this);
        idleState = new E2_IdleState(this, stateMachine, "idle", idelStateData, this);
        playerDetectedState = new E2_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedStateData, this);
        meleeAttackState = new E2_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        lookForPlayerState = new E2_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerStateData, this);
        stunState = new E2_StunState(this, stateMachine, "stun", stunStateData, this);
        deadState = new E2_DeadState(this, stateMachine, "dead", deadStateData, this);
        dodgeState = new E2_DodgeState(this, stateMachine, "dodge", dodgeStateData, this);
        rangedAttackState = new E2_RangedAttackState(this, stateMachine, "rangedAttack",rangedAttackPosition, rangedAttackStateData, this);
        
            stateMachine.Initialize(moveState);
        }

        public override void SetFloat()
        {
            base.SetFloat();
        }

        public override void Damage(AttackDetails attackDetails)
        {
            base.Damage(attackDetails);

        if (isDead)
        {
            stateMachine.ChangeState(deadState);
        }
        else if(isStunned && stateMachine.currentState != stunState)
        {
            stateMachine.ChangeState(stunState);
        }
        else if(CheckPlayerInMinAgroRange())
        {
            stateMachine.ChangeState(rangedAttackState);
        }
        else if (!CheckPlayerInMinAgroRange())
        {
            lookForPlayerState.SetTurnImmediately(true);
            stateMachine.ChangeState(lookForPlayerState);
        }
        }

        public override void ShowDamage(string text)
        {
            base.ShowDamage(text);
        }

        public override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }

    public override void Die()
    {
        base.Die();
        //temp
        gameController.CheckLastManStanding();

        gameController.duoRemaining -= 1;

        

        if (gameController.lastManStanding)
        {
            DropHealth();
        }
    }
}
