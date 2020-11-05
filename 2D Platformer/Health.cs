using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int health = 5;
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    

}
