using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OpenMenu : MonoBehaviour
{
    public InputActionAsset inputActionAsset; // Input Actionsアセットへの参照
    public GameObject menuCanvas; // メニューキャンバス
    private InputAction menuAction; // 視点移動用のアクション
    private GameObject _gameObject; // メニューキャンバスのオブジェクト
    void Awake()
    {
        var actionMap = inputActionAsset.FindActionMap("PlayerControls");
        menuAction = actionMap.FindAction("Menu");
        menuAction.Enable();
        menuAction.performed += OnMenuPerformed;

        _gameObject = GameObject.Find("MenuCanvas");
    }

    private void OnMenuPerformed(InputAction.CallbackContext context)
    {
        if (_gameObject != null)
        {
            if(_gameObject.activeInHierarchy == false)
            {
                _gameObject.SetActive(true);
                Debug.Log("Active" + _gameObject.name);
                Time.timeScale = 1;
            }
            else
            {
                _gameObject.SetActive(false);
                Debug.Log("Close " + _gameObject.name);
                Time.timeScale = 0;
            }
        }
        else
        {
            // メニューを開く
            GameObject instantedMenuCanvas = Instantiate(menuCanvas);
            instantedMenuCanvas.name = "MenuCanvas";
            _gameObject = instantedMenuCanvas;
            Debug.Log("Instante " + instantedMenuCanvas.name);
            Time.timeScale = 0;
        }
        
        //Time.timeScale = 0;
    }
}

