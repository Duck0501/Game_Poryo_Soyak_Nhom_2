using UnityEngine;

public enum PlayerType
{
    Purple,
    Red,
    Yellow
}
public class PlayerEatFood : MonoBehaviour
{
    public ConveyorBelt conveyorBelt;
    public PlayerType playerType;
    public GameObject explodePrefab;
    private Item pendingItemToEat;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (pendingItemToEat != null) return;

        Item item = collision.GetComponent<Item>();
        if (item != null && item.itemType == ItemType.Food)
        {
            if ((FoodType)playerType == item.foodType)
            {
                pendingItemToEat = item;
            }
        }
    }

    public void TryEatAfterBelt()
    {
        if (pendingItemToEat == null) return;

        StartCoroutine(DelayedEat(pendingItemToEat, 0.1f));
        pendingItemToEat = null;
    }

    private System.Collections.IEnumerator DelayedEat(Item item, float delay)
    {
        yield return new WaitForSeconds(delay);

        Vector3 foodPos = item.transform.position;
        Vector3 playerPos = transform.position;

        if (conveyorBelt != null)
        {
            int index = conveyorBelt.itemsOnBelt.IndexOf(item);
            if (index >= 0)
            {
                conveyorBelt.itemsOnBelt[index] = null;
            }
        }

        item.gameObject.SetActive(false);
        gameObject.SetActive(false);

        if (explodePrefab != null)
        {
            GameObject fx1 = Instantiate(explodePrefab, foodPos, Quaternion.identity);
            GameObject fx2 = Instantiate(explodePrefab, playerPos, Quaternion.identity);
            Destroy(fx1, 1f);
            Destroy(fx2, 1f);
        }

        ConveyorManager.Instance.QueueDestroy(item.gameObject, 0.1f);
        ConveyorManager.Instance.QueueDestroy(gameObject, 0.1f);
    }
}
