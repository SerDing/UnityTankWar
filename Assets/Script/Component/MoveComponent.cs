﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MoveComponent : ComponentBase { 
    public float moveSpeed = 80f;
    public int moveDirection;
    public bool needMove;
    public bool collisionDetection = true;
}
