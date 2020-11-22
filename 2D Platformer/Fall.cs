using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController player = GetComponent<PlayerController>(); 
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            PermanentUI.perm.Reset();

            if(PermanentUI.perm.health == 0)
            {
                SceneManager.LoadScene(4);
            }

        }
    }
}

