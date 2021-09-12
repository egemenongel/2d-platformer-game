using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crank : MonoBehaviour
{
    private Animator anim;
    private Collider2D coll;

    private void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }

    private void Update()
    {
        
    }

    public void ae()
    {
        
    }
    public void CrankActivated()
    {
        
        if(anim.GetBool("active"))
        {
            anim.SetBool("active", true);
        }
        else
        {
            anim.SetBool("active", false);
        }
        


        

        
    }



}
