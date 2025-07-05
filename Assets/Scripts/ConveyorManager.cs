using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ConveyorManager : MonoBehaviour
{
    [Header("All Belts")]
    public List<ConveyorBelt> allBelts;

    [Header("UI")]
    public TextMeshProUGUI turnText;
    public GameObject winPanel;
    public GameObject losePanel;
    public Button playButton;
    public Image playButtonImage;
    public Sprite playSprite;
    public Sprite stopSprite;

    [Header("Global Settings")]
    private float moveDuration = 0.7f;
    public float moveInterval = 1f;
    public int maxTurns = 20;

    public int currentTurn = 0;
    private bool isPlaying = false;
    private bool isGameOver = false;

    private int beltFinishedThisTurn = 0;
    private bool isRunningTurn = false;
    private int beltDoneCount = 0;
    void Start()
    {
        foreach (var belt in allBelts)
        {
            belt.moveDuration = moveDuration;
           
        }

        UpdateTurnText();
    }

    void Update()
    {
        if (!isPlaying || isGameOver || isRunningTurn) return;

        // Bắt đầu 1 lượt mới
        isRunningTurn = true;
        beltDoneCount = 0;

        foreach (var belt in allBelts)
        {
            belt.RotateBelt(() =>
            {
                beltDoneCount++;

                if (beltDoneCount >= allBelts.Count)
                {
                    currentTurn++;
                    UpdateTurnText();

                    if (!AnyBeltHasFood())
                        GameWin();
                    else if (currentTurn >= maxTurns)
                        GameLose();
                    else
                        isRunningTurn = false; // ✅ Cho phép bắt đầu lượt mới
                }
            });
        }
    }

    public void OnPlayButtonClicked()
    {
        if (isGameOver) return;

        isPlaying = !isPlaying;
        playButtonImage.sprite = isPlaying ? stopSprite : playSprite;
    }

    void OnBeltTurnFinished(ConveyorBelt belt)
    {
        beltFinishedThisTurn++;

        // ✅ Chờ tất cả belt hoàn tất
        if (beltFinishedThisTurn >= allBelts.Count)
        {
            currentTurn++;
            UpdateTurnText();
            beltFinishedThisTurn = 0;

            if (!AnyBeltHasFood())
            {
                GameWin();
            }
            else if (currentTurn >= maxTurns)
            {
                GameLose();
            }
        }
    }

    void UpdateTurnText()
    {
        turnText.text = $"{currentTurn.ToString("D2")}";
    }

    bool AnyBeltHasFood()
    {
        foreach (var belt in allBelts)
            if (belt.HasFood()) return true;
        return false;
    }

    void GameWin()
    {
        isGameOver = true;
        isPlaying = false;
        winPanel.SetActive(true);
        Debug.Log("✅ YOU WIN!");
    }

    void GameLose()
    {
        isGameOver = true;
        isPlaying = false;
        losePanel.SetActive(true);
        Debug.Log("❌ YOU LOSE!");
    }
}
