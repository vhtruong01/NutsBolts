using TMPro;
using UnityEngine;
using UnityEngine.UI;

class SubMenu : MonoBehaviour
{
    protected PlayerData playerData { get; set; }
    protected GameData gameData { get; set; }

    protected Button closeBtn { get; set; }
    protected TextMeshProUGUI titleTxt { get; set; }
    protected GameObject content { get; set; }
    private AudioManager _audioManager;
    protected Animator animator;
    protected AudioManager audioManager
    {
        get
        {
            if (_audioManager == null)
                _audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
            return _audioManager;
        }
    }
    public virtual void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator != null)
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        titleTxt = GetComponentInChildren<TextMeshProUGUI>();
        closeBtn = GetComponentInChildren<Button>();
        content = GetComponentInChildren<Image>().gameObject;
        closeBtn.onClick.AddListener(Hide);
    }
    public virtual void OnEnable()
    {
        audioManager?.PlayOpenMenuSound();
    }
    public void Show() => gameObject.SetActive(true);
    public void Hide()
    {
        if (gameObject.activeSelf == false) return;
        audioManager.PlayCloseMenuSound();
        gameObject.SetActive(false);
    }
    public void SetData(PlayerData playerData, GameData gameData)
    {
        this.playerData = playerData;
        this.gameData = gameData;
    }
}