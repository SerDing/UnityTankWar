using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    public bool isRunning = true;
    private float _count = 0.0f;
    private float _time = 0.0f;
    // private event done;
    public Timer(float time)
    {
        _time = time;
    }

    public void Enter(){
        _count = 0;
        isRunning = true;
    }

    public void Enter(float time)
    {
        _time = time;
        _count = 0;
        isRunning = true;
    }

    public void Update()
    {
        if (isRunning == false)
        {
            return ;
        }

        _count = _count + Time.deltaTime;
        if (_count >= _time)
        {
            isRunning = false;
        }
    }
}
