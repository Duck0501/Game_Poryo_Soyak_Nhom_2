using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ConveyorBelt : MonoBehaviour
{
    public List<Transform> beltSlots;      // Danh sách các vị trí theo vòng
    public List<Item> itemsOnBelt;       // Item hiện có tại từng vị trí

    public float moveInterval = 1f;        // Thời gian giữa mỗi lần xoay
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= moveInterval)
        {
            RotateBelt();
            timer = 0f;
        }
    }

    void RotateBelt()
    {
        if (beltSlots.Count != itemsOnBelt.Count) return;

        // Lưu item cuối để dịch lên đầu
        Item lastItem = itemsOnBelt[itemsOnBelt.Count - 1];

        // Dịch item về trước 1 vị trí
        for (int i = itemsOnBelt.Count - 1; i > 0; i--)
        {
            itemsOnBelt[i] = itemsOnBelt[i - 1];
        }

        itemsOnBelt[0] = lastItem;

        // Cập nhật vị trí item trên scene
        for (int i = 0; i < itemsOnBelt.Count; i++)
        {
            if (itemsOnBelt[i] != null)
            {
                itemsOnBelt[i].transform.position = beltSlots[i].position;
                itemsOnBelt[i].OnEnterNewSlot(); // Gọi hàm phản ứng
            }
        }
    }
}
