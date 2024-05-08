using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Item", menuName = "CreateItem")]
public class Item : ScriptableObject
{
    public Sprite icon; // アイコン
    public string name; // 名前
    public string description; // 情報
    public int num; // 個数

    public Item(Item item)
    {
        this.icon = item.icon;
        this.name = item.name;
        this.description = item.description;
        this.num = item.num;
    }
}