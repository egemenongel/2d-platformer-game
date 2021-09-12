using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Cherry : MonoBehaviour
{
    private Animator anim;
    private Collider2D coll;
    private AudioSource audio;

    [SerializeField] public bool trigger;

    private void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();

        if(collision.gameObject.tag == "Player")
        {
            if (collision == player.boxColl)

            {
                Collect();

                PermanentUI.perm.collectables += 1;
                PermanentUI.perm.collectablesText.text = PermanentUI.perm.collectables.ToString();






            }
        }


    }

    public void Collect()

    {
        anim.SetBool("collect", true);
        audio.Play();
    }
    public void Destroy()
    {
        Destroy(this.gameObject);
    }

    private IEnumerator CollectTimer()
    {
        yield return new WaitForSeconds(.5f);
    }
    public void CollectAudio()
    {
        audio.Play();

    }


}
