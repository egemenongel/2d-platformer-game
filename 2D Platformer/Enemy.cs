using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rb;
    protected AudioSource death;

    [SerializeField] public float hurtForce = 10f;


    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        death = GetComponent<AudioSource>();
    }

    public void enemyDeath()
    {
        anim.SetTrigger("Death");
        death.Play();
        rb.bodyType = RigidbodyType2D.Static;
        GetComponent<Collider2D>().enabled = false;
    }

    private void Death()
    {
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();

        if (collision.gameObject.tag == "Player")
        {
            if (collision.collider == player.boxColl)
            {
                if(player.playerState == PlayerController.State.falling)
                {
                    enemyDeath();
                    player.playerJump();
                }
                
                else if (player.invincibleTrigger == true)
                {
                    enemyDeath();
                }

                else
                {

                    if (transform.position.x > player.gameObject.transform.position.x)
                    {
                        player.rb.velocity = new Vector2(-hurtForce, player.rb.velocity.y);
                        // enemy is right to the player, bounce to left
                    }

                    else
                    {
                       player.rb.velocity = new Vector2(hurtForce, player.rb.velocity.y);
                        // enemy is left to the player, bounce to right
                    }

                    player.playerHurt();
                    player.Death();
                    player.Hit();



                }

            }

        }


    }


}
