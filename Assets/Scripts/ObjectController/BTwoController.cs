﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTwoController : MonoBehaviour
{
    public float rotationSpeed;

    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.Self);
    }
}
