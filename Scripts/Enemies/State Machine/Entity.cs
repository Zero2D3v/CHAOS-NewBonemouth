using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.EventSystems.EventTrigger;

//base enemy class with main shared functionalities for different enemies to inherit from
public class Entity : MonoBehaviour
{
    //reference finite state machine script
    public FiniteStateMachine stateMachine;
    //reference scriptable object data for enemy type base stats
    public Data_Entity entityData;
    //reference components
    public int facingDirection { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }
    public GameObject aliveGO { get; private set; }
    public AnimationToStateMachine atsm { get; private set; }
    public int lastDamageDirection { get; private set; }
    public Material material { get; private set; }
    public float fade { get; private set; }

    public GameController gameController;

    [SerializeField]
    private Transform wallCheck;
    [SerializeField]
    private Transform ledgeCheck;
    [SerializeField]
    private Transform playerCheck;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    public Transform hitMarker;
    [SerializeField]
    private GameObject floatingTextPrefab;
    

    private float currentHealth;
    private float currentStunResistance;
    private float lastDamageTime;

    //enemy random number to hit(d20) reference
    public RandomNumber RN;
    //player random number to hit(d20) reference
    public RandomNumber PRN;
    public PlayerController playerInCombat;
    public PlayerStats PS;
    public PlayerCombatController PCC;
    public EnemyHealthBar healthbar;

    private Vector2 velocityWorkspace;

    protected bool isStunned;
    protected bool isDead;
    protected bool isDissolving;

    public Collider2D enemyCollider;

    public virtual void Start()
    {
        //set references and starting values
        facingDirection = 1;
        currentHealth = entityData.maxHealth;
        currentStunResistance = entityData.stunResistance;
        
        fade = 1f;

        RN = GetComponent<RandomNumber>();
        PRN = GameObject.Find("Player").GetComponent<RandomNumber>();
        playerInCombat = GameObject.Find("Player").GetComponent<PlayerController>();
        PS = GameObject.Find("Player").GetComponent<PlayerStats>();
        PCC = GameObject.Find("Player").GetComponent<PlayerCombatController>();

        aliveGO = transform.Find("Alive").gameObject;
        rb = aliveGO.GetComponent<Rigidbody2D>();
        anim = aliveGO.GetComponent<Animator>();
        atsm = aliveGO.GetComponent<AnimationToStateMachine>();
        material = aliveGO.GetComponent<SpriteRenderer>().material;

        SetFloat();
        //create instance of state machine
        stateMachine = new FiniteStateMachine();
    }
    //set dissolve shader slider to value 1
    public virtual void SetFloat()
    {
        material.SetFloat("_Fade", fade);
    }

    public virtual void Update()
    {
        //make follow state machine logic
        stateMachine.currentState.LogicUpdate();
        //reset stun resistance, set up so stun health is 1 so each hit means stun state animation - more like enemy hit state
        if(Time.time >= lastDamageTime + entityData.stunRecoveryTime)
        {
            ResetStunResistance();
        }
        //on death dissolve shader activated, die at end of dissolve
        if (isDissolving)
        {
            fade -= Time.deltaTime/4;

            if (fade <= 0f)
            {
                fade = 0f;
                isDissolving = false;

                if(fade == 0f)
                {
                    Die();
                }
            }
            material.SetFloat("_Fade", fade);
        }  
    }

    //physics events handled in fixed update so no stutter
    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();

    }
    //enemy damage hop function for visual feedback, if crit by player then increase
    public virtual void DamageHop(float velocity)
    {
        velocityWorkspace.Set(rb.velocity.x, velocity);
        rb.velocity = velocityWorkspace;
    }
    //reset stun health
    public virtual void ResetStunResistance()
    {
        isStunned = false;
        currentStunResistance = entityData.stunResistance;
    }

    public virtual void Damage (AttackDetails attackDetails)
    {
        //only damage if not dead
       if (currentHealth >= 0)
       {
            //for stun reset 
            lastDamageTime = Time.time;
            //records player in combat for other enemies
            playerInCombat.inCombat = true;
            //check working
            Debug.Log("player in combat");
            //add remove from combat to queue
            PCC.Invoke("RemoveFromCombat", 0.6f);
            //do damage
            currentHealth -= attackDetails.damageAmount;
            //check correct amount
            Debug.Log("minus" + attackDetails.damageAmount);
            //minus stun damage
            currentStunResistance -= attackDetails.stunDamageAmount;
            //update UI enemy healthbar
            healthbar.UpdateHealth(currentHealth / entityData.maxHealth);
            //enemy knockback
            DamageHop(entityData.damageHopSpeed);
            //spawn hitmarker
            Instantiate(entityData.hitParticle, hitMarker.position, Quaternion.Euler(0f, 0f, 0f));
            //show damage as animated pop up prefab
            ShowDamage(attackDetails.damageAmount.ToString());
            //record direction of attack
            if (attackDetails.position.x > aliveGO.transform.position.x)
            {
                lastDamageDirection = -1;
            }
            else
            {
                lastDamageDirection = 1;
            }
            //record if stunned
            if (currentStunResistance <= 0)
            {
                isStunned = true;
            }
            if (currentHealth <= 0)
            {
                //record if dead
                isDead = true;
                //start dissolve shader transition to value 0
                isDissolving = true;
                //player gain xp
                gameController.OnDeathXP();
            }
        }
        
    }
    //damage pop up floating text offset to be above hitmarker and enemy
    public virtual void ShowDamage(string text)
    {
        if (floatingTextPrefab)
        {
            GameObject prefab = Instantiate(floatingTextPrefab, hitMarker.position + new Vector3(Random.Range(-0.5f, 0.5f), 3.5f, 0f), Quaternion.identity, hitMarker.transform);
            prefab.GetComponent<TextMeshPro>().text = text;
        }
    }
    //same as damage pop up but reads miss
    public virtual void ShowMiss(string text)
    {
        if (currentHealth >= 0)
        {
            if (floatingTextPrefab)
            {
                GameObject prefab = Instantiate(floatingTextPrefab, hitMarker.position + new Vector3(Random.Range(-0.5f, 0.5f), 3.5f, 0f), Quaternion.identity, hitMarker.transform);
                prefab.GetComponent<TextMeshPro>().text = text;
            }
        }
        
    }
    //flip function using sprite rotation instead of local scale so as not to effect other components
    public virtual void Flip()
    {
        facingDirection *= -1;
        aliveGO.transform.Rotate(0f, 180f, 0f);

    }
    //die function destroys game object so enemy array count lowered
    public virtual void Die()
    {
        
        Destroy(gameObject);
        Debug.Log("die");
    }
    //drops healthdrop prefab
    public virtual void DropHealth()
    {
        Instantiate(entityData.healthDrop, aliveGO.transform.position, Quaternion.identity);
    }
    //velocity used for general movement, stopping and charge state
    public virtual void SetVelocity(float velocity)
    {
        velocityWorkspace.Set(facingDirection * velocity, rb.velocity.y);
        rb.velocity  = velocityWorkspace;
    }
    //velocity used for knockback function and dodge state
    public virtual void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        velocityWorkspace.Set(angle.x * velocity * direction, angle.y * velocity);
        rb.velocity = velocityWorkspace;
    }
    //raycast checks
    public virtual bool CheckWall()
    {
        return Physics2D.Raycast(wallCheck.position, aliveGO.transform.right, entityData.wallCheckDistance, entityData.whatIsGround);
    }

    public virtual bool CheckLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.down, entityData.ledgeCheckDistance, entityData.whatIsGround);
    }

    public virtual bool CheckGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, entityData.groundCheckRadius, entityData.whatIsGround);
    }

    public virtual bool CheckPlayerInMinAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, aliveGO.transform.right, entityData.minAgroDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInMaxAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, aliveGO.transform.right, entityData.maxAgroDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInCloseRangeAction()
    {
        return Physics2D.Raycast(playerCheck.position, aliveGO.transform.right, entityData.closeRangeActionDistance, entityData.whatIsPlayer);
    }
    //draw gizmos to see checks
    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.wallCheckDistance));
        Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * entityData.ledgeCheckDistance));

        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.closeRangeActionDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.minAgroDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.maxAgroDistance), 0.2f);   
    }
}
