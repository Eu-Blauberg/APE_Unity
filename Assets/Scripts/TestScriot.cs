using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestScriot : MonoBehaviour
{
    [SerializeField] TradeRate tradeRate;

    private Text[] Texts = new Text[3];
    private Image[] Images = new Image[2];

    void Start(){

        if(tradeRate == null){
            Debug.LogError("tradeRate is null");
        }

        for(int i = 0; i < Texts.Length; i++){
            Texts[i] = GameObject.Find("Text" + (i + 1)).GetComponent<Text>();
        }
        for(int i = 0; i < Images.Length; i++){
            Images[i] = GameObject.Find("Image" + (i + 1)).GetComponent<Image>();
        }

        for(int i = 0; i < Texts.Length; i++){
            Texts[i].text = "Text" + (i + 1);
        }

        for(int i = 0; i < Images.Length; i++){
            Item _item = tradeRate.tradeRates[i].SellItem;
            Images[i].sprite = _item.icon;
        }
    }
}
