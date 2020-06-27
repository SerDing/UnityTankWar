using UnityEngine;


public class BossAI : NormalAI
{
    public SpriteRenderer spriteRenderer;
    public BossAI(Entity entity) : base(entity)
    {
        spriteRenderer = entity.gameobjectComponent.spriteRenderer;
    }

    public override void Update()
    {
        //base.Update();
        float distance;
        Vector3 chasingPosition;
        Vector3 relativeDirection;
        Vector3 shotPosition;

        switch (curState)
        {
            case STATE.ROAMING:
                distance = Vector2.Distance(selfTransform.position, roamingPosition);
                if (distance == lastDistance)
                {
                    stopMoveTime += Time.deltaTime;
                    if (stopMoveTime > 2.0f)
                    {
                        stopMoveTime = 0;
                        roamingPosition = GetRoamingPosition();
                        break;
                    }
                }
                if (distance >= reachedRange)
                {
                    lastDistance = distance;
                    MoveTo(roamingPosition);
                }
                else
                {
                    roamingPosition = GetRoamingPosition();
                }

                target = FindTarget();
                if (target != null)
                {
                    targetTransform = target.gameobjectComponent.transform;
                    curState = STATE.CHASING;
                }
                else
                {
                    world.gunSystem.RotateGun(gunTransform, roamingPosition, 2.0f);
                }

                break;
            case STATE.CHASING:

                if (target.identity.isDead == true)
                {
                    target = null;
                    targetTransform = null;
                    curState = STATE.ROAMING;
                    break;
                }

                //chasingPosition = targetTransform.position + targetTransform.right * sideChasingOffset;
                relativeDirection = (selfTransform.position - targetTransform.position).normalized;
                chasingPosition = targetTransform.position + relativeDirection * sideChasingOffset;
                distance = Vector2.Distance(selfTransform.position, chasingPosition);
                shotPosition = targetTransform.position;
                world.gunSystem.RotateGun(gunTransform, shotPosition, 12.0f);
                if (distance >= attackRange)
                {
                    MoveTo(chasingPosition);
                }
                else
                {
                    //entity.aiComponent.laserTimer += Time.deltaTime;
                    //Debug.Log("BossAI.laserTimer:" + entity.aiComponent.laserTimer.ToString());
                    //if (entity.aiComponent.laserTimer >= entity.aiComponent.laserCD)
                    //{
                    //    entity.aiComponent.laserTimer = 0;
                    //    Debug.Log("BossAI.LaserAttack");
                    //    InputSystem.PressAction(entity.tankComponent.gunEntity.inputComponent, "laser");
                    //}
                    //else
                    //{
                    //    if (entity.gunComponent.laserEntity == null || entity.gunComponent.laserEntity.identity.isDead == true)
                    //    {
                    //        InputSystem.PressAction(entity.tankComponent.gunEntity.inputComponent, "fire");
                    //    }
                    //}

                    InputSystem.PressAction(entity.tankComponent.gunEntity.inputComponent, "laser");
                    Debug.Log("----press laser, targetTag:" + target.identity.tag);
                    //if (entity.gunComponent.laserEntity == null || entity.gunComponent.laserEntity.identity.isDead == true)
                    //{
                    //    InputSystem.PressAction(entity.tankComponent.gunEntity.inputComponent, "fire");
                    //}

                }

                break;
            default:
                break;
        }
    }

}