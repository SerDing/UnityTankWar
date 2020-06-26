using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI
{
    public Entity entity;
    public Transform selfTransform;

    public GameWorld world;
    public AI(Entity entity)
    {
        this.entity = entity;
        selfTransform = entity.gameobjectComponent.transform;
        Debug.Log("AI base construction.");
    }

    public virtual void Update(){

    }
}

class Singleton<T> where T : class, new()
{
    private static T _instance;
    private static readonly object syslock = new object();

    public static T getInstance()
    {
        if (_instance == null)
        {
            lock (syslock)
            {
                if (_instance == null)
                {
                    _instance = new T();
                }
            }
        }
        return _instance;
    }
}
