using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class Opossum : Enemy
{
    //Start() Variables
    private Collider2D coll;

    //Inspector Variables
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;
    [SerializeField] private LayerMask ground;

    protected override void Start()

    {
        base.Start();
        coll = GetComponent<Collider2D>();
    }

    private void Update()
    {
        Move();
    }


    private void Move()
    {
        if (transform.localScale.x == -1)
        {
            if (transform.position.x < rightCap)
            {
                rb.velocity = new Vector2(2, 0);
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }
            }
            else
            {
                transform.localScale = new Vector3(1, 1);
            }

        }

        else
        {
            if (transform.position.x > leftCap)
            {
                rb.velocity = new Vector2(-2, 0);
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }
            }

            else
            {
                transform.localScale = new Vector3(-1, 1);
            }

        }
    }

}


