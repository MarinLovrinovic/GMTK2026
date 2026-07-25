using System.Collections;
using TMPro;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float areaDuration;
    [SerializeField] private float transitionDuration;
    private bool transitioning = false;
    private float targetPosition = 0f;
    private float transitionFrameDelta = 0f;

    private float timer;

    private void Start()
    {
        timer = areaDuration;
    }


    private void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                // Transition done
                if (transitioning)
                {
                    transitioning = false;
                    transform.position = new Vector3(targetPosition, transform.position.y, transform.position.z);
                    CameraMapBounds.UpdateBounds();
                }
                // Transition start
                else { StartTransition(); }
            }
            
            // Mid transition
            if (transitioning)
            {
                transform.position += Vector3.right * transitionFrameDelta * Time.deltaTime;
                CameraMapBounds.UpdateBounds();
            }
        }
    }


    void StartTransition()
    {
        float viewAreaWidth = CameraMapBounds.activeArea.GetWidth();
        targetPosition = transform.position.x + (viewAreaWidth);
        transitionFrameDelta = viewAreaWidth / transitionDuration;

        transitioning = true;
        timer = transitionDuration;
    }
}
