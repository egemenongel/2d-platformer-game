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
    [HideInInspector] public Rigidbody2D rb;
    private Animator anim;
    [HideInInspector] public CircleCollider2D circleColl;
    [HideInInspector] public BoxCollider2D boxColl;

    //Finite State Machine
    public enum State { idle, running, jumping, falling, crouching, hurt, climbing }
    public State playerState = State.idle;
    //Inspector Variables
    [SerializeField] private LayerMask ladder;
    [SerializeField] private LayerMask ground;

    [SerializeField] public float jumpForce = 11f;
    [SerializeField] private float speed = 5f;

    [SerializeField] private AudioSource footsteps;
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] public AudioSource collect;
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioSource hit;
    [SerializeField] private bool deathTrigger = false;
    [SerializeField] public bool invincibleTrigger = false;

    [HideInInspector] private float climbHorizontal = .5f;
    [HideInInspector] private float climbVertical = 3f;
    [HideInInspector] private float crouchForce = -8f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        circleColl = GetComponent<CircleCollider2D>();
        boxColl = GetComponent<BoxCollider2D>();
        circleColl.isTrigger = false;
        invincibleTrigger = false;
        PermanentUI.perm.healthCounterText.text = PermanentUI.perm.health.ToString();
        PermanentUI.perm.levelText.text = "L - " + PermanentUI.perm.levelCounter.ToString();

    }

    private void Update()

    {
        if (playerState != State.hurt)
       {
            Movement();
        }

        AnimationState();
        anim.SetInteger("state", (int)playerState);

    }
    
    private void Movement()
    {
        float hDirection = Input.GetAxis("Horizontal");
        //Moving Left
        if (hDirection < 0 && playerState != State.climbing)

        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }

        //MovingRight
        else if (hDirection > 0 && playerState != State.climbing)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }

        //Jumping
        if (Input.GetButtonDown("Jump") && playerState != State.crouching)
        {
            RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.down, 1.3f, ground);
            if (hit.collider != null || boxColl.IsTouchingLayers(ladder))
            {
                playerJump();
            }
                
        }

        float vDirection = Input.GetAxis("Vertical");

        //Crouching
        Crouch(vDirection);

        //Climbing
        if (vDirection > 0 && boxColl.IsTouchingLayers(ladder))
        {
            rb.velocity = new Vector2(rb.velocity.x, climbVertical);
            playerState = State.climbing;

            if (hDirection > .1f && boxColl.IsTouchingLayers(ladder))
            {
                rb.velocity = new Vector2(climbHorizontal, rb.velocity.y);
            }
            else if (hDirection < -.1f && boxColl.IsTouchingLayers(ladder))
            {
                rb.velocity = new Vector2(-climbHorizontal, rb.velocity.y);
            }
        }

        else if (vDirection < 0 && boxColl.IsTouchingLayers(ladder))
        {
            rb.velocity = new Vector2(rb.velocity.x, -climbVertical);
            playerState = State.climbing;

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

    public void playerJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        playerState = State.jumping;
        jumpSound.Play();

    }
    private void Crouch(float vDirection)
    {
        if (vDirection < 0 && boxColl.IsTouchingLayers(ground) && playerState != State.falling && playerState != State.climbing)
        {
            rb.velocity = new Vector2(rb.velocity.x, crouchForce);
            circleColl.isTrigger = true;
        }

        else if (boxColl.IsTouchingLayers(ground) && circleColl.IsTouchingLayers(ground) && playerState == State.crouching)
        {
            rb.velocity = new Vector2(rb.velocity.x, crouchForce);
            circleColl.isTrigger = true;
        }

        else
        {
            circleColl.isTrigger = false;
        }
    }

    private void AnimationState()

    {
        if (Mathf.Abs(rb.velocity.y) > .1f && boxColl.IsTouchingLayers(ladder))
        {
            playerState = State.climbing;
        }
        else if (playerState == State.jumping)
        {
            if (rb.velocity.y < .1f)
            {
                playerState = State.falling;
            }

            //Transition from jumping to falling

        }
        else if (playerState == State.falling)
        {
            if (boxColl.IsTouchingLayers(ground))
            {
                playerState = State.idle;
            }

        }

        else if (boxColl.IsTouchingLayers(ground) && rb.velocity.y < -7f)
        {
            playerState = State.crouching;
        }


        else if (playerState == State.hurt)
        {
            if (Mathf.Abs(rb.velocity.x) < .1f)
            {
                playerState = State.idle;
            }
        }

        //Moving
        else if (Mathf.Abs(rb.velocity.x) > 2f)

        {
            playerState = State.running;

            if (rb.velocity.y < -6)
            {

                playerState = State.falling;
            }

            //Falling from a high place
        }

        else if (playerState == State.idle)
        {
            if (rb.velocity.y < -6)
            {

                playerState = State.falling;
            }

            //Falling from a high place

        }
        else
        {
            playerState = State.idle;
        }
    }

    //Public Functions
    public void playerHurt()
    {
        playerState = State.hurt;
        PermanentUI.perm.health -= 1;
        PermanentUI.perm.healthCounterText.text = PermanentUI.perm.health.ToString();

    }

    public void Hit()
    {
        hit.Play();
    }

    public void Death()
    {
        if (PermanentUI.perm.health <= 0)
        {
            SceneManager.LoadScene(4);
            StartCoroutine(DeathTimer());
            deathTrigger = true;
            /*
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            */
            PermanentUI.perm.Reset();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Cherry cherry = collision.gameObject.GetComponent<Cherry>();

        if (collision.gameObject.tag == "Gem")
        {
            Destroy(collision.gameObject);
            GetComponent<SpriteRenderer>().color = Color.blue;
            invincibleTrigger = true;
            speed = 10f;
            StartCoroutine("GemTimer");
        }



    }

    private IEnumerator GemTimer()
    {
        yield return new WaitForSeconds(5);
        invincibleTrigger = false;
        speed = 5f;
        GetComponent<SpriteRenderer>().color = Color.white;
        

    }

    private IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(10f);
    }

    //Audio
    private void Footsteps()
    {
        footsteps.Play();

    }



    


}




