using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Item", menuName = "CreateItem")]
public class Item : ScriptableObject
{
    public Sprite icon; // アイコン
    public string itemName; // 名前
    public string description; // 情報
    public int num; // 個数
    public ItemType itemType; // アイテムの種類
    public ItemRarity itemRarity; // アイテムのレア度
    [HideInInspector]public int rarity; // アイテムのレア度

    // アイテムの種類
    public enum ItemType
    {
        Nomal,
        TradeOnly,
    }

    //アイテムのレア度
    public enum ItemRarity
    {
        Common = 1,
        Uncommon = 2,
        Rare = 3,
        Legendary = 4,
    }

    public Item(Item item)
    {
        this.icon = item.icon;
        this.name = item.itemName;
        this.description = item.description;
        this.num = item.num;
        this.itemType = item.itemType;
        this.rarity = (int)item.itemRarity;
    }
}