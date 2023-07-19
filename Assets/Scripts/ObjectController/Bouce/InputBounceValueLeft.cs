using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBounceValueLeft : MonoBehaviour
{
    public float bounceValue = 0f;
    public float bounceAngle = 45f;

    private void Start()
    {
        Debug.Log("InputBounceValueLeft");
    }
    public float GetBounceAngle(float bounceAngle)
    {
        return bounceAngle;
    }
}
