using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // Dòng này rất quan trọng

public class ConveyorBelt : MonoBehaviour
{
    public List<Transform> beltSlots;      // Vị trí các ô trên băng chuyền
    public List<Item> itemsOnBelt;       // Danh sách item tương ứng mỗi slot

    public float moveInterval = 1f;        // Thời gian giữa các bước xoay
    public float moveDuration = 0.3f;      // Thời gian để tween item

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= moveInterval)
        {
            RotateBeltSmooth();
            timer = 0f;
        }
    }

    void RotateBeltSmooth()
    {
        if (beltSlots.Count != itemsOnBelt.Count) return;

        // Dịch item như danh sách vòng tròn
        Item lastItem = itemsOnBelt[itemsOnBelt.Count - 1];
        for (int i = itemsOnBelt.Count - 1; i > 0; i--)
        {
            itemsOnBelt[i] = itemsOnBelt[i - 1];
        }
        itemsOnBelt[0] = lastItem;

        // Tween di chuyển từng item đến vị trí slot tương ứng
        for (int i = 0; i < itemsOnBelt.Count; i++)
        {
            if (itemsOnBelt[i] != null)
            {
                itemsOnBelt[i].transform.DOMove(beltSlots[i].position, moveDuration).SetEase(Ease.OutQuad);
                itemsOnBelt[i].OnEnterNewSlot(); // Gọi hiệu ứng nếu là item đặc biệt
            }
        }
    }
}
