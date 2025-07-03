using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType { Normal, Swap }

    public ItemType itemType;

    public void OnEnterNewSlot()
    {
        if (itemType == ItemType.Swap)
        {
            // Ví dụ: đổi màu
            GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        }
    }
}
