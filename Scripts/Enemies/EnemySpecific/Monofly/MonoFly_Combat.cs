using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Xml;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Timeline;
using static UnityEngine.EventSystems.EventTrigger;

public class MonoFly_Combat : MonoBehaviour
{
    public Animator anim { get; private set; }
    public GameObject aliveGO { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public AnimationToStateMachine atsm { get; private set; }
    public int facingDirection { get; private set; }
    public int lastDamageDirection { get; private set; }
    public Material material { get; private set; }
    public float fade { get; private set; }

    public GameController gameController;

    public RandomNumber RN;
    public RandomNumber PRN;
    public PlayerController playerInCombat;
    public PlayerStats PS;
    public PlayerCombatController PCC;
    public EnemyHealthBar healthbar;

    public LayerMask whatIsDamageable;

    public Data_MeleeAttackState meleeAttackData;

    public Data_Monofly monoflyData;

    public bool isStunned;
    public bool isDead;
    public bool isDissolving;
    public bool isAnimationFinished;

    private Vector2 velocityWorkspace;

    private float currentHealth;
    private float currentStunResistance;
    private float lastDamageTime;

    public AttackDetails attackDetails;

    [SerializeField]
    private Transform attackPosition;
    [SerializeField]
    private Transform playerCheck;
    [SerializeField]
    public Transform hitMarker;
    [SerializeField]
    private GameObject floatingTextPrefab;

    public EnemyAI enemyAI;

    private void Start()
    {
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


        attackDetails.damageAmount = monoflyData.attackDamage;
        attackDetails.position = aliveGO.transform.position;

        atsm.monoflyCombat = this;

        SetFloat();
    }

    public void Update()
    {
        //stateMachine.currentState.LogicUpdate();

        if (Time.time >= lastDamageTime + monoflyData.stunRecoveryTime)
        {
           ResetStunResistance();
        }


        if (isDissolving)
        {
            //TriggerDeath();

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

    public void ResetStunResistance()
    {
        isStunned = false;
        currentStunResistance = monoflyData.stunResistance;
    }

    public void TriggerAttack()
    {

        RN.GenRadNum();
        Debug.Log("entity" + RN.RadNum);

        if (PCC.isAttacking)
        {
            if (RN.RadNum >= PRN.RadNum)
            {
                monoflyData.GetDamage();
                //Debug.Log("enemy damage" + stateData.attackDamage);

                Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackPosition.position, monoflyData.attackRadius, monoflyData.whatisPlayer);

                foreach (Collider2D collider in detectedObjects)
                {
                    collider.transform.SendMessage("Damage", attackDetails);
                    //collider.transform.SendMessage("Knockback", PlayerController.knockbackSpeed);

                    Debug.Log("player HIT");
                }
            }
            else
            {
                PCC.ShowMiss("Miss");
                Debug.Log("player missed");
            }
        }
        else if ( playerInCombat.inCombat || !PCC.isAttacking)
        {
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
            else
            {
                PCC.ShowMiss("Miss");
                Debug.Log("player missed");
            }
        }
       //{
       //    if (RN.RadNum >= PS.AC)
       //    {
       //        monoflyData.GetDamage();
       //
       //        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackPosition.position, monoflyData.attackRadius, monoflyData.whatisPlayer);
       //
       //        foreach (Collider2D collider in detectedObjects)
       //        {
       //            collider.transform.SendMessage("Damage", attackDetails);
       //            //collider.transform.SendMessage("Knockback", PlayerController.knockbackSpeed);
       //
       //            Debug.Log("player HIT AC");
       //        }
       //    }
       //}
       //else if (RN.RadNum >= PRN.RadNum)
       //{
       //    monoflyData.GetDamage();
       //    //Debug.Log("enemy damage" + stateData.attackDamage);
       //
       //    Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackPosition.position, monoflyData.attackRadius, monoflyData.whatisPlayer);
       //
       //    foreach (Collider2D collider in detectedObjects)
       //    {
       //        collider.transform.SendMessage("Damage", attackDetails);
       //        //collider.transform.SendMessage("Knockback", PlayerController.knockbackSpeed);
       //
       //        Debug.Log("player HIT");
       //    }
       //}
       //else
       //{
       //    Debug.Log("player missed");
       //}

    }

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

    public void Damage(AttackDetails attackDetails)
    {
        if (currentHealth > 0)
        {
            lastDamageTime = Time.time;

            playerInCombat.inCombat = true;

            Debug.Log("player in combat");

            PCC.Invoke("RemoveFromCombat", 0.6f);

            currentHealth -= attackDetails.damageAmount;
            Debug.Log("minus" + attackDetails.damageAmount);
            currentStunResistance -= attackDetails.stunDamageAmount;
            Stun();

            healthbar.UpdateHealth(currentHealth / monoflyData.maxHealth);

            //DamageHop(entityData.damageHopSpeed);

            Instantiate(monoflyData.hitParticle, hitMarker.position, Quaternion.Euler(0f, 0f, 0f));

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
                enemyAI.enabled = false;
                gameController.OnDeathXP();
            }
        }

    }

    private void ShowDamage(string text)
    {
        if (floatingTextPrefab)
        {
            GameObject prefab = Instantiate(floatingTextPrefab, hitMarker.position + new Vector3(Random.Range(-0.5f, 0.5f), 3.5f, 0f), Quaternion.identity, hitMarker.transform);
            //GameObject prefab = Instantiate(floatingTextPrefab, transform.position + new Vector3(0f, 4f, 0f), Quaternion.identity, hitMarker.transform);
            prefab.GetComponent<TextMeshPro>().text = text;
        }
    }

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

    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        velocityWorkspace.Set(angle.x * velocity * direction, angle.y * velocity);
        rb.velocity = velocityWorkspace;
    }

    public void SetVelocity(float velocity)
    {
        velocityWorkspace.Set(facingDirection * velocity, rb.velocity.y);
        rb.velocity = velocityWorkspace;
    }

    private void FinishAttack()
    {

    }

    public void TriggerDeath()
    {
        isAnimationFinished = false;
        SetVelocity(0f);
        //death anim
        aliveGO.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
    }

    public void FinishDeath()
    {
        isAnimationFinished = true;
        gameController.monoRemaining -= 1;
    }
}
