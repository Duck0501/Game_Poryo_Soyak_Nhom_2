using UnityEngine;

public class Portal : MonoBehaviour
{
    private static Portal[] allPortals;

    private void Awake()
    {
        allPortals = FindObjectsOfType<Portal>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.GetComponent<Item>();

        if (item != null && item.itemType == ItemType.Food && item.isMovingByConveyor)
        {
            if (allPortals.Length == 2)
            {
                Portal other = (allPortals[0] == this) ? allPortals[1] : allPortals[0];
                if (other == null || other == this) return;

                ConveyorBelt belt = FindObjectOfType<ConveyorBelt>();
                if (belt != null)
                {
                    int oldIndex = belt.itemsOnBelt.IndexOf(item);
                    int newIndex = GetNearestSlotIndex(belt, other.transform.position);

                    if (oldIndex >= 0 && newIndex >= 0)
                    {
                        belt.itemsOnBelt[oldIndex] = null;
                        belt.itemsOnBelt[newIndex] = item;

                        item.transform.position = belt.beltSlots[newIndex].position;

                        item.isMovingByConveyor = false;
                    }
                }
            }
        }
    }

    int GetNearestSlotIndex(ConveyorBelt belt, Vector3 position)
    {
        float minDist = float.MaxValue;
        int nearestIndex = -1;

        for (int i = 0; i < belt.beltSlots.Count; i++)
        {
            float dist = Vector2.Distance(belt.beltSlots[i].position, position);
            if (dist < minDist)
            {
                minDist = dist;
                nearestIndex = i;
            }
        }

        return nearestIndex;
    }
}
