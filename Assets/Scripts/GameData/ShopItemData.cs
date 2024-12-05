using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ShopItemData")]
class ShopItemData : ScriptableObject
{
    public string id;
    public string itemName;
    public string howToGet;
    public int price;
    public bool canPurchase;
    public Sprite sprite;
    public Sprite spritePlus;
}