using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private Collider2D coll;
    private Rigidbody2D rb;
    private float hurtForce = 10f;



    void Start()
    {
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();

        if(collision.gameObject.tag == "Player")
        {
            if(player.invincibleTrigger == true)
            {
                coll.isTrigger = true;
            }

            else

            if (collision.collider == player.boxColl || collision.collider == player.circleColl)
            {



                if (player.gameObject.transform.position.x < transform.position.x)
                {
                    player.rb.velocity = new Vector2(hurtForce, 5);
                }

                else
                {
                    player.rb.velocity = new Vector2(-hurtForce, 5);
                }


                player.playerHurt();
                player.Death();
                player.playerState = PlayerController.State.hurt;
                player.Hit();







            }
        }
    }



}
