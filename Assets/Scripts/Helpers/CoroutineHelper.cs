using System;
using System.Collections;
using UnityEngine;

public static class CoroutineHelper
{
    public static IEnumerator ExecuteTheNextFrame(Action task)
    {
        yield return null;
        task?.Invoke();
    }

    public static IEnumerator ExecuteAfterFrames(int frames, Action task)
    {
        for (int i = 0; i < frames; i++)
        {
            yield return null;
        }
        task?.Invoke();
    }
    
    public static IEnumerator ExecuteAfterTime(float delay, Action task)
    {
        yield return new WaitForSeconds(delay);
        task?.Invoke();
    }
}
