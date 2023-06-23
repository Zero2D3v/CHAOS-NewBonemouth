using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.EventSystems.EventTrigger;

public class Entity : MonoBehaviour
{
    public FiniteStateMachine stateMachine;

    public Data_Entity entityData;

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

    public RandomNumber RN;
    public RandomNumber PRN;
    public PlayerController playerInCombat;
    public PlayerStats PS;
    public PlayerCombatController PCC;
    public EnemyHealthBar healthbar;

    private Vector2 velocityWorkspace;

    protected bool isStunned;
    protected bool isDead;
    protected bool isDissolving;

    //public bool isEnemyBetweenPlayer;

   // protected bool isGrounded;

    //public Transform player;
   // public float jumpDistance = 5f;
   // public float jumpForce = 5f;
   // public LayerMask obstacleLayer;
    //public float movementSpeed = 2f;
    //public float rotationSpeed = 5f;

    //private Rigidbody2D rb;
    public Collider2D enemyCollider;

    public virtual void Start()
    {
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



        stateMachine = new FiniteStateMachine();
    }

    public virtual void SetFloat()
    {
        material.SetFloat("_Fade", fade);
    }

    public virtual void Update()
    {
        stateMachine.currentState.LogicUpdate();

        anim.SetFloat("yVelocity", rb.velocity.y);

        if(Time.time >= lastDamageTime + entityData.stunRecoveryTime)
        {
            ResetStunResistance();
        }

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

    
    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();


      // if (player)
      // {
      //     // Check if there is an obstacle (another enemy) between the enemy and the player
      //     Collider2D[] obstacles = Physics2D.OverlapCircleAll(ledgeCheck.position, jumpDistance, obstacleLayer);
      //
      //     foreach (Collider2D obstacle in obstacles)
      //     {
      //         if (obstacle.CompareTag("Player") || obstacle == enemyCollider)
      //         {
      //             continue;
      //         }
      //
      //         // Calculate the jump direction
      //         Vector2 jumpDirection = Vector2.right * facingDirection;
      //
      //         // Jump behind the player
      //          rb.velocity = new Vector2(jumpDirection.x * 0.1f * jumpForce ,0.1f * jumpForce);//AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
      //         rb.AddForce(Vector2.up * jumpForce);
      //         break; // Only jump once
      //     }
      //     //ledgecheck.position
      // }

    }

    public virtual void DamageHop(float velocity)
    {
        velocityWorkspace.Set(rb.velocity.x, velocity);
        rb.velocity = velocityWorkspace;
    }

    public virtual void ResetStunResistance()
    {
        isStunned = false;
        currentStunResistance = entityData.stunResistance;
    }

    public virtual void Damage (AttackDetails attackDetails)
    {
       if (currentHealth >= 0)
       {
            lastDamageTime = Time.time;

            playerInCombat.inCombat = true;

            Debug.Log("player in combat");

            PCC.Invoke("RemoveFromCombat", 0.6f);

            currentHealth -= attackDetails.damageAmount;
            Debug.Log("minus" + attackDetails.damageAmount);
            currentStunResistance -= attackDetails.stunDamageAmount;

            healthbar.UpdateHealth(currentHealth / entityData.maxHealth);

            DamageHop(entityData.damageHopSpeed);

            Instantiate(entityData.hitParticle, hitMarker.position, Quaternion.Euler(0f, 0f, 0f));

            ShowDamage(attackDetails.damageAmount.ToString());

            if (attackDetails.position.x > aliveGO.transform.position.x)
            {
                lastDamageDirection = -1;
            }
            else
            {
                lastDamageDirection = 1;
            }

            if (currentStunResistance <= 0)
            {
                isStunned = true;
            }
            if (currentHealth <= 0)
            {
                //Die();
                isDead = true;
                isDissolving = true;
                gameController.OnDeathXP();
            }
        }
        
    }

    //public virtual void DisableHitMarker()
   // {
      //  entityData.hitParticle.SetActive(false);
   // }

    public virtual void ShowDamage(string text)
    {
        if (floatingTextPrefab)
        {
            GameObject prefab = Instantiate(floatingTextPrefab, hitMarker.position + new Vector3(Random.Range(-0.5f, 0.5f), 3.5f, 0f), Quaternion.identity, hitMarker.transform);
            prefab.GetComponent<TextMeshPro>().text = text;
        }
    }

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

    public virtual void Flip()
    {
        facingDirection *= -1;
        aliveGO.transform.Rotate(0f, 180f, 0f);

    }

    public virtual void Die()
    {
        
        Destroy(gameObject);
        Debug.Log("die");
    }

    public virtual void DropHealth()
    {
        Instantiate(entityData.healthDrop, aliveGO.transform.position, Quaternion.identity);
    }

    public virtual void SetVelocity(float velocity)
    {
        velocityWorkspace.Set(facingDirection * velocity, rb.velocity.y);
        rb.velocity  = velocityWorkspace;
    }

    public virtual void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        velocityWorkspace.Set(angle.x * velocity * direction, angle.y * velocity);
        rb.velocity = velocityWorkspace;
    }

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

 //  public virtual void CheckEnemyBetweenPlayer()
 //  {
 //       //Check if there is an obstacle (another enemy) between the enemy and the player
 //           Collider2D[] obstacles = Physics2D.OverlapCircleAll(ledgeCheck.position, jumpDistance, obstacleLayer);
 //      
 //           foreach (Collider2D obstacle in obstacles)
 //           {
 //               if (obstacle.CompareTag("Player") || obstacle == enemyCollider)
 //               {
 //                   continue;
 //               }
 //               isEnemyBetweenPlayer = true;
 //
 //
 //          // Calculate the jump direction
 //          // Vector2 jumpDirection = Vector2.right * facingDirection;
 //
 //          // Jump behind the player
 //          // rb.velocity = new Vector2(jumpDirection.x * 0.1f * jumpForce ,0.1f * jumpForce);//AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
 //          // rb.AddForce(Vector2.up * jumpForce);
 //          // break; // Only jump once
 //      }
 //           //ledgecheck.position
 //  }


    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.wallCheckDistance));
        Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * entityData.ledgeCheckDistance));
        //Gizmos.DrawLine(playerCheck.position, playerCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.minAgroDistance));

        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.closeRangeActionDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.minAgroDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.maxAgroDistance), 0.2f);

        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(ledgeCheck.position, jumpDistance);
        
    }
}
