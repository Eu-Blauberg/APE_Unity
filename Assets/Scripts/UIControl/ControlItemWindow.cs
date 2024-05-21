using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ControlItemWindow : MonoBehaviour
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
    private MasterMenu masterMenu;

    private int currentItemIndex = 0;
    private int InventoryNum = 0;//インベントリのアイテム数
    private bool isDown;//戻るボタン選択中かどうかのフラグ

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
        foreach (var item in itemDataBase.items)
        {
            if (item.num > 0) InventoryNum ++;
        }

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

        masterMenu = GameObject.Find("MasterMenu").GetComponent<MasterMenu>();

        //戻るボタン選択中の場合のみカーソルを表示する
        CursorAllowText.SetActive(false);
    }

    //非アクティブ時に呼び出される
    private void OnDestroy()
    {
        gameInputs?.Dispose();
    }

    private void OnRightPerformed(InputAction.CallbackContext context){
        SoundManager.Instance.PlaySE(SESoundData.SE.Cursor);
        if(gameObject.activeInHierarchy == false) return; //自身がヒエラルキー上で非アクティブなら処理を抜ける
        if(isDown) return; //戻るボタン選択中なら処理を抜ける
        Debug.Log("Right");

        if(currentItemIndex == itemDataBase.items.Count - 1) currentItemIndex = 0;
        else currentItemIndex++;

        if(itemDataBase.items[currentItemIndex].num == 0)
        {
            currentItemIndex = 0;
        }

        UpdateItemWindow(currentItemIndex);
        UpdateItemIndexText();
    }

    private void OnLeftPerformed(InputAction.CallbackContext context){
        SoundManager.Instance.PlaySE(SESoundData.SE.Cursor);
        if(gameObject.activeInHierarchy == false) return; //自身がヒエラルキー上で非アクティブなら処理を抜ける
        if(isDown) return; //戻るボタン選択中なら処理を抜ける
        Debug.Log("Left");

        if(currentItemIndex == 0) currentItemIndex = itemDataBase.items.Count - 1;
        else currentItemIndex--;

        while(itemDataBase.items[currentItemIndex].num == 0)
        {
            if(currentItemIndex <= 0) break;
            else currentItemIndex--;
        }
        UpdateItemWindow(currentItemIndex);
        UpdateItemIndexText();
    }
        
    private void OnDownPerformed(InputAction.CallbackContext context){
        SoundManager.Instance.PlaySE(SESoundData.SE.Cursor);
        if(gameObject.activeInHierarchy == false) return; //自身がヒエラルキー上で非アクティブなら処理を抜ける
        Debug.Log("Down");

        isDown = true;

        CursorAllowText.SetActive(true);
    }

    private void OnUpPerformed(InputAction.CallbackContext context){
        SoundManager.Instance.PlaySE(SESoundData.SE.Cursor);
        if(gameObject.activeInHierarchy == false) return; //自身がヒエラルキー上で非アクティブなら処理を抜ける
        Debug.Log("Up");

        isDown = false;

        CursorAllowText.SetActive(false);
    }

    private void OnClickPerformed(InputAction.CallbackContext context){
        SoundManager.Instance.PlaySE(SESoundData.SE.Click);
        if(gameObject.activeInHierarchy == false) return; //自身がヒエラルキー上で非アクティブなら処理を抜ける
        if(!isDown) return; //戻るボタン選択中でない場合は処理を抜ける

        Debug.Log("Click");

        MasterMenu masterMenu = GameObject.Find("MasterMenu").GetComponent<MasterMenu>();
        masterMenu.CloseItemMenu();
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
            if(itemDataBase.items[i].num > 0)
            {
                if(i == currentItemIndex) itemIndexText.text += "＊";
                else itemIndexText.text += "・";
            }
        }
    }

}
