using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

class LoseMenu : SubMenu
{
    [SerializeField] private TextMeshProUGUI playerCoinTxt;
    [SerializeField] private Button continueBtn;
    [SerializeField] private Button restartBtn;
    [SerializeField] private Button homeBtn;

    public override void Awake()
    {
        base.Awake();
        closeBtn.onClick.AddListener(homeBtn.onClick.Invoke);
        restartBtn.onClick.AddListener(Hide);
        continueBtn.onClick.AddListener(Hide);
        titleTxt.text = "Time out";
    }
    public override void OnEnable()
    {
        base.OnEnable();
        playerCoinTxt.text = playerData.GetCoinText();
        continueBtn.GetComponentInChildren<TextMeshProUGUI>().text = "-" + gameData.continuePrice;
        continueBtn.interactable = gameData.continuePrice <= playerData.totalCoin;
    }
    public void SetOnContinueListener(UnityAction call)
    {
        continueBtn.onClick.AddListener(call);
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