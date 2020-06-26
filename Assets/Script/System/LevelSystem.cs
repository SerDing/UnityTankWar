using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : SystemBase
{
    public LevelSystem(GameWorld world) : base(world)
    {
        CreateFloor();
        CreateDestructibles();
        CreateEnemies();
        

        Entity player = world.entitySystem.CreateTank(world.tankPrefab, new Vector3(300, 240, 0));
        player.identity.tag = "player";
        PlayerMgr.getInstance().SetPlayer(player);

    }

    public void CreateFloor()
    {
        float size = 128;
        GameObject tile; // world.tiles
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                tile = world.tiles[Random.Range(0, 2)];
                GameObject.Instantiate(tile).transform.position = new Vector3(size + i * size, size + j * size, 0);
            }
        }

        GameObject airwallLeft = GameObject.Instantiate(world.tiles[4]);
        GameObject airwallRight = GameObject.Instantiate(world.tiles[4]);
        GameObject airwallUp = GameObject.Instantiate(world.tiles[4]);
        GameObject airwallDown = GameObject.Instantiate(world.tiles[4]);
        airwallLeft.GetComponent<BoxCollider2D>().size = new Vector2(128, 128 * 20);
        airwallRight.GetComponent<BoxCollider2D>().size = new Vector2(128, 128 * 20);
        airwallUp.GetComponent<BoxCollider2D>().size = new Vector2(128 * 20, 128);
        airwallDown.GetComponent<BoxCollider2D>().size = new Vector2(128 * 20, 128);

        airwallLeft.transform.position = new Vector3(0, 128 * 20 / 2 + 128 / 2);
        airwallRight.transform.position = new Vector3(128 * 20 + 128, 128 * 20 / 2 + 128 / 2);
        airwallUp.transform.position = new Vector3(128 * 20 / 2 + 128 / 2, 128 * 20 + 128);
        airwallDown.transform.position = new Vector3(128 * 20 / 2 + 128 / 2, 0);
    }

    public void CreateDestructibles()
    {
        //world.entitySystem.CreateFrom(world.stone, new Vector3(-300, 39, 0));

        //GameObject destructible; // world.destructibles
        //for (int i = 0; i < 10; i++)
        //{
        //    for (int j = 0; j < 10; j++)
        //    {
        //        int random = Random.Range(0, 3);
        //        destructible = world.destructibles[random];
        //        world.entitySystem.CreateFrom(destructible, new Vector3(32.0f + i * 32.0f, 32.0f + j * 32.0f, 0));
        //    }
        //}
    }
    
    public void CreateEnemies()
    {
        for (int i = 0; i < 5; i++)
        {
            float rangeX = 1000;
            float rangeY = 1000;
            float x = Random.Range(600, 600 + rangeX);
            float y = Random.Range(500, 500 + rangeY);
            CreateEnemy(x, y, 24);
        }
        CreateBoss(1512, 768, 32);
    }

    public void CreateBoss(float x, float y, float firePower)
    {
        Entity enemy = world.entitySystem.CreateEnemyTank(world.enemyPrefabs[1], new Vector3(x, y, 0));
        enemy.identity.camp = 1;
        enemy.identity.tag = "Boss";
        enemy.tankComponent.gunEntity.gunComponent.firePower = firePower;
        world.aiSystem.AddAi(enemy.aiComponent, new BossAI(enemy));
    }

    public void CreateEnemy(float x, float y, float firePower)
    {
        Entity enemy = world.entitySystem.CreateEnemyTank(world.enemyPrefabs[0], new Vector3(x, y, 0));
        enemy.identity.camp = 1;
        enemy.identity.tag = "enemy";
        enemy.tankComponent.gunEntity.gunComponent.firePower = firePower;
        enemy.tankComponent.gunEntity.gunComponent.fireCD = 5.0f;
        world.aiSystem.AddAi(enemy.aiComponent, new NormalAI(enemy));

    }

    public void Init(LevelComponent level)
    {

    }


    public void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            float rangeX = 400;
            float rangeY = 350;
            float x = Random.Range(300, 300 + rangeX);
            float y = Random.Range(200, 200 + rangeY);
            CreateEnemy(x, y, 24);
            Debug.Log("Create Enemy");
        }
    }
}
