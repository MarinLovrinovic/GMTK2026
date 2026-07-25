using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour
{
    public static float Scale = 1;
    [SerializeField] private float scale = 1;
    private float initialScale;

    private void OnEnable()
    {
        initialScale = scale;
        Scale = initialScale;
    }
    private void OnDisable()
    {
        Scale = initialScale;
    }

    void Update()
    {
        Scale = scale;
        // Camera.main.orthographicSize = 4.5f * scale;
    }
}
