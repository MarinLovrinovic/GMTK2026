using System;
using UnityEngine;

public class ServiceProvider : MonoBehaviour
{
    public static ServiceProvider Instance;
    public Canvas iconCanvas;

    private void Awake()
    {
        Instance = this;
    }
}
