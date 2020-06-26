using UnityEngine;
using System.Collections;

public class EffectSystem : SystemBase
{
    public EffectSystem(GameWorld world) : base(world)
    {
    }

    public void Update(Identity identity, EffectComponent effect, GameobjectComponent gameobjectComponent)
    {
        if (effect.enable == false)
        {
            return;
        }

        Animator animator = gameobjectComponent.animator;
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 1.0f)
        {
            world.entitySystem.DestroyEntity(identity.entity);
        }
    }

    public Entity CreateExplosion(Vector3 position, string type)
    {
        GameObject prefab = null;
        if (type == "large")
        {
            prefab = world.explosionLarge;
        }
        else if (type == "small")
        {
            prefab = world.explosionSmall;
        }
        return CreateEffect(position, prefab);
    }

    public Entity CreateFireEffect(Vector3 position, int n, Quaternion rotation)
    {
        Entity effect = CreateEffect(position, world.effects[n]);
        effect.gameobjectComponent.transform.rotation = rotation;
        return effect;
    }

    public Entity CreateEffect(Vector3 position, GameObject prefab)
    {
        position.z += 1;
        Entity effect = world.entitySystem.CreateFrom(prefab, position);
        return effect;
    }
}
