using DG.Tweening;
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
    public Sprite[] possibleSprites;
    private SpriteRenderer spriteRenderer;

    private bool isJumping = false;

    // Dùng để liên kết với băng chuyền
    [HideInInspector] public ConveyorBelt conveyor;
    [HideInInspector] public int beltIndex;

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

        if (itemType == ItemType.Food && !isJumping)
        {
            TryJumpToArrow();
        }
    }

    void TryJumpToArrow()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.1f);
        foreach (var hit in hits)
        {
            ArrowDirection arrow = hit.GetComponent<ArrowDirection>();
            if (arrow != null)
            {
                Vector3 jumpDirection = Vector3.zero;

                switch (arrow.direction)
                {
                    case ArrowDir.Up: jumpDirection = Vector3.up; break;
                    case ArrowDir.Down: jumpDirection = Vector3.down; break;
                    case ArrowDir.Left: jumpDirection = Vector3.left; break;
                    case ArrowDir.Right: jumpDirection = Vector3.right; break;
                }

                Vector3 jumpTarget = transform.position + jumpDirection * 1.5f; // Nhảy xa như cầu

                isJumping = true;
                transform.DOJump(jumpTarget, 1f, 1, 0.4f)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                    {
                        isJumping = false;

                        // Cập nhật lại vị trí food trong danh sách belt
                        float minDist = float.MaxValue;
                        int newIndex = beltIndex;

                        for (int i = 0; i < conveyor.beltSlots.Count; i++)
                        {
                            float dist = Vector3.Distance(transform.position, conveyor.beltSlots[i].position);
                            if (dist < minDist)
                            {
                                minDist = dist;
                                newIndex = i;
                            }
                        }

                        conveyor.itemsOnBelt[beltIndex] = null;
                        conveyor.itemsOnBelt[newIndex] = this;
                        beltIndex = newIndex;
                    });

                break; // Chỉ nhảy theo 1 mũi tên
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (itemType == ItemType.Food)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, 0.1f);
        }
    }

    public bool IsFood()
    {
        return itemType == ItemType.Food;
    }
}
