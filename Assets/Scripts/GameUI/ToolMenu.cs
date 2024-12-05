using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

class ToolMenu : SubMenu
{
    [SerializeField] private Image icon;

    public UnityAction DisableAction { private get; set; }

    public void Display(Image img)
    {
        Show();
        icon.sprite = img.sprite;
        icon.GetComponent<RectTransform>().sizeDelta = img.GetComponent<RectTransform>().sizeDelta * 1.5f;
    }
    public void OnDisable() => DisableAction();
}