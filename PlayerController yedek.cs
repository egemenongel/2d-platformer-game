using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Start() Variables
    private Rigidbody2D rb;
    private Animator anim;
    private CircleCollider2D circleColl;
    private BoxCollider2D boxColl;

    //FSM
    private enum State { idle, running, jumping, falling, crouching, hurt, climbing }
    private State state = State.idle;

    //Inspector Variables
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpforce = 7f;
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private float crouchForce = -10f;
    [SerializeField] private float hurtForce = 10f;
    [SerializeField] private AudioSource footsteps;
    [SerializeField] private AudioSource collect;
    [SerializeField] private AudioSource gemCollect;
    [SerializeField] private int collectables = 0;
    [SerializeField] private Text collectablesText;
    [SerializeField] private float climbSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        circleColl = GetComponent<CircleCollider2D>();
        boxColl = GetComponent<BoxCollider2D>();
        circleColl.isTrigger = false;
    }

    private void Update()

    {
        if (state != State.hurt)
        {
            Movement();
        }

        AnimationState();
        anim.SetInteger("state", (int)state);
    }

    private void Movement()
    { 
        //Moving Left
        if (hDirection < 0)

        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }

        //MovingRight
        else if (hDirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }

        //Jumping
        if (Input.GetButtonDown("Jump") && boxColl.IsTouchingLayers(ground))

        {
            Jump();

        }

        float vDirection = Input.GetAxis("Vertical");

        //Crouching
        void Crouch()
        {
            state = State.crouching;
        }

        if (vDirection < 0 && boxColl.IsTouchingLayers(ground) && state != State.falling)
        {
            rb.velocity = new Vector2(rb.velocity.x, crouchForce);
            Crouch();
            circleColl.isTrigger = true;
        }
        else
        {
            circleColl.isTrigger = false;
        }

        if (circleColl.IsTouchingLayers(ground) && boxColl.IsTouchingLayers(ground))
        {
            Crouch();
        }

        else
        {

        }

        //Climbing
        private void Climb()
        {

            if (vDirection > 0 && state != State.falling)
            {
                rb.velocity = new Vector2(0, climbSpeed);
                state = State.climbing;
            }
        }

        private void Jump()
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
            state = State.jumping;
            jumpSound.Play();

        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            if (state == State.falling)

            {
                enemy.JumpedOn();
                Jump();
            }
            else
            {
                state = State.hurt;
                if (other.gameObject.transform.position.x > transform.position.x)
                {
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                    // enemy is right to the player, bounce to left
                }
                else
                {
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                    // enemy is left to the player, bounce to right
                }
            }
        }
    }

    private void AnimationState()

    {
        if (state == State.jumping)
        {
            if (rb.velocity.y < .1f)
            {
                state = State.falling;
            }

        }
        else if (state == State.falling)
        {
            if (boxColl.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }

        else if (rb.velocity.y < -2f && boxColl.IsTouchingLayers(ground))
        {
            state = State.crouching;
        }

        else if ((rb.velocity.y) > 2f && state != State.jumping)

        {
            state = State.climbing;

        }
        else if (state == State.hurt)
        {
            if (Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = State.idle;
            }
        }
        //Moving
        else if (Mathf.Abs(rb.velocity.x) > 2f)

        {

            state = State.running;
        }

        else
        {
            state = State.idle;


        }
    }
    private void Footsteps()
    {
        footsteps.Play();

    }



}





