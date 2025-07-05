using UnityEngine;

public class ItemChanger : MonoBehaviour
{
    public FoodType targetColor;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.GetComponent<Item>();
        if (item != null && item.itemType == ItemType.Food && item.isMovingByConveyor)
        {
            item.foodType = targetColor;
            item.UpdateColorByFoodType();
        }
    }
}
