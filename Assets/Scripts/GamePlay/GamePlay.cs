using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class GamePlay : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private GameData gameData;

    [SerializeField] private GameUI gameUI;
    [SerializeField] private Screw screwPrefab;
    [SerializeField] private Hole boardHolePrefab;
    [SerializeField] private Hole plateHolePrefab;

    private float curTime;
    private float remainTime;
    private Screw curScrew;
    private Stage stage;
    private LevelData levelData;
    private AudioManager audioManager;
    private PlayerInput input;

    public void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
        input = GetComponent<PlayerInput>();
        gameUI.gamePlay = this;
        gameUI.ChangeBackground(GetBgData().sprite);
        IgnorePlateCollision();
    }
    public void Start() => InitLevel();
    public void OnCollisionEnter2D(Collision2D collision)
        => CheckDropPlate(collision.gameObject.GetComponent<Plate>());
    public void Update()
    {
        if (input.action == PlayerAction.Pause) return;
        if (curTime >= 0)
        {
            gameUI.DisplayTime(curTime);
            curTime -= Time.deltaTime;
        }
        else
        {
            audioManager.PlayOnLoseSound();
            Pause();
            gameUI.HideToolMenu();
            gameUI.DisplayLoseMenu();
        }
    }
    private void IgnorePlateCollision()
    {
        for (int i = 10; i < 15; i++)
            for (int j = i + 1; j < 15; j++)
                Physics2D.IgnoreLayerCollision(i, j);
    }
    private void CheckDropPlate(Plate plate)
    {
        if (stage.RemovePlate(plate))
        {
            audioManager.PlayPlateDropSound();
            if (stage.IsClear)
                StartCoroutine(NextStageOrFinished());
        }
    }
    public void Continue()
    {
        if (playerData.SpentCoin(gameData.continuePrice))
        {
            audioManager.Refresh();
            StartCoroutine(gameUI.DisplayAddTimeUI());
            AddTime(60);
            Resume();
        }
    }
    public IEnumerator NextStageOrFinished()
    {
        audioManager.PlayClearStageSound();
        gameUI.HideToolMenu();
        yield return StartCoroutine(gameUI.DisplayClearStageUI());
        if (++gameData.stageIndex < levelData.stages.Count)
        {
            audioManager.PlayStageSound();
            InitStage();
        }
        else
        {
            audioManager.PlayOnWinSound();
            Pause();
            gameUI.DisplayWinMenu();
            if (playerData.curLevel == gameData.selectedLv)
            {
                playerData.AddCoin(levelData.totalCoin);
                playerData.LevelUp();
            }
        }
    }
    public void Restart() => InitLevel();
    public void Replay()
    {
        if (playerData.SpentCoin(gameData.replayPrice * gameData.stageIndex))
        {
            curTime = remainTime;
            InitStage();
        }
    }
    public void InitLevel()
    {
        levelData = gameData.GetCurLevel();
        audioManager.PlayGamePlaySound(levelData.bgm);
        gameData.stageIndex = 0;
        curTime = levelData.time;
        InitStage();
        StartCoroutine(gameUI.DisplayOpening());
    }

    public void InitStage()
    {
        Resume();
        StartCoroutine(Init());
    }
    private IEnumerator Init()
    {
        remainTime = curTime;
        Destroy(stage?.gameObject);
        Stage newStagePrefab = levelData.stages[gameData.stageIndex];
        stage = Instantiate(newStagePrefab);
        stage.CreateScrewAndHole(screwPrefab, boardHolePrefab, plateHolePrefab);
        stage.transform.SetParent(transform, false);
        ShopItemData screwData = GetScrewData();
        stage.SetScrewSprite(screwData.sprite, screwData.spritePlus);
        stage.SetBackgroundSprite(GetBoardData().sprite);
        gameUI.DisplayStage();
        if (stage.isHard)
        {
            StartCoroutine(gameUI.DisplayHardStageWarningUI());
            yield return new WaitForSeconds(1.2f);
            audioManager.PlayWarningSound();
        }
    }
    public void DeselectScrew()
    {
        if (curScrew == null)
            return;
        audioManager.PlayUnselectScrewSound();
        curScrew.Select(false);
        curScrew = null;
    }
    public void SelectScrew(Screw s)
    {
        bool isSelect = curScrew != s;
        DeselectScrew();
        if (isSelect)
        {
            audioManager.PlaySelectScrewSound();
            s.Select(true);
            curScrew = s;
        }
    }
    public void MoveScrew(Vector3 pos)
    {
        if (curScrew == null) return;
        audioManager.PlayPinScrewSound();
        curScrew.Move(pos);
        var hits = Physics2D.OverlapPointAll(pos);
        foreach (var hit in hits)
            if (hit.TryGetComponent(out Hole h) && h.isPlateHole)
                curScrew.Connect(h.parent);
        curScrew.Select(false);
        curScrew = null;
    }
    public void RemovePlate(Plate p)
    {
        audioManager.PlayUseItemSound();
        gameUI.HideToolMenu();
        Resume();
        if (playerData.UseSaw())
        {
            gameUI.RefreshToolIcon();
            CheckDropPlate(p);
        }
    }
    public void RemoveScrew(Screw s)
    {
        audioManager.PlayUseItemSound();
        gameUI.HideToolMenu();
        Resume();
        if (playerData.UseScrewdrive())
        {
            stage.RemoveScrew(s);
            gameUI.RefreshToolIcon();
        }
    }
    public void UseClock()
    {
        if (playerData.UseClock())
        {
            StartCoroutine(gameUI.DisplayAddTimeUI());
            AddTime(45);
            gameUI.RefreshToolIcon();
        }
        else gameUI.DisplayAddClockMenu();
    }
    public void UseSaw()
    {
        DeselectScrew();
        input.action = PlayerAction.UseSaw;
    }
    public void UseScrewdrive()
    {
        DeselectScrew();
        input.action = PlayerAction.UseScrewdrive;
    }
    public void UnlockExtraHole(Hole h)
    {
        gameUI.DisplayUnlockHoleMenu(() =>
        {
            if (playerData.SpentCoin(gameData.holePrice))
                h.Unlock();
        });
    }
    public void AddTime(float time)
    {
        curTime += time;
        remainTime += time;
        audioManager.PlayAddTimeSound();
    }
    public void Pause()
    {
        input.action = PlayerAction.Pause;
        Time.timeScale = 0;
    }
    public void Resume()
    {
        input.action = PlayerAction.Play;
        Time.timeScale = 1;
    }
    public void PurchaseItem(string type, bool isX5 = false)
    {
        if (isX5 && playerData.SpentCoin(gameData.x5ItemPrice))
            AddItem(type, 5);
        if (!isX5 && playerData.SpentCoin(gameData.x1ItemPrice))
            AddItem(type);
    }
    private void AddItem(string type, int amount = 1)
    {
        switch (type)
        {
            case "Saw":
                playerData.AddSaw(amount);
                break;
            case "Clock":
                playerData.AddClock(amount);
                break;
            case "Screwdrive":
                playerData.AddScrewdrive(amount);
                break;
        }
        gameUI.RefreshToolIcon();
    }
    private ShopItemData GetScrewData() => GetShopItemData(gameData.GetScrewItemDataList, playerData.curScrewId);
    private ShopItemData GetBoardData() => GetShopItemData(gameData.GetBoardItemDataList, playerData.curBoardId);
    private ShopItemData GetBgData() => GetShopItemData(gameData.GetBackgroundItemDataList, playerData.curBgId);
    private ShopItemData GetShopItemData(List<ShopItemData> items, string id)
    {
        foreach (ShopItemData item in items)
            if (item.id == id)
                return item;
        return null;
    }
}