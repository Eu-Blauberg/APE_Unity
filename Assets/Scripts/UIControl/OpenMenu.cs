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

    private GameInputs gameInputs;


    void Awake()
    {
        gameInputs = new GameInputs();
        if(gameInputs == null) Debug.Log("GameInputs is null");

        gameInputs.PlayerControls.Menu.performed += OnMenuPerformed;

        gameInputs.Enable();

        _gameObject = GameObject.Find("MenuCanvas");
    }

    private void OnDestroy()
    {
        gameInputs?.Dispose();
    }

    private void OnMenuPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("OnMenuPerformed");
        if (_gameObject != null)
        {
            if(_gameObject.activeInHierarchy == false)
            {
                _gameObject.SetActive(true);
                Debug.Log("Active" + _gameObject.name);
                Time.timeScale = 0;
            }
            else
            {
                _gameObject.SetActive(false);
                Debug.Log("Close " + _gameObject.name);
                Time.timeScale = 1;
            }
        }
        else
        {
            // メニューを開く
            GameObject instantedMenuCanvas = Instantiate(menuCanvas);
            instantedMenuCanvas.name = "MenuCanvas";
            Debug.Log("メニューインスタンスを生成");
            _gameObject = instantedMenuCanvas;

            SoundManager.Instance.PlaySE(SESoundData.SE.Menu);
            Time.timeScale = 0;
        }
        
        //Time.timeScale = 0;
    }
}

