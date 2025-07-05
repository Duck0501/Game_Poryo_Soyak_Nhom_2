using UnityEngine;

public enum ItemType
{
    Food,
    Block,
    Swap,
    ItemChanger,
    Other
}
public enum FoodType
{
    Purple,
    Red,
    Yellow
}
public class Item : MonoBehaviour
{
    public ItemType itemType = ItemType.Other;
    public FoodType foodType;

    public Sprite[] possibleSprites;
    private SpriteRenderer spriteRenderer;
    public Sprite purpleSprite;
    public Sprite redSprite;
    public Sprite yellowSprite;
    [HideInInspector] public bool isMovingByConveyor = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnEnterNewSlot()
    {
        if (itemType == ItemType.Swap && possibleSprites != null && possibleSprites.Length > 0)
        {
            int randomIndex = Random.Range(0, possibleSprites.Length);
            spriteRenderer.sprite = possibleSprites[randomIndex];
        }
    }
    public bool IsFood()
    {
        return itemType == ItemType.Food;
    }
    public void UpdateColorByFoodType()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        switch (foodType)
        {
            case FoodType.Purple:
                spriteRenderer.sprite = purpleSprite;
                break;
            case FoodType.Red:
                spriteRenderer.sprite = redSprite;
                break;
            case FoodType.Yellow:
                spriteRenderer.sprite = yellowSprite;
                break;
        }
    }
}
