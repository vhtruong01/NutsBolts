using TMPro;
using UnityEngine;
using UnityEngine.UI;

class MainMenu : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private GameData gameData;

    [SerializeField] private ScrollRect levelGroupUI;
    [SerializeField] private TextMeshProUGUI coinTxt;
    [SerializeField] private Button curLevelBtn;
    [SerializeField] private Button settingBtn;
    [SerializeField] private Button themeBtn;
    [SerializeField] private Button noAdsBtn;
    [SerializeField] private Button levelBtn;

    [SerializeField] private LevelMenu levelMenu;
    [SerializeField] private SettingMenu settingMenu;
    [SerializeField] private SkinMenu skinMenu;
    [SerializeField] private NoAdsMenu noAdsMenu;
    [SerializeField] private DailyUI dailyUI;
    private AudioManager audioManager;

    [RuntimeInitializeOnLoadMethod]
    public static void InitApplication()
    {
        Application.targetFrameRate = 60;
    }
    public void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
        SetMenuData();
    }
    public void Start()
    {
        audioManager.PlayMainMenuSound();
        SetupButton();
        DisplayLevelUI();
    }
    private void SetMenuData()
    {
        levelMenu.SetData(playerData, gameData);
        noAdsMenu.SetData(playerData, gameData);
        dailyUI.SetData(playerData, gameData);
        skinMenu.SetData(playerData, gameData);
        settingMenu.SetData(playerData, gameData);
    }
    private void SetupButton()
    {
        settingBtn.onClick.AddListener(settingMenu.Show);
        themeBtn.onClick.AddListener(skinMenu.Show);
        if (playerData.isRemoveAds)
        {
            noAdsBtn.GetComponent<Animator>().enabled = false;
            noAdsBtn.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
            noAdsBtn.transform.GetChild(0).gameObject.SetActive(false);
        }
        else noAdsBtn.onClick.AddListener(noAdsMenu.Show);
        coinTxt.text = playerData.GetCoinText();
    }
    private void DisplayLevelUI()
    {
        curLevelBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Current level: " + playerData.curLevel;
        for (int i = gameData.LevelCount() - 1; i >= 0; i--)
        {
            Button lv = Instantiate(levelBtn);
            var txt = lv.GetComponentInChildren<TextMeshProUGUI>();
            txt.text = "" + i;
            var imgs = lv.GetComponentsInChildren<Image>();
            imgs[0].transform.Translate(150 * (i % 2 == 0 ? 1 : -1), 0, 0);
            imgs[1].transform.Translate(150 * (i % 2 == 0 ? 1 : -1), 0, 0);
            int index = i;
            if (i <= playerData.curLevel)
            {
                txt.alpha = 0.5f;
                lv.onClick.AddListener(() => levelMenu.Display(index));
                imgs[1].gameObject.SetActive(false);
                if (i == playerData.curLevel) txt.alpha = 1;
            }
            else imgs[1].gameObject.SetActive(true);
            lv.transform.SetParent(levelGroupUI.content.transform, false);
            lv.gameObject.name = "Lv" + i;
        }
        levelGroupUI.normalizedPosition = new Vector2(0, 1f * playerData.curLevel / gameData.LevelCount());
        curLevelBtn.onClick.AddListener(() =>
        {
            levelGroupUI.normalizedPosition = new Vector2(0, 1f * playerData.curLevel / gameData.LevelCount());
            levelMenu.Display(playerData.curLevel);
        });
    }
}