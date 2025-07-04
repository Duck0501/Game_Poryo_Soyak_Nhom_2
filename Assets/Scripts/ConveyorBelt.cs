using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using static UnityEditor.Progress;

public class ConveyorBelt : MonoBehaviour
{
    [Header("Belt Setup")]
    public List<Transform> beltSlots;
    public List<Item> itemsOnBelt;

    [Header("Move Settings")]
    public float moveDuration = 0.3f;
    public float moveInterval = 1f;

    [Header("Turn Settings")]
    public int currentTurn = 0;
    public int maxTurns = 20;
    public TextMeshProUGUI turnText;

    [Header("UI")]
    public GameObject gameOverPanel;
    public GameObject winPanel;
    public Button playButton;
    public Image playButtonImage;
    public Sprite playSprite;
    public Sprite stopSprite;

    private bool isPlaying = false;
    private float timer = 0f;

    void Update()
    {
        if (!isPlaying) return;

        timer += Time.deltaTime;
        if (timer >= moveInterval)
        {
            timer = 0f;
            RotateBeltSmooth();
        }
    }

    public void OnPlayButtonClicked()
    {
        isPlaying = !isPlaying;

        playButtonImage.sprite = isPlaying ? stopSprite : playSprite;
    }

    void RotateBeltSmooth()
    {
        if (beltSlots.Count != itemsOnBelt.Count) return;

        currentTurn++;
        turnText.text = $"{currentTurn.ToString("D2")}";

        // Xoay mảng item
        Item lastItem = itemsOnBelt[itemsOnBelt.Count - 1];
        for (int i = itemsOnBelt.Count - 1; i > 0; i--)
        {
            itemsOnBelt[i] = itemsOnBelt[i - 1];
        }
        itemsOnBelt[0] = lastItem;

        int movingCount = 0;

        for (int i = 0; i < itemsOnBelt.Count; i++)
        {
            if (itemsOnBelt[i] != null)
            {
                movingCount++;
                int index = i;

                itemsOnBelt[i].transform.DOMove(beltSlots[i].position, moveDuration)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                    {
                        movingCount--;
                        if (movingCount <= 0)
                        {
                            // ✅ Sau khi tất cả Tween xong, kiểm tra trạng thái game
                            if (!HasAnyFood())
                            {
                                isPlaying = false;
                                winPanel.SetActive(true);
                                Debug.Log("✅ YOU WIN - không còn đồ ăn!");
                            }
                            else if (currentTurn >= maxTurns)
                            {
                                isPlaying = false;
                                gameOverPanel.SetActive(true);
                                Debug.Log("❌ YOU LOSE - hết lượt còn đồ ăn!");
                            }
                        }
                    });

                itemsOnBelt[i].OnEnterNewSlot();
            }
        }
    }

    bool HasAnyFood()
    {
        foreach (var item in itemsOnBelt)
        {
            if (item != null && item.itemType == ItemType.Food)
                return true;
        }
        return false;
    }
}
