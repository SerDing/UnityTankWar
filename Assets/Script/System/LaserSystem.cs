using UnityEngine;
using System.Collections;

public class LaserSystem : SystemBase
{
    public LaserSystem(GameWorld world) : base(world)
    {
    }

    public void Update(Identity identity, LaserComponent laser, GameobjectComponent gameobjectComponent)
    {
        if (laser == null || laser.enable == false)
        {
            return;
        }

        if (Time.time < laser.exisingTime)
        {

            laser.attackTimer += Time.deltaTime;
            if (laser.attackTimer >= laser.attackInterval)
            {
                laser.attackTimer = 0;
                Transform transform = gameobjectComponent.transform;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up);
                Debug.DrawLine(transform.position, transform.up * 700);
                if (hit.collider != null)
                {
                    //Debug.DrawLine(transform.position, hit.point);
                    EntityHolder entityHolder = hit.collider.gameObject.GetComponent<EntityHolder>();
                    if (entityHolder == null)
                    {
                        return;
                    }
                    Entity hitEntity = entityHolder.entity;
                    if (hitEntity != identity.master)
                    {
                        Debug.Log("LaserSystem, camps:" + hitEntity.identity.camp.ToString() + identity.camp.ToString());
                        if (hitEntity.identity.camp != identity.camp && hitEntity.lifeComponent.enable == true)
                        {
                            world.lifeSystem.ChangeHP(hitEntity.lifeComponent, -laser.damage);
                            world.effectSystem.CreateExplosion(hit.point, "small");
                        }
                    }
                }
            }
        }
        else
        {
            world.entitySystem.DestroyEntity(identity.entity);
        }

    }
}
