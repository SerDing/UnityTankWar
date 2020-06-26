using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class AiComponent : ComponentBase
{
    
    public float timeCount;
    public float accuracy = 12.0f;
    public float laserCD = 8.0f;
    public float laserTimer = 5.0f;
    public List<AI> ais;

    public AiComponent()
    {
        ais = new List<AI>();
    }
}
