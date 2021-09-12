using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private BoxCollider2D coll;
    private Rigidbody2D rb;

    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;
    private void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()

    {
        if(rb.position.x <= leftCap)
        {
            rb.velocity = new Vector2(2, 0);
        }

        else if(rb.position.x >= rightCap)
        {
            rb.velocity = new Vector2(-2, 0);
        }
    }











}
        




