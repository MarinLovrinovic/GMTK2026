using System;
using UnityEngine;

public class Icon : MonoBehaviour
{
    private Transform target;
    private Camera mainCamera;

    private void Start()
    {
        target = transform.parent;
        transform.SetParent(ServiceProvider.Instance.iconCanvas.transform);
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 screenPos = mainCamera.WorldToScreenPoint(target.position);

        transform.position = screenPos;
    }
}
