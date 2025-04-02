using UnityEngine;

public class Timer
{
    float time = 0;
    float duration = 0;
    bool completed = false;

    public Timer(float duration) { this.duration = duration; time = duration; }

    public void Update()
    {
        if (time <= 0) { completed = true; time = 0; }
        else { time -= Time.deltaTime; }
    }
    public void Reset()
    {
        time = duration;
        completed = false;
    }

    public float GetTime() { return time; }
    public bool Completed() { return completed; }
}