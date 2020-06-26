using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveSystem : SystemBase
{
    public MoveSystem(GameWorld world) : base(world)
    {

    }

    public void Update(MoveComponent move, GameobjectComponent gameobjectComponent)
    {
        if(move == null || move.enable == false)
        {
            return;
        }
        Transform transform = gameobjectComponent.transform;
        BoxCollider2D collider = gameobjectComponent.collider;
        if (move.needMove == true)
        {
            Vector3 directionVec;
            if(move.moveDirection == 1)
            {
                directionVec = transform.up;
            }
            else
            {
                directionVec = -transform.up;
            }
            Vector3 nextPosition = transform.position + directionVec * move.moveSpeed * Time.fixedDeltaTime;
            //Vector3 nextPosition = transform.position + transform.up * move.moveSpeed * Time.fixedDeltaTime;
            if (move.collisionDetection == true)
            {
                collider.enabled = false;
                Collider2D otherCollider = Physics2D.OverlapBox(nextPosition, collider.size, transform.rotation.eulerAngles.z);
                if (otherCollider != null && !otherCollider.tag.Equals("tree"))
                {
                    nextPosition = transform.position;
                }
            }
            transform.position = nextPosition;
            
            if(collider.enabled == false)
            {
                collider.enabled = true;
            }

        }
    }

    public static void MoveForward(Transform transform, float speed)
    {
        transform.position += transform.up * speed * Time.fixedDeltaTime;
    }

    public static void MoveBack(Transform transform, float speed)
    {
        transform.position += -transform.up * speed * Time.fixedDeltaTime;
    }
}
