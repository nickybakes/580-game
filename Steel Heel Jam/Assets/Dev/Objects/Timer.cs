using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{

    private float originalMaxTime;

    private float maxTimeRandomizeRangeDefault;

    public float maxTime;

    public float currentTime;

    public bool activated;

    public Timer(float _maxTime, float _maxTimeRandomizeRangeDefault = 0, bool _activated = true)
    {
        maxTime = _maxTime;
        originalMaxTime = _maxTime;
        maxTimeRandomizeRangeDefault = _maxTimeRandomizeRangeDefault;
        activated = _activated;
    }

    // Update is called once per frame
    public void Update()
    {
        if (activated)
            currentTime += Time.deltaTime;
    }

    public bool Done()
    {
        return currentTime >= maxTime;
    }

    /// <summary>
    /// updates the timer when called, and automatically restarts the timer if its done
    /// </summary>
    /// <param name="maxTimeRandomizeRange">takes the original max time and alters it for the next cycle 
    /// by a random amount: maxTime + Random(-range, range)</param>
    /// <returns>returns true if the timer is done, false if not</returns>
    public bool DoneLoop(float maxTimeRandomizeRange = -1)
    {
        Update();
        if (currentTime >= maxTime)
        {
            Restart(maxTimeRandomizeRange);
            return true;
        }
        return false;
    }

    public float Percentage()
    {
        return currentTime / maxTime;
    }

    public void Restart(float maxTimeRandomizeRange = -1, float setMaxTime = -1)
    {
        currentTime = 0;
        if (maxTimeRandomizeRange == -1)
            maxTimeRandomizeRange = maxTimeRandomizeRangeDefault;
        if (setMaxTime == -1)
            setMaxTime = originalMaxTime;
        maxTime = setMaxTime + Random.Range(-maxTimeRandomizeRange, maxTimeRandomizeRange);
    }
}
