using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ControlTradeWindow: MonoBehaviour
{
    [SerializeField] InputActionAsset inputActionAsset;
    [SerializeField] ItemDataBase itemDataBase;
    [SerializeField] TradeRate tradeRate;
    [SerializeField] Text CursorAllowText;

    private GameInputs gameInputs;
    private int currentMenuIndex = 0; // 現在選択中のアイテムのインデックス
    private int currentOptionIndex = 0;   // 現在選択中の選択肢のインデックス
    private int i;
    private float offset = -200.0f;
    
    private Image[] tradeItemImage = new Image[2]; //アイテムの画像(0:売るアイテム, 1:買うアイテム)
    private Text[] itemNameText = new Text[2]; //アイテムの名前を表示するテキスト(0:売るアイテム, 1:買うアイテム)
    private Text[] itemNumText = new Text[2]; //売買のアイテム数を表示するテキスト(0:売るアイテム, 1:買うアイテム)
    private Text menuIndexText; //現在選択中のア取引内容のインデックスを表示するテキスト

    private Text[] optionText = new Text[2];  //選択肢のテキスト(0:戻る, 1:交換)
    private Vector3[] optionPosition = new Vector3[2]; //選択肢の位置(0:戻る, 1:交換)
    private Vector3 cursorPosition; //カーソルの位置

    /*
    オンオフでアイテムと選択肢の左右操作を切り替えるためのフラグ
    上下入力で切り替える
    trueの場合はcurrentMenuIndexが増減し,アイテムが切り替わる
    falseの場合はcurrentOptionIndexが増減し,選択肢が切り替わる
    */
    private bool isDown;

    void Awake()
    {
        //各オブジェクトを取得
        tradeItemImage[0] = GameObject.Find("SellItemImage").GetComponent<Image>();
        tradeItemImage[1] = GameObject.Find("BuyItemImage").GetComponent<Image>();
        optionText[0] = GameObject.Find("ReturnOptionText").GetComponent<Text>();
        optionText[1] = GameObject.Find("TradeOptionText").GetComponent<Text>();
        optionPosition[0] = GameObject.Find("ReturnOptionText").transform.position;
        optionPosition[1] = GameObject.Find("TradeOptionText").transform.position;
        itemNameText[0] = GameObject.Find("SellItemNameText").GetComponent<Text>();
        itemNameText[1] = GameObject.Find("BuyItemNameText").GetComponent<Text>();
        itemNumText[0] = GameObject.Find("SellNumText").GetComponent<Text>();
        itemNumText[1] = GameObject.Find("BuyNumText").GetComponent<Text>();
        menuIndexText = GameObject.Find("MenuIndexText").GetComponent<Text>();
    }

    void Start()
    {
        //GameInputsを取得
        gameInputs = new GameInputs();
        if(gameInputs == null) Debug.Log("GameInputs is null");

        //UIControlsの各入力に対してコールバック関数を登録
        gameInputs.UIControls.Up.performed += OnUpPerformed;
        gameInputs.UIControls.Down.performed += OnDownPerformed;
        gameInputs.UIControls.Right.performed += OnRightPerformed;
        gameInputs.UIControls.Left.performed += OnLeftPerformed;
        gameInputs.UIControls.Click.performed += OnClickPerformed;

        //UIControlsを有効化
        gameInputs.Enable();

        currentMenuIndex = 0;
        currentOptionIndex = 0;
        isDown = false;

        UpdateMenuIndexText();
        UpdateMenuWindow(currentMenuIndex);

        if(CursorAllowText == null) Debug.Log("CursorAllowText is null");

        CursorAllowText.text = "";
    }

    //非アクティブ時に呼び出される
    private void OnDestroy()
    {
        gameInputs?.Dispose();
    }

    private void OnRightPerformed(InputAction.CallbackContext context){
        SoundManager.Instance.PlaySE(SESoundData.SE.Cursor);
        if(gameObject.activeInHierarchy == false) return; //自身がヒエラルキー上で非アクティブなら処理を抜ける
        if(isDown){
            //選択肢の右移動(アイテムが売買できない場合はcurrentOptionIndexが0のままになる)
            if(CanTradeItem() == false) currentOptionIndex = 0;
            else if(currentOptionIndex == 0) currentOptionIndex = 1;
            else currentOptionIndex = 0;

            Debug.Log("Option Right (" + currentOptionIndex + ")");

            //CursorAllowTextTextの座標を選択肢に合わせる
            CursorAllowText.transform.position = new Vector3(optionPosition[currentOptionIndex].x + offset, optionPosition[currentOptionIndex].y, optionPosition[currentOptionIndex].z);
            UpdateMenuIndexText();
        }
        else
        {
            //アイテムの右移動
            if(currentMenuIndex == tradeRate.tradeRates.Count - 1) currentMenuIndex = 0;
            else currentMenuIndex++;
            Debug.Log("Trade Right (" + currentMenuIndex + ")");

            UpdateMenuWindow(currentMenuIndex);
            UpdateMenuIndexText();
        }
    }

    private void OnLeftPerformed(InputAction.CallbackContext context){
        SoundManager.Instance.PlaySE(SESoundData.SE.Cursor);
        if(gameObject.activeInHierarchy == false) return; //自身がヒエラルキー上で非アクティブなら処理を抜ける
        if(isDown){
            //選択肢の左移動(アイテムが売買できない場合はcurrentOptionIndexが0のままになる)
            if(CanTradeItem() == false) currentOptionIndex = 0;
            else if(currentOptionIndex == 0) currentOptionIndex = 1;
            else currentOptionIndex = 0;

            Debug.Log("Option Left (" + currentOptionIndex + ")");
            //CursorAllowTextTextの座標を選択肢に合わせる
            CursorAllowText.transform.position = new Vector3(optionPosition[currentOptionIndex].x + offset, optionPosition[currentOptionIndex].y, optionPosition[currentOptionIndex].z);
            UpdateMenuIndexText();
        }
        else
        {
            //アイテムの左移動
            if(currentMenuIndex == 0) currentMenuIndex = tradeRate.tradeRates.Count - 1;
            else currentMenuIndex--;
            Debug.Log("Trade Left (" + currentMenuIndex + ")");

            UpdateMenuWindow(currentMenuIndex);
            UpdateMenuIndexText();
        }
    }
        
    private void OnDownPerformed(InputAction.CallbackContext context){
        SoundManager.Instance.PlaySE(SESoundData.SE.Cursor);
        if(gameObject.activeInHierarchy == false) return; //自身がヒエラルキー上で非アクティブなら処理を抜ける
        Debug.Log("Down");

        isDown = true;

        CursorAllowText.text = "→";
    }

    private void OnUpPerformed(InputAction.CallbackContext context){
        SoundManager.Instance.PlaySE(SESoundData.SE.Cursor);
        if(gameObject.activeInHierarchy == false) return; //自身がヒエラルキー上で非アクティブなら処理を抜ける
        Debug.Log("Up");

        isDown = false;
        currentOptionIndex = 0;

        CursorAllowText.text = "";
    }

    private void OnClickPerformed(InputAction.CallbackContext context){
        SoundManager.Instance.PlaySE(SESoundData.SE.Click);
        if(gameObject.activeInHierarchy == false) return; //自身がヒエラルキー上で非アクティブなら処理を抜ける
        if(!isDown) return; //戻るボタン選択中でない場合は処理を抜ける

        Debug.Log("Click");

        //選択肢の処理
        switch(currentOptionIndex)
        {
            case 0:
                //親のオブジェクトを取得
                var _gameObject = GameObject.Find("TradeCanvas");
                Destroy(_gameObject);
                Time.timeScale = 1.0f;
                break;
            case 1:
                if(CanTradeItem() == true) TradeItem(); //アイテムを買う

                if(CanTradeItem() == false) 
                {
                    CursorAllowText.text = "";
                    isDown = false;
                    currentOptionIndex = 0;
                    CursorAllowText.transform.position = new Vector3(optionPosition[currentOptionIndex].x + offset, optionPosition[currentOptionIndex].y, optionPosition[currentOptionIndex].z);
                }

                //アイテムの情報を更新
                UpdateMenuWindow(currentMenuIndex);
                
                ControlGameDisplay controlGameDisplay = GameObject.FindWithTag("GameDisplayMaster").GetComponent<ControlGameDisplay>();
                controlGameDisplay.UpdateInventoryUI();
                break;
        }
    }

    //アイテムウィンドウの情報を更新する
    private void UpdateMenuWindow(int index)
    {
        //売るアイテム
        tradeItemImage[0].sprite = tradeRate.tradeRates[index].SellItem.icon;
        itemNameText[0].text = tradeRate.tradeRates[index].SellItem.itemName;
        itemNumText[0].text = tradeRate.tradeRates[index].SellNum.ToString();

        //買うアイテム
        tradeItemImage[1].sprite = tradeRate.tradeRates[index].BuyItem.icon;
        itemNameText[1].text = tradeRate.tradeRates[index].BuyItem.itemName;
        itemNumText[1].text = tradeRate.tradeRates[index].BuyNum.ToString();

        //アイテムが売買できない場合はoptinText[1]を空にする
        if(CanTradeItem() == false) optionText[1].text = "";
        else optionText[1].text = "交換";
    }

    private void UpdateMenuIndexText()
    {
        menuIndexText.text = "";
        for(int i = 0; i < tradeRate.tradeRates.Count; i++)
        {
            if(i == currentMenuIndex) menuIndexText.text += "＊";
            else menuIndexText.text += "・";
        }
    }

    //アイテムを売買する
    private void TradeItem()
    {
        var nowInventryItemNum = itemDataBase.GetItemNum(tradeRate.tradeRates[currentMenuIndex].SellItem);
        //売るアイテムの数が0の場合は処理を抜ける
        if(tradeRate.tradeRates[currentMenuIndex].SellNum > nowInventryItemNum) return;

        //アイテムを売る
        for(int i = 0; i < tradeRate.tradeRates[currentMenuIndex].SellNum; i++)
        {
            itemDataBase.RemoveItem(tradeRate.tradeRates[currentMenuIndex].SellItem);
        }

        //アイテムを買う
        for(int i = 0; i < tradeRate.tradeRates[currentMenuIndex].BuyNum; i++)
        {
            itemDataBase.AddItem(tradeRate.tradeRates[currentMenuIndex].BuyItem);
        }
    }

    //アイテムが売買可能かどうかを判定する
    private bool CanTradeItem()
    {
        var nowInventryItemNum = itemDataBase.GetItemNum(tradeRate.tradeRates[currentMenuIndex].SellItem);
        //売るアイテムの数が足りなければfalseを返す
        if(tradeRate.tradeRates[currentMenuIndex].SellNum > nowInventryItemNum) return false;

        return true;
    }
}