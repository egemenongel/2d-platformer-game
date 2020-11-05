using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEditor.VersionControl;

public class PlayerController : MonoBehaviour
{
    //Start() Variables
    public Rigidbody2D rb;
    private Animator anim;
    private CircleCollider2D circleColl;
    public BoxCollider2D boxColl;
    //FSM
    private enum State { idle, running, jumping, falling, crouching, hurt, climbing }
    private State state = State.idle;
    //Inspector Variables
    [SerializeField] private LayerMask ladder;
    [SerializeField] private LayerMask ground;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 16f;
    [SerializeField] private float crouchForce = -10f;
    [SerializeField] private float hurtForce = 10f;
    [SerializeField] private float climbHorizontal = .5f;
    [SerializeField] private float climbVertical;

    [SerializeField] private AudioSource footsteps;
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource collect;
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private bool trigger;
    [SerializeField] private bool deathTrigger=false;
    [SerializeField] private Vector3 transportLocation;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        circleColl = GetComponent<CircleCollider2D>();
        boxColl = GetComponent<BoxCollider2D>();
        circleColl.isTrigger = false;
        PermanentUI.perm.healthCounterText.text = PermanentUI.perm.health.ToString();


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
        float hDirection = Input.GetAxis("Horizontal");
        //Moving Left
        if (hDirection < 0 && state != State.climbing)

        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }

        //MovingRight
        else if (hDirection > 0 && state != State.climbing)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }

        //Jumping
        if (Input.GetButtonDown("Jump"))
        {
            RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.down, 1.3f, ground);
            if (hit.collider != null || boxColl.IsTouchingLayers(ladder))
                Jump();
        }

        float vDirection = Input.GetAxis("Vertical");

        //Crouching
        if (vDirection < 0 && boxColl.IsTouchingLayers(ground) && state != State.falling && state != State.climbing)
        {
            rb.velocity = new Vector2(rb.velocity.x, crouchForce);
            circleColl.isTrigger = true;
        }
        
        else if(boxColl.IsTouchingLayers(ground) && circleColl.IsTouchingLayers(ground) && state == State.crouching)
        {
            rb.velocity = new Vector2(rb.velocity.x, crouchForce);
            circleColl.isTrigger = true;
        }

        else
        {
            circleColl.isTrigger = false;
        }

        //Climbing
        if (vDirection > 0 && boxColl.IsTouchingLayers(ladder) && state != State.hurt)
        {
            rb.velocity = new Vector2(rb.velocity.x, climbVertical);
            state = State.climbing;

            if (hDirection > .1f && boxColl.IsTouchingLayers(ladder))
            {
                rb.velocity = new Vector2(climbHorizontal, rb.velocity.y);
            }
            else if (hDirection < -.1f && boxColl.IsTouchingLayers(ladder))
            {
                rb.velocity = new Vector2(-climbHorizontal, rb.velocity.y);
            }
        }

        else if (vDirection < 0 && boxColl.IsTouchingLayers(ladder) && state != State.hurt)
        {
            rb.velocity = new Vector2(rb.velocity.x, -climbVertical);
            state = State.climbing;

            if (hDirection > .1f)
            {
                rb.velocity = new Vector2(climbHorizontal, rb.velocity.y);
            }
            else if (hDirection < -.1f)
            {
                rb.velocity = new Vector2(-climbHorizontal, rb.velocity.y);
            }
        }

        //Interaction
        if (Input.GetButtonDown("Fire1"))
        {


        }

    }
    private void Crouch()
    {
        state = State.crouching;
    }
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = State.jumping;
        jumpSound.Play();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //COLLECTABLES
        if (trigger == false)
        {
            if (collision.gameObject.tag == "Collectable")
            {
                Cherry cherry = collision.gameObject.GetComponent<Cherry>();

                collect.Play();
                cherry.Collect();
                PermanentUI.perm.collectables += 1;
                PermanentUI.perm.collectablesText.text = PermanentUI.perm.collectables.ToString();
                trigger = true;
            }
        }
        else
        {
            StartCoroutine(CollectTimer());
            trigger = false;
        }
        //GEM
        if (collision.gameObject.tag == "Gem2")
        {
            Destroy(collision.gameObject);
            jumpForce = 20;
            GetComponent<SpriteRenderer>().color = Color.red;
            StartCoroutine(GemTimer());
        }
        //PORTAL
        if (collision.gameObject.tag=="Portal in")
        {
            transform.position = new Vector3(-74.57f, -7.79f, 0);

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
                PermanentUI.perm.health -= 1;
                PermanentUI.perm.healthCounterText.text = PermanentUI.perm.health.ToString();
                Death();

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

        if (other.gameObject.tag == "Trap")
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

    private void Death()
    {
        if (PermanentUI.perm.health <= 0)
        {
            StartCoroutine(DeathTimer());
            deathTrigger = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }


    private IEnumerator GemTimer()
    {
        yield return new WaitForSeconds(5);
        jumpForce = 16;
        GetComponent<SpriteRenderer>().color = Color.white;
        

    }

    private IEnumerator CollectTimer()
    {
        yield return new WaitForSeconds(.5f);
    }

    private IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(10f);
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
            else if (boxColl.IsTouchingLayers(ladder) && Mathf.Abs(rb.velocity.y) > .1f)
            {
                state = State.climbing;
            }
        }

        else if (boxColl.IsTouchingLayers(ground)&& rb.velocity.y < -7f)
        {
            state = State.crouching;
        }

        else if (Mathf.Abs(rb.velocity.y) > .1f && boxColl.IsTouchingLayers(ladder))
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

    //Audio
    private void Footsteps()
    {
        footsteps.Play();

    }

    //Animation Events





}





