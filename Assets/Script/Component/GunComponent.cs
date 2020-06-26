using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GunComponent : ComponentBase
{
    public GameObject bulletPrefab;
    public GameObject laserPrefab;
    [System.NonSerialized] public Entity laserEntity = null;
    public float fireCD = 0.8f; // 2.0f
    public float laserCD = 8f; // 2.0f
    public float fireOffset = 22f;
    public float rotationSpeed = 10f;
    public float firePower = 48;
    public float timeCount; // 0f
    public Timer fireTimer;
    public Timer laserTimer;
    public GunData gunData;

    public GunComponent()
    {
        fireTimer = new Timer(fireCD);
        laserTimer = new Timer(laserCD);
    }
}
