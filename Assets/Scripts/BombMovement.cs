using UnityEngine;
using UnityEngine.InputSystem;

public class BombMovement : MonoBehaviour
{
    [SerializeField] private LayerMask movable;
    private Bomb carrying;
    private void Update()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        if (carrying)
        {
            carrying.transform.position = mouseWorldPos;
            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                carrying = null;
            }
        }
        else // not carrying
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Collider2D[] bombs = Physics2D.OverlapPointAll(mouseWorldPos, movable);
                foreach (Collider2D bombCollider in bombs)
                {
                    Bomb bomb = bombCollider.GetComponent<Bomb>();
                    if (bomb.Moved) continue;
                    bomb.Move();
                    carrying = bomb;
                    break;
                }
            }    
        }
    }
}
