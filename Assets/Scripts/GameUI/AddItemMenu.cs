using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

class AddItemMenu : SubMenu
{
    [SerializeField] private TextMeshProUGUI playerCoinTxt;
    [SerializeField] private Button purchaseBtn1;
    [SerializeField] private Button purchaseBtn2;
    [SerializeField] private Image icon1;
    [SerializeField] private Image icon2;

    public UnityAction DisableAction { private get; set; }

    public override void Awake()
    {
        base.Awake();
        titleTxt.text = "Purchase";
    }
    public override void OnEnable()
    {
        base.OnEnable();
        playerCoinTxt.text = playerData.GetCoinText();
        purchaseBtn1.GetComponentInChildren<TextMeshProUGUI>().text = "" + gameData.x1ItemPrice;
        purchaseBtn2.GetComponentInChildren<TextMeshProUGUI>().text = "" + gameData.x5ItemPrice;
        purchaseBtn1.interactable = gameData.x1ItemPrice <= playerData.totalCoin;
        purchaseBtn2.interactable = gameData.x5ItemPrice <= playerData.totalCoin;
    }
    public void Display(Image img, UnityAction x1Call, UnityAction x5Call)
    {
        Show();
        icon1.sprite = icon2.sprite = img.sprite;
        icon1.rectTransform.sizeDelta = icon2.rectTransform.sizeDelta = img.rectTransform.sizeDelta * 1.5f;
        purchaseBtn1.onClick.RemoveAllListeners();
        purchaseBtn2.onClick.RemoveAllListeners();
        purchaseBtn1.onClick.AddListener(x1Call);
        purchaseBtn2.onClick.AddListener(x5Call);
        purchaseBtn1.onClick.AddListener(audioManager.PlayPurchaseItemSound);
        purchaseBtn2.onClick.AddListener(audioManager.PlayPurchaseItemSound);
        purchaseBtn1.onClick.AddListener(Hide);
        purchaseBtn2.onClick.AddListener(Hide);
    }
    public void OnDisable() => DisableAction();
}