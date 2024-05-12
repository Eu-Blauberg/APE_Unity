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

    [SerializeField] Animator CursorAnimator;
    [SerializeField] GameObject CursorAllowText;
    [SerializeField] InputActionAsset inputActionAsset;
    [SerializeField] ItemDataBase itemDataBase;

    private InputAction UpAction;
    private InputAction DownAction;
    private InputAction RightAction;
    private InputAction LeftAction;
    private InputAction ClickAction;

    private int currentItemIndex = 0;
    private int InventoryNum = 0;//インベントリのアイテム数
    private bool isDown;//戻るボタン選択中かどうかのフラグ

    //インスタンス化されるたびに呼び出される
    void OnEnable()
    {
        foreach (var item in itemDataBase.items)
        {
            if (item.num > 0) InventoryNum ++;
        }

        currentItemIndex = 0;

        UpdateItemWindow(currentItemIndex);
    }

    void Start()
    {
        var actionMap = inputActionAsset.FindActionMap("UIControls");
        UpAction = actionMap.FindAction("Up");
        DownAction = actionMap.FindAction("Down");
        RightAction = actionMap.FindAction("Right");
        LeftAction = actionMap.FindAction("Left");
        ClickAction = actionMap.FindAction("Click");

        UpAction.Enable();
        DownAction.Enable();
        RightAction.Enable();
        LeftAction.Enable();
        ClickAction.Enable();

        UpAction.performed += OnUpPerformed;
        DownAction.performed += OnDownPerformed;
        RightAction.performed += OnRightPerformed;
        LeftAction.performed += OnLeftPerformed;
        ClickAction.performed += OnClickPerformed;

        CursorAllowText.SetActive(false);
    }   

    void OnDisable()
    {
        UpAction.Disable();
        DownAction.Disable();
        RightAction.Disable();
        LeftAction.Disable();
        ClickAction.Disable();
    }


    private void OnRightPerformed(InputAction.CallbackContext context){
        if(isDown) return; //戻るボタン選択中なら処理を抜ける
        Debug.Log("Right");

        if(currentItemIndex == itemDataBase.items.Count - 1) currentItemIndex = 0;
        else currentItemIndex++;

        UpdateItemWindow(currentItemIndex);

    }

    private void OnLeftPerformed(InputAction.CallbackContext context){
        if(isDown) return; //戻るボタン選択中なら処理を抜ける
        Debug.Log("Left");

        if(currentItemIndex == 0) currentItemIndex = itemDataBase.items.Count - 1;
        else currentItemIndex--;

        UpdateItemWindow(currentItemIndex);
    }
        
    private void OnDownPerformed(InputAction.CallbackContext context){
        Debug.Log("Down");

        isDown = true;

        CursorAllowText.SetActive(true);
    }

    private void OnUpPerformed(InputAction.CallbackContext context){
        Debug.Log("Up");

        isDown = false;

        CursorAllowText.SetActive(false);
    }

    private void OnClickPerformed(InputAction.CallbackContext context){
        if(!isDown) return; //戻るボタン選択中でない場合は処理を抜ける

        Debug.Log("Click");

        GameObject.Find("MenuMaster").GetComponent<MasterMenu>().StartCoroutine("CloseItemMenu");
    }

    private void UpdateItemWindow(int index){
        if(InventoryNum == 0){
            itemImage.sprite = null;
            itemName.text = "アイテムがありません";
            itemDescription.text = "";
        }else{
            itemImage.sprite = itemDataBase.items[index].icon;
            itemName.text = itemDataBase.items[index].itemName;
            itemDescription.text = itemDataBase.items[index].description;
        }
    }

}
