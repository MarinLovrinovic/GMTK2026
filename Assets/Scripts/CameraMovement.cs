using System.Collections;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float areaDuration;
    [SerializeField] private float transitionDuration;
    private bool transitioning = false;
    private float transitionDelta = 0f;

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

                }
                // Transition start
                else { StartTransition(); }
            }
            
            // Mid transition
            if (transitioning)
            {
                transform.position += Vector3.right * transitionDelta;
            }
        }
    }


    void StartTransition()
    {
        float viewAreaWidth = CameraMapBounds.activeArea.GetWidth();
        transitionDelta = viewAreaWidth / transitionDuration;

        transitioning = true;
        timer = transitionDuration;
    }
}
