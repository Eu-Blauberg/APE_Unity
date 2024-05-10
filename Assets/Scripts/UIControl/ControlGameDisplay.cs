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
    [SerializeField] private Text[] inventoryNum = new Text[3];

    private Inventry inv = new Inventry();

    //ライフUIの更新
    public void UpdateLifeUI(int life)
    {
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
        if(inv.inventry.Count == 0)
        {
            for(int i = 0; i < 3; i++)
            {
                inventoryImage[0].sprite = null;
                inventoryNum[0].text = "";
            }
            return;
        }

        for(int i = 0; i < 3; i++)
        {
            if(inv.inventry.Count > i)
            {
                inventoryImage[i].sprite = inv.inventry[i].icon;
                inventoryNum[i].text = inv.inventry[i].num.ToString();
            }
            else
            {
                inventoryImage[i].sprite = null;
                inventoryNum[i].text = "";
                Debug.Log("null");
            }
        }
    }
}
