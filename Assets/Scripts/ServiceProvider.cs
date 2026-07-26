using System;
using UnityEngine;

public class ServiceProvider : MonoBehaviour
{
    public static ServiceProvider Instance;
    public Canvas iconCanvas;
    public GameManager gameManager;
    public SoundManager soundManager;

    private void Awake()
    {
        Instance = this;
    }
}
