using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlGameDisplay : MonoBehaviour
{
    [SerializeField] private Image[] HeartImage;
    [SerializeField] private Sprite onLifeSprite;
    [SerializeField] private Sprite offLifeSprite;
    [SerializeField] private Image[] inventoryImage = new Image[3];
    [SerializeField] private Sprite NullSprite;
    [SerializeField] private Text[] inventoryNum = new Text[3];
    [SerializeField] private ItemDataBase itemDataBase;
    [SerializeField] private PlayerData playerData;

    void Start()
    {
        playerData.life = 3;

        // プレイヤーのライフを更新
        UpdateLifeUI();
        UpdateInventoryUI();
    }
    
    //ライフUIの更新
    public void UpdateLifeUI()
    {
        int life = playerData.life;
        switch(life)
        {
            case 3:
                HeartImage[0].sprite = onLifeSprite;
                HeartImage[1].sprite = onLifeSprite;
                HeartImage[2].sprite = onLifeSprite;
                break;
            case 2:
                HeartImage[0].sprite = onLifeSprite;
                HeartImage[1].sprite = onLifeSprite;
                HeartImage[2].sprite = offLifeSprite;
                break;
            case 1:
                HeartImage[0].sprite = onLifeSprite;
                HeartImage[1].sprite = offLifeSprite;
                HeartImage[2].sprite = offLifeSprite;
                break;
            case 0:
                HeartImage[0].sprite = offLifeSprite;
                HeartImage[1].sprite = offLifeSprite;
                HeartImage[2].sprite = offLifeSprite;
                break;
        }
    }

    //インベントリUIの更新
    public void UpdateInventoryUI()
    {
        //データベースを検索してアイテムの個数が1以上の場合
        for(int i = 0; i < itemDataBase.items.Count; i++)
        {
            if(itemDataBase.items[i].num > 0)
            {
                //アイテムの画像を表示
                inventoryImage[i].sprite = itemDataBase.items[i].icon;
                //アイテムの個数を表示
                inventoryNum[i].text = itemDataBase.items[i].num.ToString();
            }
            else
            {
                //アイテムの画像を非表示
                inventoryImage[i].sprite = NullSprite;
                //アイテムの個数を非表示
                inventoryNum[i].text = null;
            }
        }
    }
}
