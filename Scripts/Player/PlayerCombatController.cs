using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerCombatController : MonoBehaviour
{
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


    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("canAttack", combatEnabled);
        PC = GetComponent<PlayerController>();
        PS = GetComponent<PlayerStats>();
        PRN = GetComponent<RandomNumber>();
       // RN = GameObject.Find("Enemy1").GetComponent<RandomNumber>();
        //hitMarker = transform.Find("PlayerHitMarker").gameObject;

        DisableHitMarker();
    }

    private void Update()
    {
        CheckCombatInput();
        CheckAttacks();
        //CheckIfInCombat();

    }

   // private void CheckIfInCombat()
   // {
     //   if (!hitMarker.activeInHierarchy || !entity.entityData.hitParticle.activeInHierarchy)
    //    {
    //        PC.inCombat = false;
    //        Debug.Log("Player not in Combat");
    //    }
    //    else//(hitMarker.activeInHierarchy || entity.entityData.hitParticle.activeInHierarchy)
    //    {
    //        PC.inCombat = true;
    //        Debug.Log("player in combat");
    //    }
   // }

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
        gameManager.PlayerRollToHit();
    }

    private void UiPlayerDamageRoll()
    {
        gameManager.PLayerDamageRoll();
    }
    private void StartUiCooldown()
    {
        gameManager.StartPlayerCooldownUI();
    }
  // private void ResetUiCooldown()
  // {
  //     gameManager.ResetCooldown();
  // }
    private void CheckAttackHitBox()
    {
        PRN.GenRadNum();
        //RN.GenRadNum();
        Debug.Log("player" + PRN.RadNum);
        //Debug.Log("enemy" + RN.RadNum);
        UiPlayerRoll();


       // if (PRN.RadNum >= RN.RadNum)
       // {
            Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attack1HitBoxPos.position, attack1Radius, whatIsDamageable);

            if(PRN.RadNum != 20)
            {
                attackDetails.damageAmount = Random.Range(1, 13);
            }
            else
            {
                attackDetails.damageAmount = Random.Range(1, 13) * 2;
            }
            attackDetails.position = transform.position;
            attackDetails.stunDamageAmount = stunDamageAmount;

        UiPlayerDamageRoll();

        foreach (Collider2D collider in detectedObjects)
            {
                collider.transform.parent.SendMessage("GenRadNum");

            if (PRN.RadNum >= collider.GetComponentInParent<RandomNumber>().RadNum)
            {
                collider.transform.parent.SendMessage("Damage", attackDetails);
                //instantiate hit particle
                Debug.Log("we Hit" + collider.name);
                //collider.GetComponent<Enemy1>().AttackDetails;
            }
            else
            {
                collider.transform.parent.SendMessage("ShowMiss", "Miss");
                Debug.Log("entity missed");
            }
        }
//else if(PRN.RadNum < RN.RadNum)
       // {
            //entity.ShowMiss("MISS");
           // Debug.Log("entity missed");
       // }
    }

    private void FinishAttack1()
    {
        isAttacking = false;
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("attack1", false);
    }

    //private void FinishStun()
    //{
      //  anim.SetBool("hit", false);
    //}
    public void Damage(AttackDetails attackDetails)
    {
        int direction;

        //anim.SetBool("hit", true);
        //PS.EnableHealthBar();

        PC.inCombat = true;
        Debug.Log("Player in combat");
        Invoke("RemoveFromCombat", 0.6f);

        hitMarker.SetActive(true);
        Invoke("DisableHitMarker", 0.2f);

        if (!PS.invulnerable)
        {
            PS.DecreaseHealth(attackDetails.damageAmount);
            Debug.Log("player minus" + attackDetails.damageAmount);

            ShowDamage(attackDetails.damageAmount.ToString());
        }
        
        if(PS.invulnerable)
        {
            ShowDamage("INVULNERABLE!");
        }

        if (attackDetails.position.x < transform.position.x)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }

        PC.Knockback(direction);
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attack1HitBoxPos.position, attack1Radius);
    }

    internal void Damage(int damageAmount)
    {
        throw new System.NotImplementedException();
    }
}
