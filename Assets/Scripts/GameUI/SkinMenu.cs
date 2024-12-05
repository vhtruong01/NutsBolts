using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

class SkinMenu : SubMenu
{
    private static Color lockColor = new(0.3f, 0.3f, 0.3f);
    [SerializeField] private Button screwBtn;
    [SerializeField] private Button boardBtn;
    [SerializeField] private Button bgBtn;
    [SerializeField] private Button itemBtn;
    [SerializeField] private GameObject itemsGroup;
    [SerializeField] private Image tick;
    [SerializeField] private Image border;
    [SerializeField] private Button purchaseBtn;
    [SerializeField] private TextMeshProUGUI coinTxt;
    [SerializeField] private TextMeshProUGUI noti;
    private Button prevBtn;
    private Dictionary<Button, List<Button>> tabs;

    public override void Awake()
    {
        base.Awake();
        tabs = new();
        titleTxt.text = "Item";
    }
    public void Start()
    {
        AddTab(screwBtn, gameData.GetScrewItemDataList, playerData.curScrewId, "Screw");
        AddTab(bgBtn, gameData.GetBackgroundItemDataList, playerData.curBgId, "Bg");
        AddTab(boardBtn, gameData.GetBoardItemDataList, playerData.curBoardId, "Board");
        SelectWindow(screwBtn);
        purchaseBtn.gameObject.SetActive(false);
    }
    public void AddTab(Button b, List<ShopItemData> items, string curItemId, string tab)
    {
        List<Button> btns = new();
        Image tickIcon = Instantiate(tick);
        Image borderIcon = Instantiate(border);
        foreach (ShopItemData data in items)
        {
            Button tempBtn = Instantiate(itemBtn);
            bool isOwn = playerData.ContainsItem(data.id);
            Image tempBtnImg = tempBtn.transform.GetChild(0).GetComponent<Image>();
            tempBtnImg.sprite = data.sprite;
            tempBtn.gameObject.name = data.name;
            tempBtn.gameObject.SetActive(false);
            tempBtn.transform.SetParent(itemsGroup.transform, false);
            tempBtn.transform.GetChild(1).gameObject.SetActive(!isOwn);
            if (!isOwn)
            {
                tempBtnImg.color = lockColor;
                tempBtn.GetComponent<Image>().color = lockColor;
            }
            tempBtn.onClick.AddListener(() => SelectItem(tempBtn, data, tab, tickIcon, borderIcon));
            if (curItemId == data.id) tempBtn.onClick.Invoke();
            btns.Add(tempBtn);
        }
        b.GetComponentInChildren<TextMeshProUGUI>().alpha = 0.5f;
        b.onClick.AddListener(() => SelectWindow(b));
        tabs.Add(b, btns);
    }
    public void SelectItem(Button itemBtn, ShopItemData data, string type, Image tickIcon, Image borderIcon)
    {
        borderIcon.transform.SetParent(itemBtn.transform, false);
        purchaseBtn.onClick.RemoveAllListeners();
        audioManager.PlaySelectItemSound();
        if (playerData.ContainsItem(data.id))
        {
            purchaseBtn.gameObject.SetActive(false);
            tickIcon.transform.SetParent(itemBtn.transform, false);
            playerData.SetItem(data.id, type);
            return;
        }
        if (data.canPurchase)
        {
            purchaseBtn.gameObject.SetActive(true);
            purchaseBtn.GetComponentInChildren<TextMeshProUGUI>().text = "" + data.price;
            purchaseBtn.onClick.AddListener(() =>
            {
                if (playerData.SpentCoin(data.price))
                {
                    playerData.AddItem(data.id);
                    playerData.SetItem(data.id, type);
                    itemBtn.transform.GetChild(1).gameObject.SetActive(false);
                    itemBtn.transform.GetChild(0).GetComponent<Image>().color = Color.white;
                    itemBtn.GetComponent<Image>().color = Color.white;
                    coinTxt.text = playerData.GetCoinText();
                    tickIcon.transform.SetParent(itemBtn.transform, false);
                    purchaseBtn.gameObject.SetActive(false);
                    audioManager.PlayPurchaseItemSound();
                }
                else if (!noti.gameObject.activeSelf)
                    StartCoroutine(ShowNotification("Lack of money!"));
            });
        }
        else purchaseBtn.gameObject.SetActive(false);
    }
    public IEnumerator ShowNotification(string message)
    {
        noti.gameObject.SetActive(true);
        noti.text = message;
        yield return new WaitForSeconds(1.5f);
        noti.gameObject.SetActive(false);
    }
    public void SelectWindow(Button selectedBtn)
    {
        audioManager.PlayTabSound();
        purchaseBtn.gameObject.SetActive(false);
        noti.gameObject.SetActive(false);
        if (prevBtn != null)
        {
            foreach (Button b in tabs.GetValueOrDefault(prevBtn))
                b.gameObject.SetActive(false);
            prevBtn.GetComponentInChildren<TextMeshProUGUI>().alpha = 0.5f;
        }
        foreach (Button b in tabs.GetValueOrDefault(selectedBtn))
            b.gameObject.SetActive(true);
        selectedBtn.GetComponentInChildren<TextMeshProUGUI>().alpha = 1f;
        prevBtn = selectedBtn;
    }
}