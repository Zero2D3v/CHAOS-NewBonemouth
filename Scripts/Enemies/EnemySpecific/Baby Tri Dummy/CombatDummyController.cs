using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CombatDummyController : MonoBehaviour
{
    [SerializeField]
    private float maxHealth, knockbackSpeedX, knockbackSpeedY, knockbackDuration;
    [SerializeField]
    private bool applyKnockback;

    private float currentHealth, knockbackStart;

    private int playerFacingDirection;

    private bool playerOnLeft, knockback;

    private PlayerController pc;

    //private GameObject tri;
    private Rigidbody2D rbTri;
    private Animator triAnim;

    public void Start()
    {
        currentHealth = maxHealth;

        pc = GameObject.Find("Player").GetComponent<PlayerController>();

        //tri = transform.Find("Tri").gameObject;

        triAnim = GetComponent<Animator>();
        rbTri = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckKnockback();
    }

    private void Damage(AttackDetails attackDetails)
    {
        currentHealth -= attackDetails.damageAmount;
        playerFacingDirection = pc.GetFacingDirection();

        if(playerFacingDirection == 1)
        {
            playerOnLeft = true;
        }
        else
        {
            playerOnLeft = false;
        }

        triAnim.SetBool("playerOnLeft", playerOnLeft);
        triAnim.SetTrigger("damage");

        if(applyKnockback && currentHealth > 0.0f)
        {
            //knockback
            Knockback();
        }

        if(currentHealth <= 0.0f)
        {
            //die
            Die();
        }
    }

    private void Knockback()
    {
        knockback = true;
        knockbackStart = Time.time;
        rbTri.velocity = new Vector2(knockbackSpeedX * playerFacingDirection, knockbackSpeedY);
    }

    private void CheckKnockback()
    {
        if(Time.time >= knockbackStart + knockbackDuration && knockback)
        {
            knockback = false;
            rbTri.velocity = new Vector2(0.0f, rbTri.velocity.y);
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
