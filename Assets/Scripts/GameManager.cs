using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerController player;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        //anim = gameObject.GetComponent<Animation>();
        if (!Instance) //if (Instance == null)
        {
            Instance = this;
        }
    }
}
