using UnityEngine;
using System.Collections;

[System.Serializable]
public class LaserComponent : ComponentBase
{
    public float exisingTime = 5.0f;
    public float damage = 5;
    public float attackInterval = 0.3f;
    public float attackTimer = 0;
}
