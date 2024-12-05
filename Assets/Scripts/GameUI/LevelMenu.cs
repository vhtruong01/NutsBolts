using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

class LevelMenu : SubMenu
{
    [SerializeField] private Button playBtn;
    [SerializeField] private Image screwIcon;
    [SerializeField] private GameObject iconGroup;
    [SerializeField] private TextMeshProUGUI coinTxt;
    [SerializeField] private TextMeshProUGUI timeTxt;

    public override void Awake()
    {
        base.Awake();
        playBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("MainGame", LoadSceneMode.Single);
        });
    }
    public void Display(int levelIndex)
    {
        if (levelIndex >= gameData.LevelCount()) return;
        Show();
        LevelData levelData = gameData.GetLevel(levelIndex);
        titleTxt.text = "Level: " + levelIndex;
        foreach (var child in iconGroup.GetComponentsInChildren<Image>())
            Destroy(child.gameObject);
        for (int i = 0; i < levelData.stages.Count; i++)
        {
            var icon = Instantiate(screwIcon);
            icon.gameObject.name = "Icon";
            if (levelData.stages[i].isHard)
                icon.color = Color.yellow;
            icon.transform.SetParent(iconGroup.transform, false);
        }
        coinTxt.text = playerData.curLevel > levelIndex ? "Received" : ("" + levelData.totalCoin);
        timeTxt.text = "" + levelData.time + "s";
        gameData.selectedLv = levelIndex;
    }
}