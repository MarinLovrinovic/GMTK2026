using UnityEngine;
using UnityEngine.InputSystem;

public class BombMovement : MonoBehaviour
{
    [SerializeField] private LayerMask movable;

    public GameObject radiusPrefab;
    [SerializeField] private bool radiusIs2D;

    private Bomb carrying;
    private Plane dragPlane;
    private float bombY;
    private GameObject displayedRadius;
    private float radiusRadius;

    private void Start()
    {
        carrying = null;
        dragPlane = new Plane();
        bombY = 0.0f;
        displayedRadius = null;
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
                carrying.Release();
                carrying = null;
            }
        }
        else // not carrying
        {
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, movable))
            {
                Bomb bomb = hit.collider.GetComponent<Bomb>();
                if (bomb != null && displayedRadius != null)
                {
                    radiusRadius = bomb.getExplosionRadius();
                    if (radiusIs2D)
                    {
                        displayedRadius.transform.GetChild(0).GetComponent<SpriteRenderer>().size = new Vector2(radiusRadius, radiusRadius) * 2;
                    }
                    else
                    {
                        displayedRadius.transform.localScale = new Vector3(radiusRadius, radiusRadius, radiusRadius);
                    }
                }

                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    if (bomb != null && !bomb.Moved)
                    {
                        bomb.Move();
                        carrying = bomb;
                        bombY = bomb.transform.position.y;
                        dragPlane = new Plane(Vector3.up, bombY);

                        Destroy(displayedRadius);
                        displayedRadius = null;
                    }
                }
                else
                {
                    if (bomb != null && displayedRadius == null)
                    {
                        displayedRadius = Instantiate(radiusPrefab, bomb.transform.position, Quaternion.identity);
                        radiusRadius = bomb.getExplosionRadius();
                        if (radiusIs2D)
                        {
                            displayedRadius.transform.GetChild(0).GetComponent<SpriteRenderer>().size = new Vector2(radiusRadius, radiusRadius) * 2;
                        }
                        else
                        {
                            displayedRadius.transform.localScale = new Vector3(radiusRadius, radiusRadius, radiusRadius);
                        }
                    }
                }
            }
            else
            {
                Destroy(displayedRadius);
                displayedRadius = null;
            }
        }
    }
}
