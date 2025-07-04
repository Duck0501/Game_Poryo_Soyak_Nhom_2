using UnityEngine;

public enum PlayerType
{
    Purple,
    Red,
    Yellow
}
public class PlayerEatFood : MonoBehaviour
{
    public ConveyorBelt conveyorBelt; // Gán qua Inspector nếu cần
    public PlayerType playerType;
    public GameObject explodePrefab; // 👈 Prefab animation được gán từ Inspector

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.GetComponent<Item>();
        if (item != null && item.itemType == ItemType.Food)
        {
            // So sánh loại
            if ((FoodType)playerType == item.foodType)
            {
                // ✅ Trì hoãn 0.1 giây trước khi ăn
                StartCoroutine(DelayedEat(item, 0.1f));
            }
            else
            {
                Debug.Log("🚫 Không ăn vì sai loại: Player " + playerType + " ≠ Food " + item.foodType);
            }
        }
    }

    private System.Collections.IEnumerator DelayedEat(Item item, float delay)
    {
        yield return new WaitForSeconds(delay);

        Vector3 foodPos = item.transform.position;
        Vector3 playerPos = transform.position;

        // ✅ Remove khỏi danh sách băng chuyền
        if (conveyorBelt != null)
        {
            int index = conveyorBelt.itemsOnBelt.IndexOf(item);
            if (index >= 0)
            {
                conveyorBelt.itemsOnBelt[index] = null;
            }
        }

        // ✅ Ẩn object gốc
        item.gameObject.SetActive(false);
        gameObject.SetActive(false); // player biến mất

        // ✅ Tạo hiệu ứng tại vị trí thức ăn
        if (explodePrefab != null)
        {
            GameObject fx1 = Instantiate(explodePrefab, foodPos, Quaternion.identity);
            GameObject fx2 = Instantiate(explodePrefab, playerPos, Quaternion.identity);

            Destroy(fx1, 1f);
            Destroy(fx2, 1f);
        }

        // ✅ Huỷ cả hai sau 1 frame
        Destroy(item.gameObject, 0.1f);
        Destroy(gameObject, 0.1f);
    }
}
