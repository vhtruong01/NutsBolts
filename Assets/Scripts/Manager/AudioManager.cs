using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource bgm;
    private AudioSource sfx;
    private AudioSource sfxExtra;
    [SerializeField] private PlayerData playerData;

    [SerializeField] private AudioClip barSfx;
    [SerializeField] private AudioClip selectItemSfx;
    [SerializeField] private AudioClip purchaseItemSfx;
    [SerializeField] private AudioClip tabSfx;
    [SerializeField] private AudioClip successSfx;
    [SerializeField] private AudioClip menuSfx;
    [SerializeField] private AudioClip menu2Sfx;
    [SerializeField] private AudioClip mainMenuBgm;

    [SerializeField] private AudioClip replaySfx;
    [SerializeField] private AudioClip restartSfx;
    [SerializeField] private AudioClip useItemSfx;
    [SerializeField] private AudioClip addTimeSfx;
    [SerializeField] private AudioClip selectScrewSfx;
    [SerializeField] private AudioClip unselectScrewSfx;
    [SerializeField] private AudioClip pinScrewSfx;
    [SerializeField] private AudioClip plateDropSfx;
    [SerializeField] private AudioClip stageSfx;
    [SerializeField] private AudioClip warningSfx;
    [SerializeField] private AudioClip winSfx;
    [SerializeField] private AudioClip loseSfx;
    [SerializeField] private AudioClip clearStageSfx;

    public void Awake()
    {
        bgm = transform.GetChild(0).GetComponent<AudioSource>();
        sfx = transform.GetChild(1).GetComponent<AudioSource>();
        sfxExtra = transform.GetChild(2).GetComponent<AudioSource>();
        Refresh();
    }
    public void PlayBgm(AudioClip clip)
    {
        if (clip == null) return;
        Refresh();
        bgm.clip = clip;
        bgm.Play();
    }
    public void PlayeSfx(AudioClip clip, bool isExtra = false)
    {
        if (!isExtra)
        {
            if (sfx.isPlaying) return;
            sfx.clip = clip;
            sfx.Play();
        }
        else
        {
            sfx.Stop();
            sfxExtra.clip = clip;
            sfxExtra.Play();
        }
    }
    public void SetMute(bool isMute)
    {
        bgm.mute = sfx.mute = sfxExtra.mute = playerData.isMute = isMute;
    }
    public void ChangeSoundVolume(float volume)
    {
        playerData.soundValue = volume;
        bgm.volume = volume;
    }
    public void ChangeSfxVolume(float volume)
    {
        playerData.sfxValue = volume;
        sfx.volume = volume;
        sfxExtra.volume = volume;
    }
    public void PlayMainMenuSound() => PlayBgm(mainMenuBgm);
    public void PlayGamePlaySound(AudioClip clip) => PlayBgm(clip);
    public void PlaySelectItemSound() => PlayeSfx(selectItemSfx);
    public void PlayPurchaseItemSound() => PlayeSfx(purchaseItemSfx);
    public void PlayTabSound() => PlayeSfx(tabSfx);
    public void PlaySuccessSound() => PlayeSfx(successSfx);
    public void PlayOpenMenuSound() => PlayeSfx(menuSfx);
    public void PlayCloseMenuSound() => PlayeSfx(menu2Sfx, true);
    public void PlayReplaySound() => PlayeSfx(replaySfx);
    public void PlayRestartSound() => PlayeSfx(restartSfx);
    public void PlayUseItemSound() => PlayeSfx(useItemSfx, true);
    public void PlayAddTimeSound() => PlayeSfx(addTimeSfx);
    public void PlaySelectScrewSound() => PlayeSfx(selectScrewSfx, true);
    public void PlayUnselectScrewSound() => PlayeSfx(unselectScrewSfx, true);
    public void PlayPinScrewSound() => PlayeSfx(pinScrewSfx, true);
    public void PlayPlateDropSound() => PlayeSfx(plateDropSfx, true);
    public void PlayWarningSound() => PlayeSfx(warningSfx, true);
    public void PlayClearStageSound() => PlayeSfx(clearStageSfx, true);
    public void PlayStageSound() => PlayeSfx(stageSfx, true);
    public void PlayOnWinSound()
    {
        bgm.volume = playerData.soundValue * 0.4f;
        bgm.loop = false;
        PlayeSfx(winSfx, true);
    }
    public void PlayOnLoseSound()
    {
        bgm.volume = playerData.soundValue * 0.4f;
        bgm.loop = false;
        PlayeSfx(loseSfx, true);
    }
    public void Refresh()
    {
        bgm.loop = true;
        bgm.volume = playerData.soundValue;
        sfx.volume = sfxExtra.volume = playerData.sfxValue;
        bgm.mute = sfx.mute = sfxExtra.mute = playerData.isMute;
    }
}
