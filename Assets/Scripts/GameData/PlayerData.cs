using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerData")]
class PlayerData : ScriptableObject
{
    [SerializeField] private List<string> items;
    [SerializeField] private int coin;
    [SerializeField] private int level;
    [SerializeField] private int saw;
    [SerializeField] private int screwdrive;
    [SerializeField] private int clock;
    [SerializeField] private bool noAds;
    private float sound;
    private float sfx;
    private bool mute;
    private long time;
    public string curScrewId { get; private set; }
    public string curBoardId { get; private set; }
    public string curBgId { get; private set; }
    public float soundValue
    {
        get => sound;
        set
        {
            sound = value;
            PlayerPrefs.SetFloat("soundValue", sound);
        }
    }
    public float sfxValue
    {
        get => sfx;
        set
        {
            sfx = value;
            PlayerPrefs.SetFloat("sfxValue", sfx);
        }
    }
    public bool isMute
    {
        get => mute;
        set
        {
            mute = value;
            PlayerPrefs.SetInt("isMute", mute ? 1 : 0);
        }
    }
    public long collectingTime
    {
        get => time;
        set
        {
            time = value;
            PlayerPrefs.SetString("collectingTime", time.ToString());
        }
    }
    public int curLevel => level;
    public int sawCnt => saw;
    public int screwdriveCnt => screwdrive;
    public int clockCnt => clock;
    public bool isRemoveAds => noAds;
    public int totalCoin => coin;

    public void Awake() => InitData();
    public void OnEnable() => InitData();
    public string GetCoinText()
    {
        string rs = "";
        if (coin >= 1000000)
            rs = coin / 1000000 + "M";
        else if (coin >= 100000)
            rs = coin / 1000 + "k";
        else rs += coin;
        return rs;
    }
    public void LevelUp()
    {
        ++level;
        PlayerPrefs.SetInt("level", level);
    }
    public void AddCoin(int coin)
    {
        this.coin += coin;
        PlayerPrefs.SetInt("coin", this.coin);
    }
    public void AddSaw(int amount)
    {
        saw += amount;
        PlayerPrefs.SetInt("saw", saw);
    }
    public void AddClock(int amount)
    {
        clock += amount;
        PlayerPrefs.SetInt("clock", clock);
    }
    public void AddScrewdrive(int amount)
    {
        screwdrive += amount;
        PlayerPrefs.SetInt("screwdrive", screwdrive);
    }
    public bool SpentCoin(int coin)
    {
        if (this.coin < coin) return false;
        this.coin -= coin;
        PlayerPrefs.SetInt("coin", this.coin);
        return true;
    }
    public bool UseSaw()
    {
        if (sawCnt <= 0) return false;
        saw--;
        PlayerPrefs.SetInt("saw", saw);
        return true;
    }
    public bool UseClock()
    {
        if (clockCnt <= 0) return false;
        clock--;
        PlayerPrefs.SetInt("clock", clock);
        return true;
    }
    public bool UseScrewdrive()
    {
        if (screwdriveCnt <= 0) return false;
        screwdrive--;
        PlayerPrefs.SetInt("screwdrive", screwdrive);
        return true;
    }
    public void RemoveAds()
    {
        noAds = true;
        PlayerPrefs.SetInt("noAds", 1);
    }
    public void AddItem(string itemId)
    {
        if (!items.Contains(itemId))
        {
            items.Add(itemId);
            PlayerPrefs.SetString("items", string.Join(',', items));
        }
    }
    public bool ContainsItem(string itemId) => items.Contains(itemId);
    public void SetItem(string itemId, string type)
    {
        switch (type)
        {
            case "Screw":
                curScrewId = itemId;
                PlayerPrefs.SetString("curScrewId", curScrewId);
                break;
            case "Bg":
                curBgId = itemId;
                PlayerPrefs.SetString("curBgId", curBgId);
                break;
            case "Board":
                curBoardId = itemId;
                PlayerPrefs.SetString("curBoardId", curBoardId);
                break;
        }
    }
    public void InitData()
    {
        //PlayerPrefs.DeleteAll();
        Debug.Log("Init data");
        time = long.Parse(PlayerPrefs.GetString("collectingTime", "0"));
        coin = PlayerPrefs.GetInt("coin", 99000);
        level = PlayerPrefs.GetInt("level", 0);
        noAds = PlayerPrefs.GetInt("noAds", 0) == 1;
        saw = PlayerPrefs.GetInt("saw", 5);
        screwdrive = PlayerPrefs.GetInt("screwdrive", 5);
        clock = PlayerPrefs.GetInt("clock", 1);
        curScrewId = PlayerPrefs.GetString("curScrewId", "screw1");
        curBoardId = PlayerPrefs.GetString("curBoardId", "board1");
        curBgId = PlayerPrefs.GetString("curBgId", "bg1");
        sound = PlayerPrefs.GetFloat("soundValue", 0.75f);
        sfx = PlayerPrefs.GetFloat("sfxValue", 0.4f);
        mute = PlayerPrefs.GetInt("isMute", 0) == 1;
        string itemsData = PlayerPrefs.GetString("items", "");
        if (itemsData != "")
            items = new(itemsData.Split(','));
        else
        {
            items.Clear();
            items.Add(curScrewId);
            items.Add(curBoardId);
            items.Add(curBgId);
            PlayerPrefs.SetString("items", string.Join(',', items));
        }
    }
    //
    public void LoadData()
    {
        Debug.Log("Load data");
        //string data = PlayerPrefs.GetString(GetType().Name, null);
        string data = null;
        var path = Path.Combine(Application.persistentDataPath, GetType().Name);
        if (File.Exists(path))
        {
            using (var reader = new StreamReader(path))
            {
                data = reader.ReadToEnd();
                Debug.Log(data);
                JsonUtility.FromJsonOverwrite(data, this);
            }
        }
        else InitData();
    }
    public void SaveData()
    {
        Debug.Log("Save data");
        string data = JsonUtility.ToJson(this);
        Debug.Log(data);
        string path = Path.Combine(Application.persistentDataPath, GetType().Name);
        var stream = new FileStream(path, FileMode.OpenOrCreate);
        using (var writer = new StreamWriter(stream))
            writer.Write(data);
    }
}