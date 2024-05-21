using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlBackpack : MonoBehaviour
{
    [SerializeField] ItemDataBase itemDataBase;

    //オブジェクトに衝突した際に呼び出される
    private void OnCollisionEnter(Collision collision)
    {
        //衝突したオブジェクトがプレイヤーでない場合は処理を終了
        if (!collision.gameObject.CompareTag("Player")) return;

        //アイテムを取得
        Item item = GetRandomItem();

        //アイテムが取得できなかった場合は処理を終了
        if (item == null) return;

        //アイテムの情報を表示 (赤文字で表示される)
        Debug.Log($"アイテム名: {item.itemName}");
        Debug.Log($"アイテムの種類: {item.itemType}");
        Debug.Log($"アイテムのレア度: {item.itemRarity}");

        //アイテムをデータベースに追加
        itemDataBase.AddItem(item);

        //インベントリUIを更新
        ControlGameDisplay controlGameDisplay = GameObject.FindWithTag("GameDisplayMaster").GetComponent<ControlGameDisplay>();
        controlGameDisplay.UpdateInventoryUI();

        //アイテムを非表示にする
        gameObject.GetComponent<Renderer>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;
        
        var message = item.itemName + "を入手した";
        StartCoroutine(DisplayNews(item, message));

    }

    //レアリティを基準に重み付き確立でアイテムを取得
    private Item GetRandomItem()
    {
        //レアリティごとの重み付き確立
        Dictionary<Item.ItemRarity, float> rarityWeight = new Dictionary<Item.ItemRarity, float>
        {
            { Item.ItemRarity.Common, 0.6f },
            { Item.ItemRarity.Uncommon, 0.4f },
            /*
            { Item.ItemRarity.Rare, 0.0f },
            { Item.ItemRarity.Legendary, 0.0f },
            */
        };

        //重み付き確立を元にレアリティを取得
        Item.ItemRarity rarity = GetRandomRarity(rarityWeight);

        //データベース内のアイテムでitemTypeがNomalのものを取得
        List<Item> items = itemDataBase.GetItemByType(Item.ItemType.Nomal);

        //レアリティが一致するアイテムを取得
        List<Item> rarityItems = items.FindAll(item => item.itemRarity == rarity);

        //レアリティが一致するアイテムがない場合はnullを返す
        if (rarityItems.Count == 0) return null;

        //レアリティが一致するアイテムの中からランダムで一つ取得
        return rarityItems[Random.Range(0, rarityItems.Count)];
    }

    //重み付き確立を元にレアリティを取得
    private Item.ItemRarity GetRandomRarity(Dictionary<Item.ItemRarity, float> rarityWeight)
    {
        //重み付き確立の合計値を取得
        float totalWeight = 0;
        foreach (var weight in rarityWeight)
        {
            totalWeight += weight.Value;
        }

        //重み付き確立の合計値を元に乱数を生成
        float randomValue = Random.Range(0, totalWeight);

        //重み付き確立を元にレアリティを取得
        float weightSum = 0;
        foreach (var weight in rarityWeight)
        {
            weightSum += weight.Value;
            //乱数が重み付き確立の合計値を超えた場合、そのレアリティを返す
            if (randomValue < weightSum) return weight.Key;
        }

        //どの条件にも該当しない場合(ここに到達することはない)
        return Item.ItemRarity.Common;
    }

    //コルーチンを使ってニュースを表示
    IEnumerator DisplayNews(Item item, string message)
    {
        //NoticeTextを取得(タグで検索)
        var noticeTextObj = GameObject.FindGameObjectWithTag("NoticeText");
        Text noticeText = noticeTextObj.GetComponent<Text>();

        noticeText.text = message;
        yield return new WaitForSecondsRealtime(2f);
        Debug.Log("2秒経過");
        noticeText.text = "";

        //オブジェクトを削除
        Destroy(gameObject);
    }
}
