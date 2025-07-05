using UnityEngine;

public enum ItemType
{
    Food,   // Đồ ăn – cần phải xử lý khi win/lose
    Block,  // Không thể thay đổi
    Swap,   // Có thể đổi chỗ hoặc màu
    Other   // Mặc định
}

public class Item : MonoBehaviour
{
    public ItemType itemType = ItemType.Other;

    // Nếu là Swap item → có thể đổi màu khi tới ô mới
    public Sprite[] possibleSprites;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Hàm này được gọi mỗi khi item được đẩy tới 1 ô mới
    public void OnEnterNewSlot()
    {
        if (itemType == ItemType.Swap && possibleSprites != null && possibleSprites.Length > 0)
        {
            // Đổi sprite ngẫu nhiên mỗi khi đến ô mới (chỉ với loại Swap)
            int randomIndex = Random.Range(0, possibleSprites.Length);
            spriteRenderer.sprite = possibleSprites[randomIndex];
        }
    }

    // Hàm gọi thủ công để kiểm tra nếu cần
    public bool IsFood()
    {
        return itemType == ItemType.Food;
    }
}
