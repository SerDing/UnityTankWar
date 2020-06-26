using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AISystem : SystemBase
{
    public AISystem(GameWorld world) : base(world)
    {

    }

    public void Update(Identity identity, AiComponent aicompont, InputComponent input)
    {
        if(aicompont == null || aicompont.enable == false)
        {
            return;
        }

        if(identity.isAIControl == true)
        {
            foreach (AI ai in aicompont.ais)
            {
                ai.Update();
            }
        }
    }
    
    public void AddAi(AiComponent aiComponent, AI ai)
    {
        aiComponent.ais.Add(ai);
        ai.world = this.world;
    }
}
