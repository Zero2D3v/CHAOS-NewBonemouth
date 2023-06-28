using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script that handles the throwing of the spear in enemy 2 long ranged attack state, attached to spear prefab
//from state machine tutorial series, seems over complicated for my purposes, will replace with a homing script where velocity increased depending on crit or miss
public class Projectile : MonoBehaviour
{
    //struct as container
    private AttackDetails attackDetails;
    //declare fields
    private float speed;
    private float travelDistance;
    private float xStartPos;

    [SerializeField]
    private float gravity;
    [SerializeField]
    private float damageRadius;

    private Rigidbody2D rb;

    private bool isGravityOn;
    private bool hasHitGround;

    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private LayerMask whatIsPlayer;
    [SerializeField]
    private Transform damagePosition;


    private void Start()
    {
        //set references
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0.0f;
        rb.velocity = transform.right * speed;
        //gravity set to off to allow for straight thrown path
        isGravityOn = false;

        xStartPos = transform.position.x;
    }

    private void Update()
    {
        if (!hasHitGround)
        {
            //update struct attack position value if still in air
            attackDetails.position = transform.position;
            //calculates and aplies rotation based on gravity
            if (isGravityOn)
            {
                float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
    }

    private void FixedUpdate()
    {
        if (!hasHitGround)
        {
            //checks to see what it hits if still travelling
            Collider2D damageHit = Physics2D.OverlapCircle(damagePosition.position, damageRadius, whatIsPlayer);
            Collider2D groundHit = Physics2D.OverlapCircle(damagePosition.position, damageRadius, whatIsGround);

            if (damageHit)
            {
                //if player hit apply damage with send message
                damageHit.transform.SendMessage("Damage", attackDetails);
                //destroy spear
                Destroy(gameObject);
            }

            if (groundHit)
            {
                //if ground hit record, stop velocity and disable gravity calculations
                hasHitGround = true;
                rb.gravityScale = 0f;
                rb.velocity = Vector2.zero;
            }
            //enable gravity after spear travelled set distance to give fall off effect
            if (Mathf.Abs(xStartPos - transform.position.x) >= travelDistance && !isGravityOn)
            {
                isGravityOn = true;
                rb.gravityScale = gravity;
            }
        }

        
    }
    //in charge of throwing spear
    public void FireProjectile(float speed, float travelDistance, float damage)
    {
        this.speed = speed;
        this.travelDistance = travelDistance;
        attackDetails.damageAmount = damage;
    }
    //draws damage radius
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(damagePosition.position, damageRadius);
    }
}

