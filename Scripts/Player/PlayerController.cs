using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//player controller script to handle movement and non combat functionality
public class PlayerController : MonoBehaviour
{
    //declare fields
    private float movementInputDirection;
    private float knockbackStartTime;

    [SerializeField]
    private float knockbackDuration;
    //not implemented player stun yet
    //[SerializeField]
   // private float stunDuration;

    private int amountOfJumpsLeft;
    private int facingDirection = 1;

    private bool isFacingRight = true;
    private bool isWalking;
    private bool isGrounded;
    private bool isJumping = false;
    private bool canJump;
    private bool canFlip;
    private bool knockback;
    //private bool isStunned;

    [SerializeField]
    private Vector2 knockbackSpeed;

    private Rigidbody2D rb;
    private Animator anim;

    public int amountOfJumps = 1;

    public bool inCombat;

    public float movementSpeed = 10f;
    public float jumpForce = 16f;
    public float groundCheckRadius;

    public Transform groundCheck;

    public LayerMask whatIsGround;

    // Start is called before the first frame update
    void Start()
    {
        //set references and starting values
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfJumps;
    }

    // Update is called once per frame
    void Update()
    {
        //always do checks and update animations based on inupts
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfCanJump();
        CheckKnockback();
    }

    public int GetFacingDirection()
    {
        return facingDirection;
    }

    private void CheckMovementDirection()
    {
        //flip when necessary
        if (isFacingRight && canFlip && movementInputDirection < 0)
        {
            Flip();
        }
        else if(!isFacingRight && canFlip && movementInputDirection > 0)
        {
            Flip();
        }
        //record walking for animation purposes
        if (rb.velocity.x != 0)
        {
            isWalking = true;
        }
        else 
        {
            isWalking = false;
        }
        //record jump
        if (Input.GetButtonDown("Jump") && (canJump = true))
        {
            isJumping = true;
        }
        else
        {
            isJumping = false;
        }
    }

    private void FixedUpdate()
    {
        //apply movement and check surroundings in fixed update to reduce clutter/jitter
        ApplyMovement();
        CheckSurroundings();
    }
    //player knockback function
    public void Knockback(int direction)
    {
        knockback = true;
        knockbackStartTime = Time.time;
        rb.velocity = new Vector2(knockbackSpeed.x * direction, knockbackSpeed.y);

        //isStunned = true;
    }

    //record knockback checks
    private void CheckKnockback()
    {
        if(Time.time >= knockbackStartTime + knockbackDuration && knockback)
        {
            knockback = false;
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }
       // if(Time.time >= knockbackStartTime + stunDuration && isStunned)
       // {
       //     isStunned = false;
       // }
    }
    //check if grounded for jump purposes
    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }
    //check and record if can jump depending on if grounded and jumps remaining for double jump
    private void CheckIfCanJump()
    {
        if (isGrounded && rb.velocity.y <= 0)
        {
            amountOfJumpsLeft = amountOfJumps;
        }
        if (amountOfJumpsLeft <= 0)
        {
            canJump = false;
        }
        else
        {
            canJump = true;
        }
    }
    //record player input
    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }
    //function responsible for setting animations
    private void UpdateAnimations()
    {
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isJumping", isJumping);
        //anim.SetBool("isStunned", isStunned);
        
        //anim.SetBool("isGrounded", isGrounded);
        //anim.SetFloat("yVelocity", rb.velocity.y);
    }
    //jump function applies new velocity and reduces jumps left count
    private void Jump()
    {
        if (canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            amountOfJumpsLeft--;
        }

    }
    //apply movement velocity based on input
    private void ApplyMovement()
    {
            rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
    }
    //make so cant flip , called when attacking
    public void DisableFlip()
    {
        canFlip = false;
    }
    //reset after attack animation finished
    public void EnableFlip()
    {
        canFlip = true;
    }

    private void Flip()
    {
        //invert facing direction set in start function
        facingDirection *= -1;
        //record inversion from previous facing direction
        isFacingRight = !isFacingRight;
        //flip object using rotation to affect child objects but if something not want to be rotated can keep unaffected using transform.rotation = quaternion.identity (unlike if using local scale to flip
        transform.Rotate(0.0f, 180f, 0.0f);
    }
    //draw ground check to see
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
