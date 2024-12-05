using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

class ExtraHoleMenu : SubMenu
{
    [SerializeField] private TextMeshProUGUI playerCoinTxt;
    [SerializeField] private Button unlockBtn;

    public UnityAction DisableAction { private get; set; }

    public override void Awake()
    {
        base.Awake();
        titleTxt.text = "Unlock hole";
    }
    public override void OnEnable()
    {
        base.OnEnable();
        playerCoinTxt.text = playerData.GetCoinText();
        unlockBtn.GetComponentInChildren<TextMeshProUGUI>().text = "" + gameData.holePrice;
        unlockBtn.interactable = gameData.holePrice <= playerData.totalCoin;
    }
    public void Display(UnityAction call)
    {
        Show();
        unlockBtn.onClick.RemoveAllListeners();
        unlockBtn.onClick.AddListener(audioManager.PlayPurchaseItemSound);
        unlockBtn.onClick.AddListener(Hide);
        unlockBtn.onClick.AddListener(call);
    }
    public void OnDisable() => DisableAction();
}