using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private void Start()
    {
        PermanentUI.perm.gameObject.SetActive(false);
    }
    public void Retry()
    {
        PermanentUI.perm.gameObject.SetActive(true);

        switch (PermanentUI.perm.levelCounter)
        {
            case 1:
                SceneManager.LoadScene(1);
                break;
            case 2:
                SceneManager.LoadScene(2);
                break;
            case 3:
                SceneManager.LoadScene(3);
                break;
        }

        PermanentUI.perm.health = 3;
      
    }
    
    public void Return()
    {
        SceneManager.LoadScene(0);
    }


 

}
