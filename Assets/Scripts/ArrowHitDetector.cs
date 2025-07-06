using UnityEngine;

public class ArrowHitDetector : MonoBehaviour
{
    public ArrowDir detectDirection;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.GetComponent<Item>();
        if (item == null || item.itemType != ItemType.Food) return;

        if (!item.isMovingByConveyor) return;

        Arrow_Control arrowControl = GetComponentInParent<Arrow_Control>();
        if (arrowControl != null && arrowControl.IsDraggingPublic)
            return;

        if (item.isRedirectingByArrow) return;

        item.isRedirectingByArrow = true;

        ArrowDirection arrow = GetComponentInParent<ArrowDirection>();
        if (arrow != null)
        {
            arrow.HandleFoodFromDirection(item, detectDirection);
        }
    }
}
