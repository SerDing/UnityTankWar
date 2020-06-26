using UnityEngine;
public class EntitySystem : SystemBase
{
    public EntitySystem(GameWorld world) : base(world)
    {
    }

    public Entity CreateTank(GameObject prefab, Vector3 position)
    {
        Entity tank = CreateFrom(prefab, position);

        world.controllerSystem.InitBodywork(1, tank.tankComponent, tank.moveComponent, tank.gameobjectComponent);
        Entity gunEntity = world.controllerSystem.CreateGunEntity(tank.gameobjectComponent);
        world.gunSystem.InitGun(1, gunEntity.gunComponent, gunEntity.gameobjectComponent);
        tank.tankComponent.gunEntity = gunEntity;
        gunEntity.identity.master = tank;
        gunEntity.gunComponent.fireTimer.Enter(gunEntity.gunComponent.fireCD);
        gunEntity.gunComponent.laserTimer.Enter(gunEntity.gunComponent.laserCD);

        Debug.Log("CreateTank: " + tank.identity.tag);
        return tank;
    }

    public Entity CreateEnemyTank(GameObject prefab, Vector3 position)
    {
        Entity tank = CreateTank(prefab, position);
        tank.identity.isAIControl = true;
        tank.inputComponent.isAIControl = true;
        tank.tankComponent.gunEntity.identity.isAIControl = true;
        tank.tankComponent.gunEntity.inputComponent.isAIControl = true;
        return tank;
    }

    public Entity CreateFrom(GameObject prefab, Vector3 position)
    {
        GameObject instance = GameObject.Instantiate(prefab);
        instance.transform.position = position;
        Entity entity = CreateFor(instance);
        return entity;
    }

    public Entity CreateFor(GameObject instance)
    {
        EntityConfig entityConfig = instance.GetComponent<EntityConfig>();
        Entity entity = entityConfig.entity;
        entity.gameobjectComponent.gameObject = instance;
        entity.gameobjectComponent.gameObject.AddComponent<EntityHolder>();
        EntityHolder entityHolder = entity.gameobjectComponent.gameObject.GetComponent<EntityHolder>();
        entityHolder.entity = entity;
        entity.LinkComponentToEntity();
        world.AddEntity(entity);

        //Debug.Log("EntitySystem.CreateFor, tag: " + entity.identity.tag);
        return entity;
    }

    public Entity NewEntity(GameObject prefab, Vector3 position){

        Entity entity = new Entity();
        
        entity.gameobjectComponent.SetGameObject(GameObject.Instantiate(prefab));
        entity.gameobjectComponent.transform.position = position;
        entity.gameobjectComponent.gameObject.AddComponent<EntityHolder>();
        EntityHolder entityHolder = entity.gameobjectComponent.gameObject.GetComponent<EntityHolder>();
        entityHolder.entity = entity;
        
        world.AddEntity(entity);

        ConvertToEntity convertToEntity = entity.gameobjectComponent.gameObject.GetComponent<ConvertToEntity>();
        if(convertToEntity != null)
        {
            convertToEntity.convert = false;
        }

        Debug.Log("NewEntity");
        
        return entity;
    }

    public Entity NewPawn(GameObject prefab, Vector3 position)
    {
        Entity pawn = NewEntity(prefab, position);
        pawn.identity.tag = "pawn";

        pawn.inputComponent = new InputComponent() { entity = pawn };
        pawn.moveComponent = new MoveComponent() { entity = pawn };
        pawn.aiComponent = new AiComponent() { entity = pawn };

        return pawn;
    }

    public Entity NewTank(GameObject prefab, Vector3 position){
        Entity tank = NewPawn(prefab, position);
        tank.identity.tag = "tank";
        tank.tankComponent = new TankComponent() { entity = tank };
        tank.buffComponent = new BuffComponent() { entity = tank };

        world.controllerSystem.InitBodywork(1, tank.tankComponent, tank.moveComponent, tank.gameobjectComponent);
        Entity gunEntity = world.controllerSystem.CreateGunEntity(tank.gameobjectComponent);
        world.gunSystem.InitGun(1, gunEntity.gunComponent, gunEntity.gameobjectComponent);
        tank.tankComponent.gunEntity = gunEntity;
        gunEntity.identity.master = tank;

        Debug.Log("NewTank, bulletComponent: " + tank.bulletComponent);
        return tank;
    }

    public Entity NewEnemyTank(GameObject prefab, Vector3 position){
        Entity tank = NewTank(prefab, position);
        tank.identity.isAIControl = true;
        tank.inputComponent.isAIControl = true;
        tank.tankComponent.gunEntity.identity.isAIControl = true;
        tank.tankComponent.gunEntity.inputComponent.isAIControl = true;
        return tank;
    }

    public void DestroyEntity(Entity entity)
    {
        Debug.Log("destroy entity: " + entity.identity.tag);
        entity.identity.isDead = true;
        world.RemoveEntity(entity);
        if (entity.children.Count > 0)
        {
            foreach (var child in entity.children)
            {
                world.entitySystem.DestroyEntity(child);
            }
        }
    }
}
