using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataBase", menuName = "CreateItemDataBase")]
public class ItemDataBase : ScriptableObject
{
    public List<Item> items = new List<Item>();

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
            Debug.Log("AddItem(" + item.itemName + ")");
        }

        SortItem();
    }

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
                //itemsからitemを削除
                items.Remove(item);
                Debug.Log("RemoveItem(" + item.itemName + ")");
            }
        }
        SortItem();
    }

    //アイテムを個数順に並び替える
    private void SortItem()
    {
        items.Sort((a, b) => b.num - a.num);
    }
}
