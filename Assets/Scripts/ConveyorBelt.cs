using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [Header("Belt Setup")]
    public List<Transform> beltSlots;
    public List<Item> itemsOnBelt;

    [HideInInspector] public float moveDuration;

    public void RotateBelt(System.Action onFinished)
    {
        if (beltSlots.Count != itemsOnBelt.Count)
        {
            onFinished?.Invoke();
            return;
        }

        Item lastItem = itemsOnBelt[itemsOnBelt.Count - 1];
        for (int i = itemsOnBelt.Count - 1; i > 0; i--)
            itemsOnBelt[i] = itemsOnBelt[i - 1];
        itemsOnBelt[0] = lastItem;

        int totalToMove = 0;

        for (int i = 0; i < itemsOnBelt.Count; i++)
        {
            var item = itemsOnBelt[i];

            if (item != null && item.gameObject.activeInHierarchy)
            {
                totalToMove++;

                item.isMovingByConveyor = true;

                item.transform.DOMove(beltSlots[i].position, moveDuration)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                    {
                        item.isMovingByConveyor = false;
                        totalToMove--;
                        if (totalToMove <= 0)
                            onFinished?.Invoke();
                    });

                item.OnEnterNewSlot();
            }
        }

        if (totalToMove == 0)
            onFinished?.Invoke();
    }

    public bool HasFood()
    {
        foreach (var item in itemsOnBelt)
        {
            if (item != null && item.itemType == ItemType.Food)
                return true;
        }
        return false;
    }
}
