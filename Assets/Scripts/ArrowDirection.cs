using UnityEngine;
using System.Collections;

public enum ArrowDir
{
    Up,
    Down,
    Left,
    Right,
    All
}

public class ArrowDirection : MonoBehaviour
{
    public ArrowDir direction;
    public Transform redirectPoint;

    public void HandleFoodFromDirection(Item item, ArrowDir incomingDirection)
    {
        Vector3 moveDir = GetDirectionVector(incomingDirection);
        if (moveDir == Vector3.zero) return;

        ConveyorBelt nearestBelt = null;
        int nearestSlotIndex = -1;
        float minDistance = float.MaxValue;
        float tolerance = 0.1f;

        Vector3 refPos = redirectPoint != null ? redirectPoint.position : transform.position;

        foreach (var belt in FindObjectsOfType<ConveyorBelt>())
        {
            for (int i = 0; i < belt.beltSlots.Count; i++)
            {
                Transform slot = belt.beltSlots[i];
                float dist = Vector3.Distance(slot.position, refPos);

                if (dist < minDistance)
                {
                    nearestBelt = belt;
                    nearestSlotIndex = i;
                    minDistance = dist;
                }
            }
        }

        if (nearestBelt != null && nearestSlotIndex >= 0)
        {
            foreach (var belt in FindObjectsOfType<ConveyorBelt>())
            {
                int oldIndex = belt.itemsOnBelt.IndexOf(item);
                if (oldIndex >= 0)
                {
                    belt.itemsOnBelt[oldIndex] = null;
                    break;
                }
            }

            nearestBelt.itemsOnBelt[nearestSlotIndex] = item;
            item.transform.position = nearestBelt.beltSlots[nearestSlotIndex].position;
            item.isMovingByConveyor = false;
        }

        StartCoroutine(ResetRedirectFlag(item, 0.1f));
    }

    private IEnumerator ResetRedirectFlag(Item item, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (item != null)
            item.isRedirectingByArrow = false;
    }

    private Vector3 GetDirectionVector(ArrowDir dir)
    {
        return dir switch
        {
            ArrowDir.Up => Vector3.up,
            ArrowDir.Down => Vector3.down,
            ArrowDir.Left => Vector3.left,
            ArrowDir.Right => Vector3.right,
            _ => Vector3.zero
        };
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (direction == ArrowDir.All)
        {
            Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 0.3f);
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 0.3f);
            Gizmos.DrawLine(transform.position, transform.position + Vector3.left * 0.3f);
            Gizmos.DrawLine(transform.position, transform.position + Vector3.right * 0.3f);
        }
        else
        {
            Vector3 dir = GetDirectionVector(direction);
            Gizmos.DrawLine(transform.position, transform.position + dir * 0.5f);
            Gizmos.DrawSphere(transform.position + dir * 0.5f, 0.05f);
        }
    }
}
