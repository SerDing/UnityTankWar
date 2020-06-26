using UnityEngine;
using System.Collections;

public class NormalAI : AI
{
    public enum STATE
    {
        ROAMING,
        CHASING,
        LASER
    }

    public Entity gunEntity;
    public Vector3 initPosition;

    public STATE curState;
    public float roamingRange = 300f;
    public Vector3 roamingPosition;
    public float reachedRange = 35f;

    public float visionRadius = 300f;
    public float attackRange = 100f;
    public float sideChasingOffset = 150;
    public float lastDistance = 0;
    public float stopMoveTime = 0;
    public Transform gunTransform = null;
    public Entity target = null;
    public Transform targetTransform = null;

    public float reactionTime = 0.8f;

    public NormalAI(Entity entity):base(entity)
    {
        initPosition = entity.gameobjectComponent.transform.position;
        roamingPosition = GetRoamingPosition();
        curState = STATE.ROAMING;
        gunTransform = entity.tankComponent.gunEntity.gameobjectComponent.transform;
        gunEntity = entity.tankComponent.gunEntity;
    }

    public override void Update()
    {
        float distance;
        Vector3 chasingPosition;
        Vector3 relativeDirection;
        Vector3 shotPosition;

        switch (curState)
        {
            case STATE.ROAMING:
                {
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
                }
            case STATE.CHASING:
                {
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
                        InputSystem.PressAction(entity.tankComponent.gunEntity.inputComponent, "fire");
                    }

                    break;
                }
            default:
                break;
        }
    }

    public Vector3 GetRoamingPosition()
    {
        float offsetX = Random.Range(-roamingRange, roamingRange);
        float offsetY = Random.Range(-roamingRange, roamingRange);
        return new Vector3(initPosition.x + offsetX, initPosition.y + offsetY, 0);
    }

    public void MoveTo(Vector3 targetPosition)
    {
        //Debug.Log("MoveTo: " + targetPosition);
        float angle = Vector2.SignedAngle(selfTransform.up, targetPosition - selfTransform.position);
        if (Mathf.Abs(angle) > 1.0f)
        {
            if (angle > 0)
            {
                InputSystem.PressAction(entity.inputComponent, "move_left");
            }
            else if (angle < 0)
            {
                InputSystem.PressAction(entity.inputComponent, "move_right");
            }
        }
        else
        {
            InputSystem.PressAction(entity.inputComponent, "move_up");
        }
    }

    public Entity FindTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(selfTransform.position, visionRadius);
        Entity otherEntity;
        foreach (var collider in colliders)
        {
            EntityHolder entityHolder = collider.gameObject.GetComponent<EntityHolder>();
            if (entityHolder == null)
            {
                return null;
            }
            otherEntity = entityHolder.entity;
            if (otherEntity.tankComponent != null && otherEntity.identity.camp != entity.identity.camp)
            {
                return otherEntity;
            }
        }

        return null;
    }
}
