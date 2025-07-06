using UnityEngine;
using static UnityEngine.LightAnchor;

public enum ArrowDir
{
    Up,
    Down,
    Left,
    Right
}
public class ArrowDirection : MonoBehaviour
{
    public ArrowDir direction;

    private void OnDrawGizmos()
    {
        Vector3 jumpTarget = transform.position;
        Vector3 jumpDirection = Vector3.zero;
        Vector3 dir = Vector3.zero;
        switch (direction)
        {
            case ArrowDir.Up: dir = Vector3.up; break;
            case ArrowDir.Down: dir = Vector3.down; break;
            case ArrowDir.Left: dir = Vector3.left; break;
            case ArrowDir.Right: dir = Vector3.right; break;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + dir * 0.5f);
        Gizmos.DrawSphere(transform.position + dir * 0.5f, 0.05f);

        jumpTarget += jumpDirection * 1.5f;
    }
}
