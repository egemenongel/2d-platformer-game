using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    //Start() Variables
    private Rigidbody2D rb;
    private Collider2D coll;
    public Rigidbody2D playerRb;
    
    [SerializeField]public Vector3 teleportLocation;

    private void Start()
    {
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();          
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            playerRb.transform.position = teleportLocation;
            player.rb.velocity = new Vector2(player.rb.position.x, player.rb.position.y);

        }
    }



}


 