using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem2 : MonoBehaviour
{
    public Collider2D coll;
    public Animator anim;
    PlayerController playercontroller;
    GameObject player = GameObject.FindWithTag("Player");
    void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }    


    public void OnTriggerEnter2D(Collider2D collision)
    {

        PlayerController player = collision.gameObject.GetComponent<PlayerController>();

        if (collision.gameObject.tag == "Player")
        {
            if (collision == player.boxColl)
            {
                Destroy();
                player.jumpForce = 20f;
                player.GetComponent<SpriteRenderer>().color = Color.red;
                StartCoroutine(Timer());

            }
        }



    }


    public IEnumerator Timer()
    {
        playercontroller = player.GetComponent<PlayerController>();
        yield return new WaitForSeconds(5);
        playercontroller.jumpForce = 11f;
        playercontroller.GetComponent<SpriteRenderer>().color = Color.white;


    }

    public void Destroy()
    {
        Destroy(this.gameObject);
        
    }

}








