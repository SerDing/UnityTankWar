using UnityEngine;
using System.Collections;

public class LifeSystem : SystemBase
{
    public LifeSystem(GameWorld world) : base(world)
    {
    }

    public void ChangeHP(LifeComponent life, float delta)
    {
        life.currentHP = life.currentHP + delta;
        if (life.currentHP >= life.maxHP)
        {
            life.currentHP = life.maxHP;
        }
        if (life.currentHP <= 0)
        {
            life.currentHP = 0;
            life.entity.identity.isDead = true;
            world.entitySystem.DestroyEntity(life.entity);
            if (life.entity.tankComponent.enable == true)
            {
                world.entitySystem.DestroyEntity(life.entity.tankComponent.gunEntity);
                world.effectSystem.CreateExplosion(life.entity.gameobjectComponent.transform.position, "large");
            }
        }
        Debug.Log("LifeSystem.ChangeHp:" + life.entity.identity.tag + " " + delta);
    }

    public void SetData(LifeComponent life, float hp)
    {
        life.currentHP = hp;
        life.maxHP = hp;
    }
}
