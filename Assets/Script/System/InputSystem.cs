using UnityEngine;
using System.Collections.Generic;
public class InputSystem : SystemBase
{
    public InputSystem(GameWorld world):base(world)
    {

    }

    public void Update(InputComponent input)
    {
        if (input == null || input.enable == false)
        {
            return;
        }
        if (input.isAIControl == false)
        {
            foreach (var map in InputComponent.inputMap)
            {

                if (!map.Key.Contains("mouse") && Input.GetKeyDown(map.Key))
                {
                    input.tempList.Add(map.Value);
                    
                }
            }
            ProcessInputStates(input, InputComponent.STATE.PRESSED);

            if (InputComponent.inputMap.ContainsKey("mouseLeft") && Input.GetMouseButtonDown(0))
            {
                input.inputStates[InputComponent.inputMap["mouseLeft"]] = InputComponent.STATE.PRESSED;
            }
            if (InputComponent.inputMap.ContainsKey("mouseRight") && Input.GetMouseButtonDown(1))
            {
                input.inputStates[InputComponent.inputMap["mouseRight"]] = InputComponent.STATE.PRESSED;
            }
            if (InputComponent.inputMap.ContainsKey("mouseMiddle") && Input.GetMouseButtonDown(2))
            {
                input.inputStates[InputComponent.inputMap["mouseMiddle"]] = InputComponent.STATE.PRESSED;
            }

            foreach (var map in InputComponent.inputMap)
            {
                if (!map.Key.Contains("mouse") && Input.GetKeyUp(map.Key))
                {
                    input.tempList.Add(map.Value);
                }
            }
            ProcessInputStates(input, InputComponent.STATE.RELEASED);

            if (InputComponent.inputMap.ContainsKey("mouseLeft") && Input.GetMouseButtonUp(0))
            {
                input.inputStates[InputComponent.inputMap["mouseLeft"]] = InputComponent.STATE.RELEASED;
            }
            if (InputComponent.inputMap.ContainsKey("mouseRight") && Input.GetMouseButtonUp(1))
            {
                input.inputStates[InputComponent.inputMap["mouseRight"]] = InputComponent.STATE.RELEASED;
            }
            if (InputComponent.inputMap.ContainsKey("mouseMiddle") && Input.GetMouseButtonUp(2))
            {
                input.inputStates[InputComponent.inputMap["mouseMiddle"]] = InputComponent.STATE.RELEASED;
            }
        }

        foreach (var state in input.inputStates) // set pressed keys to STATE.HOLD
        {
            if (input.inputStates[state.Key] == InputComponent.STATE.PRESSED)
            {
                input.tempList.Add(state.Key);
            }
        }
        ProcessInputStates(input, InputComponent.STATE.HOLD);


        foreach (var state in input.inputStates) // set released keys to STATE.CLEAER
        {
            if (input.inputStates[state.Key] == InputComponent.STATE.RELEASED)
            {
                input.tempList.Add(state.Key);
            }
        }
        ProcessInputStates(input, InputComponent.STATE.CLEAER);

        ReleaseAll(input);
    }

    private static void ProcessInputStates(InputComponent input, InputComponent.STATE state)
    {
        for (int i = 0; i < input.tempList.Count; i++)
        {
            input.inputStates[input.tempList[i]] = state;
        }
        input.tempList.Clear();
    }

    public static void PressAction(InputComponent input, string action)
    {
        if (input.inputStates.ContainsKey(action))
        {
            input.inputStates[action] = InputComponent.STATE.HOLD;
        }
        else
        {
            input.inputStates[action] = InputComponent.STATE.PRESSED;
        }

    }

    public static void ReleaseAction(InputComponent input, string action)
    {
        if (input.inputStates.ContainsKey(action))
        {
            input.inputStates[action] = InputComponent.STATE.RELEASED;
        }
    }

    public static bool GetPressAction(InputComponent input, string action)
    {
        if (input.inputStates.ContainsKey(action))
        {
            return input.inputStates[action] == InputComponent.STATE.PRESSED;
        }

        return false;
    }

    public static bool GetHoldAction(InputComponent input, string action)
    {
        if (input.inputStates.ContainsKey(action))
        {
            return input.inputStates[action] == InputComponent.STATE.HOLD;
        }

        return false;
    }

    public static bool GetReleaseAction(InputComponent input, string action)
    {
        if (input.inputStates.ContainsKey(action))
        {
            return input.inputStates[action] == InputComponent.STATE.RELEASED;
        }

        return false;
    }

    public void ReleaseAll(InputComponent input)
    {
        if (input.isAIControl == true)
        {
            foreach (var state in input.inputStates) // set pressed keys to STATE.HOLD
            {
                if (input.inputStates[state.Key] == InputComponent.STATE.HOLD)
                {
                    input.tempList.Add(state.Key);
                }
            }
        }
        ProcessInputStates(input, InputComponent.STATE.RELEASED);
    }
}
