using UnityEngine;
using UnityEngine.UI;

class NoAdsMenu : SubMenu
{
    [SerializeField] private Button blockBtn;
    [SerializeField] private Button noAdsBtn;

    public override void Awake()
    {
        base.Awake();
        blockBtn.onClick.AddListener(() =>
        {
            playerData.RemoveAds();
            gameObject.SetActive(false);
            noAdsBtn.onClick.RemoveAllListeners();
            noAdsBtn.GetComponent<Animator>().enabled = false;
            noAdsBtn.transform.GetChild(0).gameObject.SetActive(false);
            noAdsBtn.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
            audioManager.PlaySuccessSound();
        });
        titleTxt.text = "No Ads";
    }
}