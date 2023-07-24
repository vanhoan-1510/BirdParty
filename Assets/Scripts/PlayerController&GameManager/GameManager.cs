using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerController player;
    public GameObject playerObject;


    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (!Instance) //if (Instance == null)
        {
            Instance = this;
        }
    }
}
