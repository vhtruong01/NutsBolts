using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

class GameUI : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private GameData gameData;

    [SerializeField] private TextMeshProUGUI levelTxt;
    [SerializeField] private TextMeshProUGUI timeTxt;
    [SerializeField] private Button pauseBtn;
    [SerializeField] private Button replayBtn;
    [SerializeField] private Button screwdriveBtn;
    [SerializeField] private Button sawBtn;
    [SerializeField] private Button clockBtn;
    [SerializeField] private Image screwIconPrefab;
    [SerializeField] private GameObject iconGroup;
    [SerializeField] private Image background;

    [SerializeField] private ReplayMenu replayMenu;
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private AddItemMenu addItemMenu;
    [SerializeField] private ToolMenu toolMenu;
    [SerializeField] private ExtraHoleMenu extraHoleMenu;
    [SerializeField] private LoseMenu loseMenu;
    [SerializeField] private WinMenu winMenu;
    [SerializeField] private WarningUI warningUI;
    [SerializeField] private GameObject openingUI;

    public GamePlay gamePlay { private get; set; }

    public void Awake() => SetMenuData();
    public void Start()
    {
        SetupButton();
        SetupMenu();
    }
    private void SetMenuData()
    {
        replayMenu.SetData(playerData, gameData);
        pauseMenu.SetData(playerData, gameData);
        addItemMenu.SetData(playerData, gameData);
        extraHoleMenu.SetData(playerData, gameData);
        loseMenu.SetData(playerData, gameData);
        winMenu.SetData(playerData, gameData);
    }
    private void SetupMenu()
    {
        loseMenu.SetOnContinueListener(gamePlay.Continue);
        replayMenu.SetOnReplayListener(gamePlay.Replay);
        pauseMenu.SetOnRestartListener(gamePlay.Restart);
        loseMenu.SetOnRestartListener(gamePlay.Restart);
        winMenu.SetOnRestartListener(gamePlay.Restart);
        pauseMenu.SetOnHomeListener(Home);
        loseMenu.SetOnHomeListener(Home);
        winMenu.SetOnHomeListener(Home);
        pauseMenu.DisableAction = gamePlay.Resume;
        replayMenu.DisableAction = gamePlay.Resume;
        addItemMenu.DisableAction = gamePlay.Resume;
        extraHoleMenu.DisableAction = gamePlay.Resume;
        toolMenu.DisableAction = gamePlay.Resume;
    }
    private void SetupButton()
    {
        replayBtn.onClick.AddListener(replayMenu.Show);
        replayBtn.onClick.AddListener(gamePlay.Pause);
        pauseBtn.onClick.AddListener(pauseMenu.Show);
        pauseBtn.onClick.AddListener(gamePlay.Pause);
        sawBtn.onClick.AddListener(DisplaySawScreen);
        screwdriveBtn.onClick.AddListener(DisplayScrewdriveScreen);
        clockBtn.onClick.AddListener(gamePlay.UseClock);
        RefreshToolIcon();
    }
    public void DisplayStage()
    {
        LevelData levelData = gameData.GetCurLevel();
        levelTxt.text = "Level: " + gameData.selectedLv;
        for (int i = 0; i < iconGroup.transform.childCount; i++)
            Destroy(iconGroup.transform.GetChild(i).gameObject);
        for (int i = 0; i < levelData.stages.Count; i++)
        {
            var icon = Instantiate(screwIconPrefab);
            icon.gameObject.name = "ScrewIcon";
            if (levelData.stages[i].isHard)
                icon.color = new Color(0.5f, 0.5f, 0.5f);
            icon.transform.SetParent(iconGroup.transform, false);
            icon.transform.localScale = Vector3.one * (i == gameData.stageIndex ? 1f : 0.75f);
            if (i < gameData.stageIndex)
            {
                Color c = icon.color;
                c.a = 0.75f;
                icon.color = c;
            }
        }
    }
    public void DisplayTime(float time)
    {
        int m = (int)time / 60, s = (int)time % 60;
        timeTxt.text = (m < 10 ? "0" : "") + m + ":" + (s < 10 ? "0" : "") + s;
    }
    private void Home()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
    private void DisplaySawScreen()
    {
        if (playerData.sawCnt > 0)
        {
            toolMenu.Display(sawBtn.transform.GetChild(0).GetComponent<Image>());
            gamePlay.UseSaw();
        }
        else DisplayAddItemMenu(sawBtn, "Saw");
    }
    private void DisplayScrewdriveScreen()
    {
        if (playerData.screwdriveCnt > 0)
        {
            toolMenu.Display(screwdriveBtn.transform.GetChild(0).GetComponent<Image>());
            gamePlay.UseScrewdrive();
        }
        else DisplayAddItemMenu(screwdriveBtn, "Screwdrive");
    }
    private void DisplayAddItemMenu(Button btn, string type)
    {
        gamePlay.Pause();
        addItemMenu.Display(btn.transform.GetChild(0).GetComponent<Image>()
            , () => gamePlay.PurchaseItem(type)
            , () => gamePlay.PurchaseItem(type, true)
        );
    }
    public IEnumerator DisplayOpening()
    {
        Animator openingAnimator = openingUI.GetComponent<Animator>();
        openingUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(openingAnimator.GetCurrentAnimatorStateInfo(0).length);
        openingUI.gameObject.SetActive(false);
    }
    public IEnumerator DisplayHardStageWarningUI()
    {
        warningUI.Show();
        yield return StartCoroutine(warningUI.Display("HardStage"));
    }
    public IEnumerator DisplayAddTimeUI()
    {
        warningUI.Show();
        yield return StartCoroutine(warningUI.Display("AddTime"));
    }
    public IEnumerator DisplayClearStageUI()
    {
        warningUI.Show();
        yield return StartCoroutine(warningUI.Display("Clear"));
    }
    public void DisplayAddClockMenu() => DisplayAddItemMenu(clockBtn, "Clock");
    public void HideToolMenu() => toolMenu.Hide();
    public void DisplayLoseMenu() => loseMenu.Show();
    public void DisplayWinMenu() => winMenu.Show();
    public void DisplayUnlockHoleMenu(UnityAction unlockAction)
    {
        gamePlay.Pause();
        extraHoleMenu.Display(unlockAction);
    }
    public void RefreshToolIcon()
    {
        sawBtn.GetComponentInChildren<TextMeshProUGUI>().text = playerData.sawCnt > 0 ? ("" + playerData.sawCnt) : "+";
        screwdriveBtn.GetComponentInChildren<TextMeshProUGUI>().text = playerData.screwdriveCnt > 0 ? ("" + playerData.screwdriveCnt) : "+";
        clockBtn.GetComponentInChildren<TextMeshProUGUI>().text = playerData.clockCnt > 0 ? ("" + playerData.clockCnt) : "+";
    }
    public void ChangeBackground(Sprite sprite)
    {
        background.GetComponent<Image>().sprite = sprite;
    }
}