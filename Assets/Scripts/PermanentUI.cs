using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
public class PermanentUI : MonoBehaviour
{
    public int collectables = 0;
    public TextMeshProUGUI collectablesText;
    public int health = 3;
    public Text healthCounterText;

    public static PermanentUI perm;

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
            
    }

    public void Reset()
    {
        health -=1;
        collectables = 0;
        collectablesText.text = collectables.ToString();
    }

}
