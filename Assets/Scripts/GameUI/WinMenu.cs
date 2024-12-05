 using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

class WinMenu : SubMenu
{
    [SerializeField] private TextMeshProUGUI playerCoinTxt;
    [SerializeField] private GameObject display1;
    [SerializeField] private GameObject display2;

    [SerializeField] private TextMeshProUGUI coinReward;
    [SerializeField] private Button nextLevelBtn;
    [SerializeField] private Button restartBtn;
    [SerializeField] private Button homeBtn;

    [SerializeField] private Image screwIcon;
    [SerializeField] private GameObject iconGroup;
    [SerializeField] private TextMeshProUGUI coinTxt;
    [SerializeField] private TextMeshProUGUI levelTxt;
    [SerializeField] private TextMeshProUGUI timeTxt;
    [SerializeField] private Button playBtn;
    [SerializeField] private Button cancelBtn;

    public override void Awake()
    {
        base.Awake();
        closeBtn.gameObject.SetActive(false);
        restartBtn.onClick.AddListener(Hide);
        cancelBtn.onClick.AddListener(() =>
        {
            display1.SetActive(true);
            display2.SetActive(false);
            titleTxt.text = "Victory";
        });
        nextLevelBtn.onClick.AddListener(() =>
        {
            display1.SetActive(false);
            display2.SetActive(true);
            titleTxt.text = "Next level";
        });
        playBtn.onClick.AddListener(() =>
        {
            ++gameData.selectedLv;
            restartBtn.onClick.Invoke();
        });
    }
    public override void OnEnable()
    {
        LevelData curLevelData = gameData.GetCurLevel();
        bool isCollected = playerData.curLevel > gameData.selectedLv;
        coinReward.text = "+" + (isCollected ? 0 : curLevelData.totalCoin);
        playerCoinTxt.text = playerData.GetCoinText()+coinReward.text;
        cancelBtn.onClick.Invoke();
        if (gameData.selectedLv + 1 >= gameData.LevelCount())
        {
            nextLevelBtn.interactable = false;
            return;
        }
        nextLevelBtn.interactable = true;
        LevelData nextLevelData = gameData.GetLevel(gameData.selectedLv + 1);
        foreach (var child in iconGroup.GetComponentsInChildren<Image>())
            Destroy(child.gameObject);
        for (int i = 0; i < nextLevelData.stages.Count; i++)
        {
            var icon = Instantiate(screwIcon);
            icon.gameObject.name = "Icon";
            if (nextLevelData.stages[i].isHard)
                icon.color = Color.yellow;
            icon.transform.SetParent(iconGroup.transform, false);
        }
        coinTxt.text = isCollected ? "Received" : ("" + nextLevelData.totalCoin);
        levelTxt.text = "" + (gameData.selectedLv + 1);
        timeTxt.text = "" + nextLevelData.time + "s";
    }
    public void SetOnRestartListener(UnityAction call)
    {
        restartBtn.onClick.AddListener(call);
    }
    public void SetOnHomeListener(UnityAction call)
    {
        homeBtn.onClick.AddListener(call);
    }
}