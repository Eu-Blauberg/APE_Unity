using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ControlTradeWindow: MonoBehaviour
{
    //アイテムウィンドウの各要素を格納する変数
    [SerializeField] GameObject CursorAllowText;
    [SerializeField] InputActionAsset inputActionAsset;
    [SerializeField] ItemDataBase itemDataBase;
    [SerializeField] TradeRate tradeRate;

    private Animator CursorAnimator;
    private GameInputs gameInputs;
    private int currentMenuIndex = 0; // 現在選択中のアイテムのインデックス
    private int currentChoicesIndex = 0;   // 現在選択中の選択肢のインデックス

    [SerializeField] private Image[] tradeItemImage; //アイテムの画像(0:売るアイテム, 1:買うアイテム)
    [SerializeField] private Text[] optionText; //選択肢のテキスト(0:戻る, 1:買う)
    [SerializeField] private Text menuIndexText; //現在選択中のアイテムのインデックスを表示するテキスト
    [SerializeField] private Text[] itemNameText; //アイテムの名前を表示するテキスト(0:売るアイテム, 1:買うアイテム)
    [SerializeField] private Text[] itemNumText; //アイテムの数を表示するテキスト(0:売るアイテム, 1:買うアイテム)
    
    /*
    オンオフでアイテムと選択肢の左右操作を切り替えるためのフラグ
    上下入力で切り替える
    trueの場合はcurrentMenuIndexが増減し,アイテムが切り替わる
    falseの場合はcurrentChoicesIndexが増減し,選択肢が切り替わる
    */
    private bool isDown;

    void OnEnable()
    {
        currentMenuIndex = 0;
        currentChoicesIndex = 0;
        UpdateMenuWindow(currentMenuIndex);
        isDown = false;
        CursorAllowText.SetActive(false);

        UpdateMenuIndexText();
        UpdateMenuWindow(currentMenuIndex);

        //戻るボタン選択中の場合のみカーソルを表示する
        CursorAllowText.SetActive(false);
    }

    void Start()
    {
        //カーソルのアニメーターを取得
        CursorAnimator = CursorAllowText.GetComponent<Animator>();

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
    }

    //非アクティブ時に呼び出される
    private void OnDestroy()
    {
        gameInputs?.Dispose();
    }

    private void OnRightPerformed(InputAction.CallbackContext context){
        if(gameObject.activeInHierarchy == false) return; //自身がヒエラルキー上で非アクティブなら処理を抜ける
        if(isDown){
            //選択肢の右移動
            if(currentChoicesIndex == optionText.Length - 1) currentChoicesIndex = 0;
            else currentChoicesIndex++;
            Debug.Log("Right");
            float offset = 100.0f;
            //CUrsorAllowTextの座標を選択肢に合わせる
            CursorAnimator.transform.position = new Vector3(optionText[currentChoicesIndex].transform.position.x - offset , optionText[currentChoicesIndex].transform.position.y, 0);
            UpdateMenuIndexText();
        }
        else
        {
            //アイテムの右移動
            if(currentMenuIndex == tradeRate.tradeRates.Count - 1) currentMenuIndex = 0;
            else currentMenuIndex++;
            Debug.Log("Right");
            if(CanTradeItem() == false)
            {
                //アイテムが売買できなければ購入テキストの透明度を下げる
                optionText[1].color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            }
            else
            {
                //アイテムが売買できれば購入テキストの透明度を上げる
                optionText[1].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            UpdateMenuWindow(currentMenuIndex);
            UpdateMenuIndexText();
        }
    }

    private void OnLeftPerformed(InputAction.CallbackContext context){
        if(gameObject.activeInHierarchy == false) return; //自身がヒエラルキー上で非アクティブなら処理を抜ける
        if(isDown){
            //選択肢の左移動
            if(currentChoicesIndex == 0) currentChoicesIndex = optionText.Length - 1;
            else currentChoicesIndex--;
            Debug.Log("Left");
            float offset = 100.0f;
            //CUrsorAllowTextの座標を選択肢に合わせる
            CursorAnimator.transform.position = new Vector3(optionText[currentChoicesIndex].transform.position.x - offset , optionText[currentChoicesIndex].transform.position.y, 0);
            UpdateMenuIndexText();
        }
        else
        {
            //アイテムの左移動
            if(currentMenuIndex == 0) currentMenuIndex = tradeRate.tradeRates.Count - 1;
            else currentMenuIndex--;
            Debug.Log("Left");
            if(CanTradeItem() == false)
            {
                //アイテムが売買できなければ購入テキストの透明度を下げる
                optionText[1].color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            }
            else
            {
                //アイテムが売買できれば購入テキストの透明度を上げる
                optionText[1].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            UpdateMenuWindow(currentMenuIndex);
            UpdateMenuIndexText();
        }
    }
        
    private void OnDownPerformed(InputAction.CallbackContext context){
        if(gameObject.activeInHierarchy == false) return; //自身がヒエラルキー上で非アクティブなら処理を抜ける
        Debug.Log("Down");

        isDown = true;

        CursorAllowText.SetActive(true);
    }

    private void OnUpPerformed(InputAction.CallbackContext context){
        if(gameObject.activeInHierarchy == false) return; //自身がヒエラルキー上で非アクティブなら処理を抜ける
        Debug.Log("Up");

        isDown = false;
        currentChoicesIndex = 0;

        CursorAllowText.SetActive(false);
    }

    private void OnClickPerformed(InputAction.CallbackContext context){
        if(gameObject.activeInHierarchy == false) return; //自身がヒエラルキー上で非アクティブなら処理を抜ける
        if(!isDown) return; //戻るボタン選択中でない場合は処理を抜ける

        Debug.Log("Click");

        //選択肢の処理
        switch(currentChoicesIndex)
        {
            case 0:
                //親のオブジェクトを取得
                var _gameObject = GameObject.Find("TradeCanvas");
                Destroy(_gameObject);
                break;
            case 1:
                if(CanTradeItem() == false) break;
                else TradeTtem(); //アイテムを買う
                break;
            case 2:
                //戻る
                break;
        }
    }

    //アイテムウィンドウの情報を更新する
    private void UpdateMenuWindow(int index)
    {
        //売るアイテムの情報を表示
        tradeItemImage[0].sprite = tradeRate.tradeRates[index].SellItem.icon;
        itemNameText[0].text = tradeRate.tradeRates[index].SellItem.itemName;
        itemNumText[0].text = tradeRate.tradeRates[index].SellNum.ToString();

        //買うアイテムの情報を表示
        tradeItemImage[1].sprite = tradeRate.tradeRates[index].BuyItem.icon;
        itemNameText[1].text = tradeRate.tradeRates[index].BuyItem.itemName;
        itemNumText[1].text = tradeRate.tradeRates[index].BuyNum.ToString();
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
    private void TradeTtem()
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