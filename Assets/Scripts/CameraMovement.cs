using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float areaDuration;
    [SerializeField] private float transitionDuration;
    public bool transitioning { get; private set; }  = false;
    private float targetPosition = 0f;
    private float transitionFrameDelta = 0f;

    private float timer;

    [SerializeField] private Transform sea;

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
                    Move(targetPosition);
                }
                // Transition start
                else { StartTransition(); }
            }
            
            // Mid transition
            if (transitioning)
            {
                Move(transform.position.x + (transitionFrameDelta * Time.deltaTime));
            }
        }
    }


    void StartTransition()
    {
        float viewAreaWidth = CameraMapBounds.activeArea.GetWidth();
        targetPosition = transform.position.x + viewAreaWidth;
        transitionFrameDelta = viewAreaWidth / transitionDuration;
        transitioning = true;
        timer = transitionDuration;

        // Fill new area with obstacles
        PlaneTrapezoid newArea = CameraMapBounds.activeArea.Translate(new Vector2(viewAreaWidth, 0f));
        FindAnyObjectByType<ObstacleSpawner>().FillArea(newArea);

        // Add new bomb into the mix
        BombSpawner bombSpawner = FindAnyObjectByType<BombSpawner>();
        if (bombSpawner.addNewOnNextArea) { bombSpawner.AddNewBomb(); }
    }


    void Move(float position)
    {
        transform.position = new Vector3(position, transform.position.y, transform.position.z);
        sea.position = new Vector3(position, sea.position.y, sea.position.z);
        CameraMapBounds.UpdateBounds();
    }
}
