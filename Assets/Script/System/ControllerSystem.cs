using UnityEngine;
public class ControllerSystem : SystemBase
{
    public ControllerSystem(GameWorld world) : base(world)
    {
    }

    public void InitBodywork(int bodyworkID, TankComponent tank, MoveComponent move, GameobjectComponent gameobjectComponent)
    {
        gameobjectComponent.animator.SetInteger("state", 0); // enter the idle animation
        //gameobjectComponent.animator.SetInteger("init", 2);
        
        //EquipBodywork(EquipmentManager.GetInstance().GetBodywork(bodyworkID), tank, move, gameobjectComponent);
    }

    public Entity CreateGunEntity(GameobjectComponent gameobjectComponent)
    {
        // create the entity for gun
        Transform transform = gameobjectComponent.transform;
        GameObject gunGo = null;
        Transform[] children = transform.GetComponentsInChildren<Transform>();
        foreach (var t in children)
        {
            if (t.tag == "gun")
            {
                gunGo = t.gameObject;
                break;
            }
        }
        Entity gunEntity = world.entitySystem.CreateFor(gunGo);
        //GenerateGun(entity);
        return gunEntity;
    }

    public void Update(TankComponent tank, InputComponent input, MoveComponent move, GameobjectComponent gameobjectComponent)
    {
        if (tank == null || tank.enable == false)
        {
            return;
        }
        //Debug.Log("contorller system update, entity tag: " + tank.entity.identity.tag);
        MoveControl(tank, input, move, gameobjectComponent.transform, gameobjectComponent);    
    }

    void MoveControl(TankComponent tank, InputComponent input, MoveComponent move, Transform transform, GameobjectComponent gameobjectComponent)
    {
        float spinSpeed = tank.spinSpeed;
        bool moveForward = InputSystem.GetHoldAction(input, "move_up");
        bool moveBack = InputSystem.GetHoldAction(input, "move_down");
        bool spinLeft = InputSystem.GetHoldAction(input, "move_left");
        bool spinRight = InputSystem.GetHoldAction(input, "move_right");
        move.needMove = false;
        if (moveForward || moveBack)
        {
            if (moveForward)
            {
                move.moveDirection = 1;
            }
            else if (moveBack)
            {
                move.moveDirection = -1;
            }
            move.needMove = true;
            spinSpeed /= 2;
            gameobjectComponent.animator.SetInteger("state", 1);
            if (tank.trackLeftAnimator != null && tank.trackRightAnimator != null)
            {
                tank.trackLeftAnimator.SetBool("move", true);
                tank.trackRightAnimator.SetBool("move", true);
            }
        }
        else
        {
            gameobjectComponent.animator.SetInteger("state", 0);
            if (tank.trackLeftAnimator != null && tank.trackRightAnimator != null)
            {
                tank.trackLeftAnimator.SetBool("move", false);
                tank.trackRightAnimator.SetBool("move", false);
            }
        }

        if (spinLeft)
        {
            transform.Rotate(new Vector3(0, 0, spinSpeed));
        }
        else if (spinRight)
        {
            transform.Rotate(new Vector3(0, 0, -spinSpeed));
        }


        BoxCollider2D collider = gameobjectComponent.collider;
        if (move.needMove == true)
        {
            Vector3 directionVec;
            if (move.moveDirection == 1)
            {
                directionVec = transform.up;
            }
            else
            {
                directionVec = -transform.up;
            }
            Vector3 nextPosition = transform.position + directionVec * move.moveSpeed * Time.fixedDeltaTime;
            if (move.collisionDetection == true)
            {
                collider.enabled = false;
                Collider2D otherCollider = Physics2D.OverlapBox(transform.position + directionVec * 35, new Vector2(40, 20), transform.rotation.eulerAngles.z);
                if (otherCollider != null && !otherCollider.tag.Equals("tree"))
                {
                    nextPosition = transform.position;
                }
            }
            transform.position = nextPosition;

            if (collider.enabled == false)
            {
                collider.enabled = true;
            }

        }

    }

    //void GenerateGun(Entity entity)
    //{
    //    Entity gunEntity = world.entitySystem.NewPawn(gunPrefab, entity.gameobjectComponent.transform.position);
    //    gunEntity.identity.master = entity;
    //}

    void EquipBodywork(Bodywork bodywork, TankComponent tank, MoveComponent move, GameobjectComponent gameobjectComponent)
    {
        tank.bodywork = bodywork;
        gameobjectComponent.animator.SetInteger("init", bodywork.animTrigger);
        var sprite = Resources.Load<Sprite>(bodywork.spritePath);
        gameobjectComponent.spriteRenderer.sprite = sprite;

        move.moveSpeed = bodywork.moveSpeed;
        world.lifeSystem.SetData(move.entity.lifeComponent, bodywork.hp);

        Debug.Log("EquipBodywork:" + bodywork.spritePath);
    }
}
