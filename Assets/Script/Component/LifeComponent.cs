using UnityEngine;
using System.Collections;

[System.Serializable]
public class LifeComponent : ComponentBase
{
    public float currentHP = 100.0f;
    public float maxHP = 100.0f;
    public float explosionScale = 1.0f;
}
