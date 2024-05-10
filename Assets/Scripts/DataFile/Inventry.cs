using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventry
{
    public List<Item> inventry = new List<Item>();
    private ControlGameDisplay controlGameDisplay = new ControlGameDisplay();

    void Start()
    {
        controlGameDisplay = GameObject.Find("DisplayManager").GetComponent<ControlGameDisplay>();

        //UIにアイテムを反映
        controlGameDisplay.UpdateInventoryUI();
    }

    public void AddItem(Item item)
    {
        //inventryにitemがある場合
        if(inventry.Contains(item))
        {
            //itemの個数を増やす
            item.num++;
        }
        else
        {
            //inventryにitemがない場合
            //itemをinventryに追加
            inventry.Add(item);
            Debug.Log("AddItem(" + item.itemName + ")");
        }

        //UIにアイテムを反映
        controlGameDisplay.UpdateInventoryUI();
    }

    public void RemoveItem(Item item)
    {
        //inventryにitemがある場合
        if(inventry.Contains(item))
        {
            //itemの個数が1より大きい場合
            if(item.num > 1)
            {
                //itemの個数を減らす
                item.num--;
            }
            else
            {
                //itemの個数が1の場合
                //inventryからitemを削除
                inventry.Remove(item);
            }
        }

        //UIにアイテムを反映
        controlGameDisplay.UpdateInventoryUI();
    }   
}