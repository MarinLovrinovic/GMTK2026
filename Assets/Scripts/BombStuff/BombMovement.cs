using UnityEngine;
using UnityEngine.InputSystem;

public class BombMovement : MonoBehaviour
{
    [SerializeField] private LayerMask movable;

    private Bomb pointingTo;
    private Bomb carrying;
    private Plane dragPlane;
    private float bombY;

    private void Start()
    {
        carrying = null;
        dragPlane = new Plane();
        bombY = 0.0f;
    }

    private void Update()
    {
        if (Mouse.current == null || Camera.main == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (carrying)
        {
            if (dragPlane.Raycast(ray, out float distance))
            {
                Vector3 mouseWorldPos = ray.GetPoint(distance);
                mouseWorldPos.y = bombY;
                carrying.transform.position = mouseWorldPos;
            }

            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                ServiceProvider.Instance.soundManager.playSound(0);
                carrying.Release();
                carrying = null;
            }
        }
        else // not carrying
        {
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, movable))
            {
                Bomb bomb = hit.collider.GetComponent<Bomb>();
                if (bomb != null)
                {
                    pointingTo = bomb;
                    pointingTo.SetRadiusDisplay(true);
                }

                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    if (bomb != null && !bomb.Moved)
                    {
                        ServiceProvider.Instance.soundManager.playSound(1);
                        bomb.Move();
                        carrying = bomb;
                        bombY = bomb.transform.position.y;
                        dragPlane = new Plane(Vector3.up, bombY);
                    }
                }
            }
            else if (pointingTo)
            {
                pointingTo.SetRadiusDisplay(false);
                pointingTo = null;    
            }
        }
    }
}
