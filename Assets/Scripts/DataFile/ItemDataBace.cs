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
            else return;
        }
        else return;
    }
}
