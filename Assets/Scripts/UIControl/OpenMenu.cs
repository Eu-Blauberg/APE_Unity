using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OpenMenu : MonoBehaviour
{
    public InputActionAsset inputActionAsset; // Input Actionsアセットへの参照
    public GameObject menuCanvas; // メニューキャンバス
    private InputAction menuAction; // 視点移動用のアクション

    void Awake()
    {
        var actionMap = inputActionAsset.FindActionMap("PlayerControls");
        menuAction = actionMap.FindAction("Menu");
        menuAction.Enable();
        menuAction.performed += OnMenuPerformed;
    }

    private void OnMenuPerformed(InputAction.CallbackContext context)
    {
        if (GameObject.Find("MenuCanvas(Clone)") != null)
        {
            // メニューを閉じる
            Destroy(GameObject.Find("MenuCanvas(Clone)"));
            Time.timeScale = 1;
            return;
        }else
        {
            // メニューを開く
            GameObject instantedMenuCanvas = Instantiate(menuCanvas);
            Time.timeScale = 0;
        }
        
        //Time.timeScale = 0;
    }
}

