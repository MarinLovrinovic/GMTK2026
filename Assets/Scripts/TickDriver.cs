using System;
using UnityEngine;

public class TickDriver : MonoBehaviour
{
    public static TickDriver instance;

    public event Action OnTick;

    private float timer;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        timer = 0.0f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1.0f)
        {
            OnTick?.Invoke();
            timer -= 1.0f;
        }
    }
}
