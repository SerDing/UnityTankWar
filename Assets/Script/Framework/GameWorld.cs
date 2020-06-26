using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWorld : MonoBehaviour
{
    public EntitySystem entitySystem;
    public LevelSystem levelSystem;
    public InputSystem inputSystem;
    public MoveSystem moveSystem;
    public LifeSystem lifeSystem;
    public ControllerSystem controllerSystem;
    public GunSystem gunSystem;
    public BulletSystem bulletSystem;
    public BuffSystem buffSystem;
    public EffectSystem effectSystem;
    public LaserSystem laserSystem;
    public AISystem aiSystem;

    public List<Entity> entities;
    private Queue<Entity> newEntities;
    private Queue<Entity> expiredEntities;

    public GameObject tankPrefab;
    public GameObject[] enemyPrefabs;
    public GameObject[] tiles;
    public GameObject[] effects;
    public GameObject explosionLarge;
    public GameObject explosionSmall;
    public GameObject brick;
    public GameObject stone;
    public GameObject tree;
    public GameObject river;

    private void Awake()
    {
        entities = new List<Entity>();
        newEntities = new Queue<Entity>();
        expiredEntities = new Queue<Entity>();

        entitySystem = new EntitySystem(this);
        inputSystem = new InputSystem(this);
        moveSystem = new MoveSystem(this);
        lifeSystem = new LifeSystem(this);
        controllerSystem = new ControllerSystem(this);
        gunSystem = new GunSystem(this);
        bulletSystem = new BulletSystem(this);
        buffSystem = new BuffSystem(this);
        effectSystem = new EffectSystem(this);
        laserSystem = new LaserSystem(this);
        aiSystem = new AISystem(this);
        levelSystem = new LevelSystem(this);
    }

    void Start()
    {

    }

    void Update()
    {
        if (newEntities.Count > 0)
        {
            int num = newEntities.Count;
            for (int i = 0; i < num; ++i)
            {
                entities.Add(newEntities.Dequeue());
            }
        }

        foreach (Entity entity in entities)
        {

            //Debug.Log("GameWorld.Update, tag:" + entity.identity.tag);
            aiSystem.Update(entity.identity, entity.aiComponent, entity.inputComponent);
            controllerSystem.Update(entity.tankComponent, entity.inputComponent, entity.moveComponent, entity.gameobjectComponent);
            gunSystem.Update(entity.gunComponent, entity.inputComponent, entity.gameobjectComponent);
            bulletSystem.Update(entity.identity, entity.bulletComponent, entity.moveComponent, entity.gameobjectComponent);
            buffSystem.Update(entity.buffComponent);
            effectSystem.Update(entity.identity, entity.effectComponent, entity.gameobjectComponent);
            laserSystem.Update(entity.identity, entity.laserComponent, entity.gameobjectComponent);
            levelSystem.Update();
            inputSystem.Update(entity.inputComponent);
        }

        if (expiredEntities.Count > 0)
        {
            Entity entity = null;
            int num = expiredEntities.Count;
            for (int i = 0; i < num; ++i)
            {
                entity = expiredEntities.Dequeue();
                Debug.Log("remove entity:" + entity.identity.tag);
                entity.gameobjectComponent.Destroy();
                entities.Remove(entity);

            }
        }
    }

    public void AddEntity(Entity entity)
    {
        newEntities.Enqueue(entity);
    }

    public void RemoveEntity(Entity entity)
    {
        expiredEntities.Enqueue(entity);
    }
}
