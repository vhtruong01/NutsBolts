using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

class DailyUI : SubMenu
{
    [SerializeField] private TextMeshProUGUI cointTxt;
    private TextMeshProUGUI timeTxt;
    private Button btn;
    private long dayToMilis;
    private bool canCollect;

    public override void Awake()
    {
        dayToMilis = 24 * 60 * 60 * 1000;
        btn = GetComponent<Button>();
        timeTxt = GetComponentInChildren<TextMeshProUGUI>();
        btn.onClick.AddListener(CollectReward);
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }
    public override void OnEnable() { }
    public void Start()
    {
        StartCoroutine(StartClock());
    }
    public IEnumerator StartClock()
    {
        while (!canCollect)
        {
            long delta = playerData.collectingTime + dayToMilis - DateTimeOffset.Now.ToUnixTimeMilliseconds();
            if (delta <= 0)
            {
                canCollect = true;
                timeTxt.text = "";
                animator.enabled = true;
                break;
            }
            long h = delta / 3600000;
            delta %= 3600000;
            long m = delta / 60000;
            delta %= 60000;
            long s = delta / 1000;
            timeTxt.text = (h < 10 ? "0" : "") + h + ":" + (m < 10 ? "0" : "") + m + ":" + (s < 10 ? "0" : "") + s;
            yield return new WaitForSeconds(1);
        }
    }
    public void CollectReward()
    {
        if (!canCollect) return;
        playerData.collectingTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        playerData.AddCoin(gameData.dailyRewardCoin);
        cointTxt.text = playerData.GetCoinText();
        canCollect = false;
        animator.enabled = false;
        transform.rotation = Quaternion.identity;
        audioManager.PlaySuccessSound();
        StartCoroutine(StartClock());
    }
}