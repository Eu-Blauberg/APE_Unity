using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    //体力
    public int life;
    //取得したアイテムを保持するリスト
    public List<Item> holdItems = new List<Item>();

    //アイテムを取得するメソッド
    public void GetItem(Item item)
    {
        if (holdItems.Contains(item))
        {
            //アイテムを持っている場合は個数を増やす
            item.num++;
        }
        holdItems.Add(item);
    }

    //アイテムを使用するメソッド
    public void UseItem(Item item)
    {
        if (holdItems.Contains(item))
        {
            //アイテムを持っている場合は個数を減らす
            item.num--;
            if (item.num <= 0)
            {
                holdItems.Remove(item);
            }
        }
    }
}
