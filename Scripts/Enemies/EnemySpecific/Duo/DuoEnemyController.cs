using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuoEnemyController : MonoBehaviour
{
    public GameObject player;
    public GameObject spearPrefab;
    public Transform spearSpawnPoint;
    public float spearSpeed = 10f;
    public float spearCooldownTime = 1f;
    public float meleeCooldown = 1f;
    public float stunDuration = 2f;
    public float detectionRange = 10f;
    public float meleeRange = 2f;
    public float moveSpeed = 3f;
    public int maxHealth = 3;
    public int meleeDamage = 1;
    public int spearDamage = 2;

    private int spearsRemaining = 3;
    private float timeSinceLastSpear = 0f;
    private float timeSinceLastMelee = 0f;
    private int currentHealth;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isPlayerInRange = false;
    private bool isPlayerInMeleeRange = false;
    private bool isAttacking = false;
    private bool isStunned = false;

    void Start()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (isStunned)
        {
            StunState();
        }
        else if (isAttacking)
        {
            AttackState();
        }
        else if (isPlayerInMeleeRange)
        {
            MeleeState();
        }
        else if (isPlayerInRange)
        {
            RangedState();
        }
        else
        {
            MoveState();
        }

        timeSinceLastSpear += Time.deltaTime;
        timeSinceLastMelee += Time.deltaTime;
    }

    void MoveState()
    {
        animator.SetBool("isMoving", true);

        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);

        if (isPlayerInRange)
        {
            isAttacking = true;
        }
    }

    void RangedState()
    {
        animator.SetBool("isMoving", false);

        if (CanThrowSpear())
        {
            animator.SetTrigger("ThrowSpear");
            //ResetSpearCooldown();
            isAttacking = true;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }
    }

    void MeleeState()
    {
        animator.SetBool("isMoving", false);

        if (CanMelee())
        {
            animator.SetTrigger("MeleeAttack");
            //ResetMeleeCooldown();
            isAttacking = true;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }
    }

    void AttackState()
    {
        animator.SetBool("isMoving", false);

        if (!isAttacking)
        {
            if (isPlayerInMeleeRange)
            {
                isAttacking = true;
            }
            else if (isPlayerInRange)
            {
                if (CanThrowSpear())
                {
                    animator.SetTrigger("ThrowSpear");
                    //ResetSpearCooldown();
                }
            }
            else
            {
                isAttacking = false;
            }
        }
    }

    void StunState()
    {
        animator.SetBool("isMoving", false);

        isAttacking = false;

        rb.velocity = Vector2.zero;

        animator.SetTrigger("Stunned");

        isStunned = false;
    }

    bool CanThrowSpear()
    {
        return spearsRemaining > 0 && timeSinceLastSpear >= spearCooldownTime && isPlayerInRange;
    }

    bool CanMelee()
    {
        return timeSinceLastMelee >= 1f && isPlayerInMeleeRange;

    }
 // void ThrowSpear()
 // {
 //     GameObject spear = Instantiate(spearPrefab, spearSpawnPoint.position, Quaternion.identity);
 //     spear.GetComponent<Rigidbody2D>().velocity = (player.transform.position - transform.position).normalized * spearSpeed;
 //     spear.GetComponent<SpearController>().damage = spearDamage;
 //
 //     spearsRemaining--;
 // }
 //
 // void MeleeAttack()
 // {
 //     player.GetComponent<PlayerHealth>().TakeDamage(meleeDamage);
 // }
 //
 // void ResetSpearCooldown()
 // {
 //     timeSinceLastSpear = 0f;
 // }
 //
 // void ResetMeleeCooldown()
 // {
 //     timeSinceLastMelee = 0f;
 // }
 //
 // public void TakeDamage(int damage)
 // {
 //     currentHealth -= damage;
 //
 //     if (currentHealth <= 0)
 //     {
 //         Die();
 //     }
 //     else
 //     {
 //         isStunned = true;
 //         StartCoroutine(StunCoroutine());
 //     }
 // }
 //
 // IEnumerator StunCoroutine()
 // {
 //     yield return new WaitForSeconds(stunDuration);
 //     isStunned = false;
 // }
 //
 // void Die()
 // {
 //     animator.SetTrigger("Die");
 //
 //     rb.velocity = Vector2.zero;
 //
 //     GetComponent<Collider2D>().enabled = false;
 //
 //     Destroy(gameObject, 1f);
 // }
}
