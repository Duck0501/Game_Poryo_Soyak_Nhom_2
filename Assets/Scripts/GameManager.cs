using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject canvasHome;
    public GameObject canvasHelp;
    public GameObject canvasLevel;
    public GameObject[] levelPrefabs;
    public GameObject currentLevel;

    public Transform levelParent;

    public Button buttonHelp;
    public Button buttonPlay;
    public Button buttonHomeInHelp;
    public Button buttonHomeInLevel;

    public Button[] levelButtons;

    private int currentLevelIndex = -1;

    void Start()
    {
        ShowCanvas(canvasHome);

        buttonHelp.onClick.AddListener(() => ShowCanvas(canvasHelp));
        buttonPlay.onClick.AddListener(() => ShowCanvas(canvasLevel));
        buttonHomeInHelp.onClick.AddListener(() => ShowCanvas(canvasHome));
        buttonHomeInLevel.onClick.AddListener(() => ShowCanvas(canvasHome));

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelIndex = i + 1;
            levelButtons[i].onClick.AddListener(() => LevelSelected(levelIndex));
        }
    }

    void Update()
    {
        bool isAnyCanvasActive = canvasHome.activeSelf
                           || canvasHelp.activeSelf
                           || canvasLevel.activeSelf;

        if (Input.GetKeyDown(KeyCode.R) && !isAnyCanvasActive && currentLevelIndex >= 0 && currentLevelIndex < levelPrefabs.Length)
        {
            ResetCurrentLevel();
        }
    }

    public void ShowCanvas(GameObject canvasToShow)
    {
        canvasHome.SetActive(canvasToShow == canvasHome);
        canvasHelp.SetActive(canvasToShow == canvasHelp);
        canvasLevel.SetActive(canvasToShow == canvasLevel);
    }

    public void LevelSelected(int level)
    {
        StartCoroutine(LoadLevelRoutine(level));
    }

    private System.Collections.IEnumerator LoadLevelRoutine(int level)
    {
        currentLevelIndex = level - 1;

        canvasHome.SetActive(false);
        canvasHelp.SetActive(false);
        canvasLevel.SetActive(false);

        if (currentLevel != null) Destroy(currentLevel);


        if (level > 0 && level <= levelPrefabs.Length)
        {
            currentLevel = Instantiate(levelPrefabs[level - 1], levelParent);

            Button[] buttons = currentLevel.GetComponentsInChildren<Button>();
            foreach (Button btn in buttons)
            {
                if (btn.name.Contains("home"))
                    btn.onClick.AddListener(() => ShowCanvas(canvasHome));
                if (btn.name.Contains("replay"))
                    btn.onClick.AddListener(ResetCurrentLevel);
                if (btn.name.Contains("Button"))
                    btn.onClick.AddListener(() => ShowCanvas(canvasLevel));
            }
        }

        yield return new WaitForSeconds(1f);
    }

    public void ResetCurrentLevel()
    {
        if (currentLevelIndex >= 0 && currentLevelIndex < levelPrefabs.Length)
        {
            LevelSelected(currentLevelIndex + 1);
        }
    }

    public void LoadNextLevelManual()
    {
        if (currentLevelIndex + 1 < levelPrefabs.Length)
        {
            LevelSelected(currentLevelIndex + 2);
        }
        else
        {
            ShowCanvas(canvasHome);
        }
    }
}