using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TradeRate", menuName = "CreateTradeRate")]
public class TradeRate : ScriptableObject
{
    public List<Rate> tradeRates = new List<Rate>();

}

[System.Serializable]
public class Rate
{
    public Item SellItem; // 売るアイテム
    public int SellNum; // 売るアイテムの数
    public Item BuyItem; // 買うアイテム 
    public int BuyNum; // 買うアイテムの数

    public Rate(Item sellItem, int sellNum, Item buyItem, int buyNum)
    {
        SellItem = sellItem;
        SellNum = sellNum;
        BuyItem = buyItem;
        BuyNum = buyNum;
    }
}

