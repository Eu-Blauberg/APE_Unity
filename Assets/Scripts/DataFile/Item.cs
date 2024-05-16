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
    public ItemType itemType; // アイテムの種類å

    public enum ItemType
    {
        Nomal,
        TradeOnly,
    }

    public Item(Item item)
    {
        this.icon = item.icon;
        this.name = item.itemName;
        this.description = item.description;
        this.num = item.num;
    }
}