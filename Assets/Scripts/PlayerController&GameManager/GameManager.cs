using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerController player;
    public Camera cam;

    [Header("BounceObject")]
    public GameObject parentObjLeft;
    public GameObject parentObjRight;
    public GameObject parentBTwoObject;
    public GameObject parentTrampolineObject;
    public GameObject spindleTrap;
    public GameObject parentTrampolineObjectVTwo;
    public GameObject parentGroundBounceRight;
    public GameObject parentDeathObject;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (!Instance) //if (Instance == null)
        {
            Instance = this;
        }
    }
}
