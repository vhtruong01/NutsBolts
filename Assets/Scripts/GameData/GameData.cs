using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AllLevelData")]
class GameData : ScriptableObject
{
    [SerializeField] private List<LevelData> allLevel;
    [SerializeField] private List<ShopItemData> screws;
    [SerializeField] private List<ShopItemData> backgrounds;
    [SerializeField] private List<ShopItemData> boards;
    [SerializeField] private int level;

    public int dailyRewardCoin { get; private set; }
    public int replayPrice { get; private set; }
    public int x1ItemPrice { get; private set; }
    public int x5ItemPrice { get; private set; }
    public int holePrice { get; private set; }
    public int continuePrice { get; private set; }
    public int selectedLv
    {
        get => level;
        set
        {
            stageIndex = 0;
            level = value >= allLevel.Count ? 0 : value;
        }
    }
    public int stageIndex { get; set; }
    public LevelData GetCurLevel() => allLevel[selectedLv];
    public LevelData GetLevel(int index) => allLevel[index];
    public int LevelCount() => allLevel.Count;
    public List<ShopItemData> GetScrewItemDataList => screws;
    public List<ShopItemData> GetBackgroundItemDataList => backgrounds;
    public List<ShopItemData> GetBoardItemDataList => boards;

    public void OnEnable()
    {
        dailyRewardCoin = 10000;
        replayPrice = 200;
        x1ItemPrice = 100;
        x5ItemPrice = 400;
        holePrice = 90;
        continuePrice = 120;
    }
}