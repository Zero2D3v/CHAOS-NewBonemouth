using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Xml;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Timeline;
using static UnityEngine.EventSystems.EventTrigger;

//state machine varient without inheritance for flying enemy, works in conjunction with A* pathfinding movement without velocity being set to 0
public class MonoFly_Combat : MonoBehaviour
{
    //set references to read
    public Animator anim { get; private set; }
    public GameObject aliveGO { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public AnimationToStateMachine atsm { get; private set; }
    public int facingDirection { get; private set; }
    public int lastDamageDirection { get; private set; }
    public Material material { get; private set; }
    public float fade { get; private set; }
    //reference to game controller script for speaking between script and storing values
    public GameController gameController;
    //reference to random number script (d20 to hit)
    public RandomNumber RN;
    //reference to player random number script (d20 to hit)
    public RandomNumber PRN;
    //reference to player scripts
    public PlayerController playerInCombat;
    public PlayerStats PS;
    public PlayerCombatController PCC;
    //healthbar script reference
    public EnemyHealthBar healthbar;
    //refeence to what can be damaged
    public LayerMask whatIsDamageable;
    //reference to scriptable object data (attack relavent)
    public Data_MeleeAttackState meleeAttackData;
    //reference to enemy stats data
    public Data_Monofly monoflyData;
    //behaviour determining bools reference
    public bool isStunned;
    public bool isDead;
    public bool isDissolving;
    public bool isAnimationFinished;
    //velocity scale used in movement speed change
    private Vector2 velocityWorkspace;
    //in script references to hold external refernces
    private float currentHealth;
    private float currentStunResistance;
    private float lastDamageTime;
    //reference to struct for damage holding
    public AttackDetails attackDetails;
    //references to in scene points and objects that systems work from
    [SerializeField]
    private Transform attackPosition;
    [SerializeField]
    private Transform playerCheck;
    [SerializeField]
    public Transform hitMarker;
    [SerializeField]
    private GameObject floatingTextPrefab;
    //reference A* movement script
    public EnemyAI enemyAI;

    private void Start()
    {
        //set references and starting values
        facingDirection = -1;
        currentHealth = monoflyData.maxHealth;
        currentStunResistance = monoflyData.stunResistance;
        fade = 1f;
        
        RN = GetComponent<RandomNumber>();
        PRN = GameObject.Find("Player").GetComponent<RandomNumber>();
        playerInCombat = GameObject.Find("Player").GetComponent<PlayerController>();
        PS = GameObject.Find("Player").GetComponent<PlayerStats>();
        PCC = GameObject.Find("Player").GetComponent<PlayerCombatController>();

        aliveGO = GameObject.Find("Monodrone GFX").gameObject;
        rb = aliveGO.GetComponent<Rigidbody2D>();
        anim = aliveGO.GetComponent<Animator>();
        atsm = aliveGO.GetComponentInChildren<AnimationToStateMachine>();
        material = aliveGO.GetComponentInChildren<SpriteRenderer>().material;

        //set struct starting values
        attackDetails.damageAmount = monoflyData.attackDamage;
        attackDetails.position = aliveGO.transform.position;
        //reference animatio to state machine script for animation events trigger
        atsm.monoflyCombat = this;
        //set dissolve shader to 1
        SetFloat();
    }

    public void Update()
    {
        //reset stun resistance, set up so stun health is 1 so each hit means stun state animation - more like enemy hit state
        if (Time.time >= lastDamageTime + monoflyData.stunRecoveryTime)
        {
           ResetStunResistance();
        }
        //on death (current health = 0) dissolve shader activated, die at end of dissolve (destroy game object)
        if (isDissolving)
        {
            fade -= Time.deltaTime / 4;

            if (fade <= 0f)
            {
                fade = 0f;
                isDissolving = false;

                if (fade == 0f)
                {
                    FinishDeath();
                    Die();
                }
            }
            material.SetFloat("_Fade", fade);
        }
    }

    public void SetFloat()
    {
        material.SetFloat("_Fade", fade);
    }
    //reset stun health like in entity based state machine
    public void ResetStunResistance()
    {
        isStunned = false;
        currentStunResistance = monoflyData.stunResistance;
    }
    //attack function
    public void TriggerAttack()
    {
        //generate random number for enemy d20 to hit roll
        RN.GenRadNum();
        Debug.Log("entity" + RN.RadNum);
        //if player in batle with player also attacking then vs player to hit roll
        if (PCC.isAttacking)
        {
            if (RN.RadNum >= PRN.RadNum)
            {
                //generate damage roll from enemy data
                monoflyData.GetDamage();
                //check hit box for player
                Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackPosition.position, monoflyData.attackRadius, monoflyData.whatisPlayer);

                foreach (Collider2D collider in detectedObjects)
                {
                    //do damage to player
                    collider.transform.SendMessage("Damage", attackDetails);
                    Debug.Log("player HIT");
                }
            }
            //if enemy loses vs to hit roll, then show miss
            else
            {
                PCC.ShowMiss("Miss");
                Debug.Log("player missed");
            }
        }
        //if player already in combat with another enemy or not attacking back, then enemy to hit roll vs player defense stat
        else if ( playerInCombat.inCombat || !PCC.isAttacking)
        {
            //if enemy win to hit roll then do damage as before
            if(RN.RadNum >= PS.AC)
            {
                monoflyData.GetDamage();

                Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackPosition.position, monoflyData.attackRadius, monoflyData.whatisPlayer);

                foreach (Collider2D collider in detectedObjects)
                {
                    collider.transform.SendMessage("Damage", attackDetails);
                    //collider.transform.SendMessage("Knockback", PlayerController.knockbackSpeed);

                    Debug.Log("player HIT AC");
                }
            }
            //if enemy lose against player defense then enemy miss
            else
            {
                PCC.ShowMiss("Miss");
                Debug.Log("player missed");
            }
        }
    }
    //enemy stun function slight knockback on hit which goes with stun animation, if crit then knockback hits into air
    public void Stun()
    {
        if (PRN.RadNum != 20)
        {
            SetVelocity(monoflyData.stunKnockbackSpeed, monoflyData.stunKnockbackAngle, lastDamageDirection);
        }
        else
        {
            SetVelocity(monoflyData.stunCritKnocbackSpeed, monoflyData.stunKnockbackAngle, lastDamageDirection);
        }
    }
    //damage function called from player attack send message
    public void Damage(AttackDetails attackDetails)
    {
        //only take damage if not dead, so when dissolving doesn't still have damage pop ups and hit markers
        if (currentHealth > 0)
        {
            //record time of attack for stun reset timer
            lastDamageTime = Time.time;
            //record player in combat for if multpile enemies attacking
            playerInCombat.inCombat = true;
            Debug.Log("player in combat");
            //add remove player from combat to queue
            PCC.Invoke("RemoveFromCombat", 0.6f);
            //apply damage
            currentHealth -= attackDetails.damageAmount;
            Debug.Log("minus" + attackDetails.damageAmount);
            //apply stun damage which triggers animation
            currentStunResistance -= attackDetails.stunDamageAmount;
            //apply stun movement
            Stun();
            //update UI healthbar
            healthbar.UpdateHealth(currentHealth / monoflyData.maxHealth);
            //spawn hitmarker
            Instantiate(monoflyData.hitParticle, hitMarker.position, Quaternion.Euler(0f, 0f, 0f));
            //spawn pop up floating damage text
            ShowDamage(attackDetails.damageAmount.ToString());
            //record from which direction enemy took damage from
            if (attackDetails.position.x > aliveGO.transform.position.x)
            {
                lastDamageDirection = -1;
            }
            else
            {
                lastDamageDirection = 1;
            }
            //bool if stunned relied upon by stun animation bool
            if (currentStunResistance <= 0)
            {
                isStunned = true;
            }
            //handles death sequence
            if (currentHealth <= 0)
            {
                //record death status
                isDead = true;
                //start dissolve shader
                isDissolving = true;
                //disable A* movement
                enemyAI.enabled = false;
                //reward player with XP gain
                gameController.OnDeathXP();
            }
        }

    }
    //show damage text and can't happen if enemy already dead
    private void ShowDamage(string text)
    {
        if (currentHealth <= 0)
        {
            if (floatingTextPrefab)
            {
                GameObject prefab = Instantiate(floatingTextPrefab, hitMarker.position + new Vector3(Random.Range(-0.5f, 0.5f), 3.5f, 0f), Quaternion.identity, hitMarker.transform);
                prefab.GetComponent<TextMeshPro>().text = text;
            }
        }
    }
    //show miss text and can't happen if enemy already dead
    private void ShowMiss(string text)
    {
        if(currentHealth <= 0)
        {
            if (floatingTextPrefab)
            {
                GameObject prefab = Instantiate(floatingTextPrefab, hitMarker.position + new Vector3(Random.Range(-0.5f, 0.5f), 3.5f, 0f), Quaternion.identity, hitMarker.transform);
                prefab.GetComponent<TextMeshPro>().text = text;
            }
        }
        
    }

    public void Die()
    {
        Destroy(gameObject);
        Debug.Log("die");
    }
    //for knockback and crit stun movement
    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        velocityWorkspace.Set(angle.x * velocity * direction, angle.y * velocity);
        rb.velocity = velocityWorkspace;
    }
    //for movement
    public void SetVelocity(float velocity)
    {
        velocityWorkspace.Set(facingDirection * velocity, rb.velocity.y);
        rb.velocity = velocityWorkspace;
    }

    private void FinishAttack()
    {
        //for adding effects or functionality in future
    }

    public void TriggerDeath()
    {
        //plays death animation aided by 90 degree rotation so enemy goes on its back as it dissolves
        isAnimationFinished = false;
        SetVelocity(0f);
        aliveGO.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
    }

    public void FinishDeath()
    {
        //on dissolve and death animation completion, update values in game controller
        isAnimationFinished = true;
        gameController.monoRemaining -= 1;
    }
}
