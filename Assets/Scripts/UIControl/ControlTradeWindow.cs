using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ControlTradeWindo: MonoBehaviour
{
    //アイテムウィンドウの各要素を格納する変数
    [SerializeField] UnityEngine.UI.Image itemImage;
    [SerializeField] UnityEngine.UI.Text itemName;
    [SerializeField] UnityEngine.UI.Text itemDescription;
    [SerializeField] UnityEngine.UI.Text itemIndexText;

    [SerializeField] Animator CursorAnimator;
    [SerializeField] GameObject CursorAllowText;
    [SerializeField] InputActionAsset inputActionAsset;
    [SerializeField] ItemDataBase itemDataBase;

    private GameInputs gameInputs;

    private int currentItemIndex = 0; // 現在選択中のアイテムのインデックス
    private int currentChoicesIndex = 0;   // 現在選択中の選択肢のインデックス
    
    /*
    オンオフでアイテムと選択肢の左右操作を切り替えるためのフラグ
    上下入力で切り替える
    trueの場合はcurrentItemIndexが増減し,アイテムが切り替わる
    falseの場合はcurrentChoicesIndexが増減し,選択肢が切り替わる
    */
    private bool isDown;

    void OnEnable()
    {
        currentItemIndex = 0;
        UpdateItemWindow(currentItemIndex);
        isDown = false;
        CursorAllowText.SetActive(false);

        UpdateItemIndexText();
    }

    void Start()
    {

        currentItemIndex = 0;

        UpdateItemWindow(currentItemIndex);

        gameInputs = new GameInputs();
        if(gameInputs == null) Debug.Log("GameInputs is null");

        gameInputs.UIControls.Up.performed += OnUpPerformed;
        gameInputs.UIControls.Down.performed += OnDownPerformed;
        gameInputs.UIControls.Right.performed += OnRightPerformed;
        gameInputs.UIControls.Left.performed += OnLeftPerformed;
        gameInputs.UIControls.Click.performed += OnClickPerformed;

        gameInputs.Enable();


        //戻るボタン選択中の場合のみカーソルを表示する
        CursorAllowText.SetActive(false);
    }

    //非アクティブ時に呼び出される
    private void OnDestroy()
    {
        gameInputs?.Dispose();
    }

    private void OnRightPerformed(InputAction.CallbackContext context){
        if(gameObject.activeInHierarchy == false) return; //自身がヒエラルキー上で非アクティブなら処理を抜ける
        if(isDown) return; //戻るボタン選択中なら処理を抜ける
        Debug.Log("Right");

        if(currentItemIndex == itemDataBase.items.Count - 1) currentItemIndex = 0;
        else currentItemIndex++;

        UpdateItemWindow(currentItemIndex);
        UpdateItemIndexText();
    }

    private void OnLeftPerformed(InputAction.CallbackContext context){
        if(gameObject.activeInHierarchy == false) return; //自身がヒエラルキー上で非アクティブなら処理を抜ける
        if(isDown) return; //戻るボタン選択中なら処理を抜ける
        Debug.Log("Left");

        if(currentItemIndex == 0) currentItemIndex = itemDataBase.items.Count - 1;
        else currentItemIndex--;

        UpdateItemWindow(currentItemIndex);
        UpdateItemIndexText();
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

        CursorAllowText.SetActive(false);
    }

    private void OnClickPerformed(InputAction.CallbackContext context){
        if(gameObject.activeInHierarchy == false) return; //自身がヒエラルキー上で非アクティブなら処理を抜ける
        if(!isDown) return; //戻るボタン選択中でない場合は処理を抜ける

        Debug.Log("Click");

        //親オブジェクトを消去する
        Destroy(this.transform.parent.gameObject);
    }

    //アイテムウィンドウの情報を更新する
    private void UpdateItemWindow(int index)
    {
        //アイテムウィンドウの各要素を更新
        itemImage.sprite = itemDataBase.items[index].icon;
        itemName.text = itemDataBase.items[index].itemName;
        itemDescription.text = itemDataBase.items[index].description;
    }

    private void UpdateItemIndexText()
    {
        itemIndexText.text = "";
        for(int i = 0; i < itemDataBase.items.Count; i++)
        {
            if(i == currentItemIndex) itemIndexText.text += "＊";
            else itemIndexText.text += "・";
        }
    }

}
