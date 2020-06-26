using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSystem : SystemBase
{
    public BulletSystem(GameWorld world) : base(world)
    {
    }

    public void Init()
    {
    }

    public void Update(Identity identity, BulletComponent bullet, MoveComponent move, GameobjectComponent gameobjectComponent)
    {
        if (bullet == null || bullet.enable == false)
        {
            return ;
        }

        Transform transform = gameobjectComponent.transform;
        BoxCollider2D boxCollider2D = gameobjectComponent.collider;

        if (!identity.isDead)
        {
            boxCollider2D.enabled = false;
            Collider2D otherCollider = Physics2D.OverlapBox(transform.position, boxCollider2D.size, transform.rotation.eulerAngles.z);
            if (otherCollider != null)
            {
                OnCollision(otherCollider, identity, bullet, boxCollider2D, move);
            }
            boxCollider2D.enabled = true;

            MoveSystem.MoveForward(transform, move.moveSpeed);
            bullet.currentDistance += move.moveSpeed * Time.fixedDeltaTime;
            if (bullet.currentDistance > bullet.shootDistance)
            {
                world.entitySystem.DestroyEntity(identity.entity);
            }
        }
    }

    protected void OnCollision(Collider2D other, Identity identity, BulletComponent bulletComponent, BoxCollider2D boxCollider2D, MoveComponent move)
    {
        EntityHolder entityHolder = other.gameObject.GetComponent<EntityHolder>();
        if (entityHolder == null)
        {
            return;
        }
        Entity otherEntity = entityHolder.entity;
        Identity otherIdentity = otherEntity.identity;

        if(otherEntity == identity.master)
        {
            return;
        }

        if (otherIdentity.tag != "river")
        {
            move.needMove = false;
            identity.isDead = true;
        }

        if (otherIdentity.tag == "brick" || otherIdentity.tag == "stone")
        {
            if (other.gameObject.tag == "brick")
            {
                world.entitySystem.DestroyEntity(otherEntity);
            }
        }
        else if (otherEntity != identity.master && otherEntity.lifeComponent.enable == true)
        {
            world.lifeSystem.ChangeHP(otherEntity.lifeComponent, -bulletComponent.damage);
            world.buffSystem.AddBuff(otherEntity, bulletComponent.buffName);
            Debug.Log("bullet hit:" + otherIdentity.tag);
        }

        if (identity.isDead == true)
        {
            boxCollider2D.enabled = false; // disable collider to avoid mutiple times collision.
            world.entitySystem.DestroyEntity(identity.entity);
            world.effectSystem.CreateExplosion(bulletComponent.entity.gameobjectComponent.transform.position, "small");
        }
    }
    
}
