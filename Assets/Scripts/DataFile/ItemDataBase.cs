using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ItemDataBase", menuName = "CreateItemDataBase")]
public class ItemDataBase : ScriptableObject
{
    public List<Item> items = new List<Item>();

    //アイテムを追加
    public void AddItem(Item item)
    {
        //itemsにitemがある場合
        if(items.Contains(item))
        {
            //itemの個数を増やす
            item.num++;
        }
        else
        {
            //itemsにitemがない場合
            //itemをitemsに追加
            items.Add(item);
        }

        SortItem();
    }

    //アイテムを削除
    public void RemoveItem(Item item)
    {
        //itemsにitemがある場合
        if(items.Contains(item))
        {
            //itemの個数が1以上の場合
            if(item.num > 0)
            {
                //itemの個数を減らす
                item.num--;
            }
            else
            {
                //itemの個数が0の場合
                return;
            }
        }
        SortItem();
    }

    //アイテムを個数順に並び替える
    private void SortItem()
    {
        items.Sort((a, b) => b.num - a.num);
    }

    //アイテムの個数を取得
    public int GetItemNum(Item item)
    {
        return item.num;
    }

    //アイテムの種類でアイテムを取得
    public List<Item> GetItemByType(Item.ItemType itemType)
    {
        return items.FindAll(item => item.itemType == itemType);
    }

    //アイテムの名前でアイテムを取得
    public Item GetItemByName(string itemName)
    {
        return items.Find(item => item.itemName == itemName);
    }
}
