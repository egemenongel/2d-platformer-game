using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerController : MonoBehaviour

{
    //Start() Variables
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;

    //FSM
    private enum State { idle, running, jumping, falling, crouching}
    private State state = State.idle;

    //Inspector Variables
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpforce = 7f;
    [SerializeField] private float crouchForce = -10f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()

    {
        float hDirection = Input.GetAxis("Horizontal");

        //Moving Left
        if (hDirection < 0)

        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }

        //Moving Right
        else if (hDirection > 0)

        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }

        //Jumping
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))

        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
            state = State.jumping;
        }

        float vDirection = Input.GetAxis("Vertical");

        //Crouching
        if (vDirection < 0 && coll.IsTouchingLayers(ground) && state != State.falling)
        {
            rb.velocity = new Vector2(rb.velocity.x, crouchForce);
            state = State.crouching;
        }

        AnimatonState();
        anim.SetInteger("state", (int)state);   

    }

    private void AnimatonState()

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
            if (coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }

        else if (rb.velocity.y < -2f )
        {
            state = State.crouching;
        }

        else if (Mathf.Abs(rb.velocity.x) > 2f )
        {
            //moving
            state = State.running;
        }

        else
        {
            //not moving
            state = State.idle;
        }

    }

}
