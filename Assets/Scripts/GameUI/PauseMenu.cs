using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

class PauseMenu : SettingMenu
{
    [SerializeField] private Button resumeBtn;
    [SerializeField] private Button restartBtn;
    [SerializeField] private Button homeBtn;

    public UnityAction DisableAction { private get; set; }

    public override void Awake()
    {
        base.Awake();
        restartBtn.onClick.AddListener(audioManager.PlayRestartSound);
        resumeBtn.onClick.AddListener(Hide);
        restartBtn.onClick.AddListener(Hide);
        titleTxt.text = "Pause";
    }
    public void SetOnRestartListener(UnityAction call)
    {
        restartBtn.onClick.AddListener(call);
    }
    public void SetOnHomeListener(UnityAction call)
    {
        homeBtn.onClick.AddListener(call);
    }
    public void OnDisable() => DisableAction();
}