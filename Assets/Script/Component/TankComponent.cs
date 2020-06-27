using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TankComponent : ComponentBase
{
    public Bodywork bodywork;
    public GameObject gun;
    [System.NonSerialized] public Entity gunEntity;
    public float spinSpeed = 1.0f;
    public Animator trackLeftAnimator;
    public Animator trackRightAnimator;
}
