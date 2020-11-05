using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class Frog : Enemy
{
    //Start() Variables
    private Collider2D coll;

    //Inspector Variables
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;
    [SerializeField] private float jumpLength;
    [SerializeField] private float jumpHeight;
    [SerializeField] private LayerMask ground;

    protected override void Start()

    {
        base.Start();
        coll = GetComponent<Collider2D>();
    }

    private bool facingLeft = true;

    private void Update()
    {
        if (anim.GetBool("jumping"))
        {
            if (rb.velocity.y < .1f)
            {
                anim.SetBool("falling", true);
                anim.SetBool("jumping", false);

            }
        }

        if(coll.IsTouchingLayers(ground) && anim.GetBool("falling"))
        {
            anim.SetBool("falling", false);
        }
    }

    private void Move()
    {
        if (facingLeft)
        {
            //Test to see if beyond the leftCap
            if (transform.position.x > leftCap)
            {
                //Making sure sprite is facing the right location
                if (transform.localScale.x != 1 && coll.IsTouchingLayers(ground))
                {
                    transform.localScale = new Vector3(1, 1);
                }
                if (coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(-jumpLength, jumpHeight);
                    anim.SetBool("jumping", true);
                }

            }
            else
            {
                facingLeft = false;
            }
        }

        else
        {
            //Test to see if beyond the rightCap
            if (transform.position.x < rightCap)
            {
                //Making sure sprite is facing the left location
                if (transform.localScale.x != -1 && coll.IsTouchingLayers(ground))
                {
                    transform.localScale = new Vector3(-1, 1);
                }
                if (coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(jumpLength, jumpHeight);
                    anim.SetBool("jumping", true);
                }

            }
            else
            {
                facingLeft = true;
            }
        }
    }

}
