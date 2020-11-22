using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
public class PermanentUI : MonoBehaviour
{
    [HideInInspector] public int collectables = 0;
    [HideInInspector] public TextMeshProUGUI collectablesText;
    [HideInInspector] public int health = 3;
    [HideInInspector] public Text healthCounterText;
    [HideInInspector] public TextMeshProUGUI levelText;
    [HideInInspector] public int levelCounter = 1;
    [HideInInspector] public static PermanentUI perm;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);


        if (!perm)
        {
            perm = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        /*
         * Created to determine level in the beginning of the scene.
         * 
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 1:
                levelCounter = 1;
                break;
            case 2:
                levelCounter = 2;
                break;
            case 3:
                levelCounter = 3;
                break;
        }
        */

    }

    public void Reset()
    {
        health -=1;
        collectables = 0;
        collectablesText.text = collectables.ToString();
    }

}
