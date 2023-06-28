using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerCombatController : MonoBehaviour
{
    //declare fields and classes to reference and affect
    [SerializeField]
    private bool combatEnabled;
    [SerializeField]
    private float inputTimer, attack1Radius, attack1Damage;
    [SerializeField]
    private float stunDamageAmount = 1f;
    [SerializeField]
    private Transform attack1HitBoxPos;
    [SerializeField]
    private GameObject floatingTextPrefab;
    [SerializeField]
    private LayerMask whatIsDamageable;

    private bool gotInput, isFirstAttack;
    public bool isAttacking;

    private float lastInputTime = Mathf.NegativeInfinity;
    //declare struct which will be used as a container for attack information
    public AttackDetails attackDetails;

    public Entity entity;

    public MonoFly_Combat monofly;

    private Animator anim;

    private PlayerController PC;
    private PlayerStats PS;
    public RandomNumber PRN;
    private RandomNumber RN;
    public GameObject hitMarker;

    public GameController gameManager;

    public int activeWeaponDamageAmount;


    private void Start()
    {
        //set references
        anim = GetComponent<Animator>();
        anim.SetBool("canAttack", combatEnabled);
        PC = GetComponent<PlayerController>();
        PS = GetComponent<PlayerStats>();
        PRN = GetComponent<RandomNumber>();
        //make sure hit marker game object set to off
        DisableHitMarker();
    }

    private void Update()
    {
        //do checks
        CheckCombatInput();
        CheckAttacks();
    }

    private void CheckCombatInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (combatEnabled)
            {
                //attempt combat
                gotInput = true;
                lastInputTime = Time.time;
            }
        }
    }

    public void CheckAttacks()
    {
        if (gotInput)
        {
            //perform attack1
            if (!isAttacking)
            {
                //can add more attacks
                gotInput = false;
                isAttacking = true;
                isFirstAttack = !isFirstAttack;
                anim.SetBool("attack1", true);
                anim.SetBool("firstAttack", isFirstAttack);
                anim.SetBool("isAttacking", isAttacking);
            }
        }

        if (Time.time >= lastInputTime + inputTimer)
        {
            //wait for new input
            gotInput = false;
        }
    }
    private void UiPlayerRoll()
    {
        //To hit dice roll UI d20 element
        gameManager.PlayerRollToHit();
    }

    private void UiPlayerDamageRoll()
    {
        //damage dice roll UI depending on active weapon 
        gameManager.PLayerDamageRoll();
    }
    private void StartUiCooldown()
    {
        //starts cooldown UI bar on attack
        gameManager.StartPlayerCooldownUI();
    }
    private void CheckAttackHitBox()
    {
        //generate random number for d20 to hit dice
        PRN.GenRadNum();
        Debug.Log("player" + PRN.RadNum);
        UiPlayerRoll();
        //calculate damage depending on active weapon
        AttackDamage(gameManager.playerSelectedWeaapon);
        //check colliders for detected objects
            Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attack1HitBoxPos.position, attack1Radius, whatIsDamageable);
        //apply crit double damage roll if to hit was 20, add strength modeifier seperately
            if(PRN.RadNum != 20)
            {
                attackDetails.damageAmount = PS.Strength + activeWeaponDamageAmount;
            }
            else
            {
                attackDetails.damageAmount = PS.Strength + (activeWeaponDamageAmount * 2);
            }
            //where attack comes from
            attackDetails.position = transform.position;
            //stun damage amount done seperately, generally set to 1 to stun enemy but if crit, enemy thrown into air
            attackDetails.stunDamageAmount = stunDamageAmount;

        UiPlayerDamageRoll();

        foreach (Collider2D collider in detectedObjects)
            {
            //generate enemy roll to hit d20 result to compare against player to hit d20
                collider.transform.parent.SendMessage("GenRadNum");
            //compare results
            if (PRN.RadNum >= collider.GetComponentInParent<RandomNumber>().RadNum)
            {
                //if player win or draw then do damage
                collider.transform.parent.SendMessage("Damage", attackDetails);
                //instantiate hit particle
                Debug.Log("we Hit" + collider.name);
                //collider.GetComponent<Enemy1>().AttackDetails;
            }
            else
            {
                //if player lose show miss!
                collider.transform.parent.SendMessage("ShowMiss", "Miss");
                Debug.Log("entity missed");
            }
        }
    }
    //reset values when attack finished, called at end of attack animtion with an event
    private void FinishAttack1()
    {
        isAttacking = false;
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("attack1", false);
    }
    //yet to implement player stun if I do
    //private void FinishStun()
    //{
      //  anim.SetBool("hit", false);
    //}
    public void Damage(AttackDetails attackDetails)
    {
        //record direction from attack
        int direction;
        //player in combat true
        PC.inCombat = true;
        Debug.Log("Player in combat");
        Invoke("RemoveFromCombat", 0.6f);
        //visual feedback player hit by enemy
        hitMarker.SetActive(true);
        Invoke("DisableHitMarker", 0.2f);
        //do damage if player not using invulnerable ability
        if (!PS.invulnerable)
        {
            PS.DecreaseHealth(attackDetails.damageAmount);
            Debug.Log("player minus" + attackDetails.damageAmount);

            ShowDamage(attackDetails.damageAmount.ToString());
        }
        //otherwise invulnerable text pop up
        if(PS.invulnerable)
        {
            ShowDamage("INVULNERABLE!");
        }
        //calaculate knockback direction
        if (attackDetails.position.x < transform.position.x)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }
        //apply player knockback
        PC.Knockback(direction);
    }
    //calculate player attack damage depending on active weapon
    void AttackDamage(int selectedWeapon)
    {
        if(selectedWeapon <= 0)
        {
            activeWeaponDamageAmount = Random.Range(1, 5);
            Debug.Log("d4");
        }
        else
        {
            activeWeaponDamageAmount = Random.Range(1, 13);
        }
    }

    private void ShowDamage(string text)
    {
        if (floatingTextPrefab)
        {
            GameObject prefab = Instantiate(floatingTextPrefab, hitMarker.transform.position + new Vector3(0f, 4f, 0f), Quaternion.identity);//, hitmarker.transform)
            prefab.GetComponent<TextMeshPro>().text = text;
        }
    }

    public void ShowMiss(string text)
    {
        if (floatingTextPrefab)
        {
            GameObject prefab = Instantiate(floatingTextPrefab, hitMarker.transform.position + new Vector3(0f, 4f, 0f), Quaternion.identity);
            prefab.GetComponent<TextMeshPro>().text = text;
        }
    }

    private void DisableHitMarker()
    {
        hitMarker.SetActive(false);
    }

    private void RemoveFromCombat()
    {
        PC.inCombat = false;
        Debug.Log("player not in combat");
    }
    //draw player attack hitbox
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attack1HitBoxPos.position, attack1Radius);
    }
}
