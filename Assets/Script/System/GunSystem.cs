using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSystem : SystemBase
{
    public GunSystem(GameWorld world) : base(world)
    {
    }

    public void InitGun(int gunID, GunComponent gunComponent, GameobjectComponent gameobjectComponent)
    {
        if(gunComponent == null || gunComponent.enable == false)
        {
            return;
        }

        //EquipGun(EquipmentManager.GetInstance().GetGun(1), gunComponent, gameobjectComponent);

        Debug.Log("GunSystem.Init");
    }

    public void Update(GunComponent gunComponent,InputComponent input, GameobjectComponent gameobjectComponent)
    {
        if (gunComponent == null || gunComponent.enable == false)
        {
            return;
        }

        if (gunComponent.entity.identity.isAIControl == false)
        {
            SetDirection(gunComponent, input, gameobjectComponent.transform);
        }

        gunComponent.laserTimer.Update();
        if (gunComponent.laserTimer.isRunning == false)
        {
            if (InputSystem.GetHoldAction(input, "laser"))
            {
                gunComponent.laserTimer.Enter();
                EmitLaser(gunComponent, gameobjectComponent.transform);
                return;
            }
        }

        gunComponent.fireTimer.Update();
        if (gunComponent.fireTimer.isRunning == false)
        {
            if (InputSystem.GetHoldAction(input, "fire"))
            {
                gunComponent.fireTimer.Enter();
                Fire(gunComponent, gameobjectComponent.transform);
            }   
        }

        
        
    }

    protected void SetDirection(GunComponent gunComponent, InputComponent input, Transform transform)
    {
        Vector3 shotPosition;
        shotPosition = Input.mousePosition;
        shotPosition = Camera.main.ScreenToWorldPoint(shotPosition);

        RotateGun(transform, shotPosition, 1.0f);
    }

    public void RotateGun(Transform transform, Vector3 shotPosition, float accuracy)
    {
        float deltaAngle = Vector2.SignedAngle(transform.up, shotPosition - transform.position);
        if (Mathf.Abs(deltaAngle) >= accuracy)
        {
            transform.Rotate(new Vector3(0, 0, Mathf.Sign(deltaAngle) * 1.0f));
        }
    }

    protected void Fire(GunComponent gunComponent, Transform transform)
    {
        //Debug.Log("GunSystem.Fire");
        Vector3 bulletPos = transform.position + transform.up * gunComponent.fireOffset * transform.lossyScale.x;
        Entity bullet = world.entitySystem.CreateFrom(gunComponent.bulletPrefab, bulletPos);
        bullet.gameobjectComponent.transform.rotation = transform.rotation;
        bullet.bulletComponent.buffName = gunComponent.gunData.bulletBuff;
        bullet.bulletComponent.damage = gunComponent.firePower;
        bullet.identity.master = gunComponent.entity.identity.master;
        bullet.identity.camp = bullet.identity.master.identity.camp;
        Entity fireEffect = world.effectSystem.CreateFireEffect(bulletPos, 0, transform.rotation);
        fireEffect.gameobjectComponent.transform.SetParent(transform);
        gunComponent.entity.AddChild(fireEffect);
    }

    protected void EmitLaser(GunComponent gunComponent, Transform transform)
    {
        Vector3 startingPosition = transform.position + transform.up * (gunComponent.fireOffset - 5) * transform.lossyScale.x;
        Entity laser = world.entitySystem.CreateFrom(gunComponent.laserPrefab, startingPosition);
        laser.gameobjectComponent.transform.rotation = transform.rotation;
        laser.identity.master = gunComponent.entity.identity.master;
        laser.identity.camp = laser.identity.master.identity.camp;
        laser.laserComponent.exisingTime += Time.time;
        laser.gameobjectComponent.transform.SetParent(transform);
        gunComponent.laserEntity = laser;
        gunComponent.entity.AddChild(laser);
        Entity fireEffect = world.effectSystem.CreateFireEffect(startingPosition, 1, transform.rotation);
        fireEffect.gameobjectComponent.transform.SetParent(transform);
    }

    protected void EquipGun(GunData gunData, GunComponent gunComponent, GameobjectComponent gameobjectComponent)
    {
        gunComponent.gunData = gunData;
        var sprite = Resources.Load<Sprite>(gunData.spritePath);
        gameobjectComponent.spriteRenderer.sprite = sprite;
        Debug.Log("EquipGun:" + gunData.spritePath);
    }
}
