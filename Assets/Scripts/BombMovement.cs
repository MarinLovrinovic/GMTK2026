using System;
using UnityEngine;

public class BombMovement : MonoBehaviour
{
    [SerializeField] private LayerMask movable;
    private Bomb carrying;
    private void Update()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (carrying)
        {
            carrying.transform.position = mouseWorldPos;
            if (Input.GetMouseButtonUp(0))
            {
                carrying = null;
            }
        }
        else // not carrying
        {
            if (Input.GetMouseButtonDown(0))
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
