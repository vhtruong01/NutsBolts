using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

class ReplayMenu : SubMenu
{
    [SerializeField] private TextMeshProUGUI playerCoinTxt;
    [SerializeField] private Button replayBtn;
    private TextMeshProUGUI replayPriceTxt;

    public UnityAction DisableAction { private get; set; }

    public override void Awake()
    {
        base.Awake();
        replayBtn.onClick.AddListener(audioManager.PlayReplaySound);
        replayBtn.onClick.AddListener(Hide);
        replayPriceTxt = replayBtn.GetComponentInChildren<TextMeshProUGUI>();
        titleTxt.text = "Replay";
    }
    public override void OnEnable()
    {
        base.OnEnable();
        playerCoinTxt.text = playerData.GetCoinText();
        replayPriceTxt.text = "" + gameData.replayPrice * gameData.stageIndex;
        if (playerData.totalCoin < gameData.replayPrice * gameData.stageIndex)
        {
            replayPriceTxt.color = Color.red;
            replayBtn.interactable = false;
        }
        else
        {
            replayBtn.interactable = true;
            replayPriceTxt.color = Color.white;
        }
    }
    public void SetOnReplayListener(UnityAction call)
    {
        replayBtn.onClick.AddListener(call);
    }
    public void OnDisable() => DisableAction();
}