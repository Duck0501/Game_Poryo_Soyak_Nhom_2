using System.Collections;
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

    public static ConveyorManager Instance { get; private set; }
    private List<(GameObject go, float delay)> destroyQueue = new List<(GameObject, float)>();
    void Start()
    {
        Instance = this;
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
                    StartCoroutine(ProcessAfterTurn());
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
        StartCoroutine(ShowLevelCanvasAfterDelay());
    }

    void GameLose()
    {
        isGameOver = true;
        isPlaying = false;
        losePanel.SetActive(true);
    }
    public void QueueDestroy(GameObject go, float delay)
    {
        destroyQueue.Add((go, delay));
    }
    private System.Collections.IEnumerator ShowLevelCanvasAfterDelay()
    {
        yield return new WaitForSeconds(2f);

        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            gm.ShowCanvas(gm.canvasLevel);
        }
    }
    private IEnumerator ProcessAfterTurn()
    {
        foreach (var player in FindObjectsOfType<PlayerEatFood>())
            player.TryEatAfterBelt();

        yield return new WaitForSeconds(0.2f);

        foreach (var (go, delay) in destroyQueue)
            Destroy(go, delay);
        destroyQueue.Clear();

        if (isGameOver) yield break;

        if (!AnyBeltHasFood())
        {
            GameWin();
        }
        else
        {
            currentTurn++;
            UpdateTurnText();

            if (currentTurn >= maxTurns)
                GameLose();
            else
                isRunningTurn = false;
        }
    }
}
