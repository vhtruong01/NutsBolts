using UnityEngine;
using UnityEngine.UI;

class SettingMenu : SubMenu
{
    [SerializeField] private Slider soundBar;
    [SerializeField] private Slider sfxBar;
    [SerializeField] private Toggle muteCB;

    public override void Awake()
    {
        base.Awake();
        soundBar.value = playerData.soundValue;
        sfxBar.value = playerData.sfxValue;
        muteCB.isOn = playerData.isMute;
        soundBar.onValueChanged.AddListener(v => audioManager.ChangeSoundVolume(v));
        sfxBar.onValueChanged.AddListener(v => audioManager.ChangeSfxVolume(v));
        muteCB.onValueChanged.AddListener(v => audioManager.SetMute(v));
        titleTxt.text = "Setting";
    }
}