using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlMainMenu : MonoBehaviour
{
    [SerializeField] Animator CursorAnimator;
    [SerializeField] InputActionAsset inputActionAsset;
    [SerializeField] GameObject CursorAllowText;
    [SerializeField] GameObject[] MenuTexts;

    private InputAction UpAction;
    private InputAction DownAction;
    private InputAction ClickAction;

    private int currentMenuIndex;

    void OnEnable()
    {
        //初期状態でカーソルが最初のメニューに合わせる
        currentMenuIndex = 0;

        //CursorAllowTextのY座標をMenuTexts[currentMenuIndex]のY座標に合わせる
        CursorAllowText.transform.position = new Vector3(CursorAllowText.transform.position.x, MenuTexts[currentMenuIndex].transform.position.y, CursorAllowText.transform.position.z);
    }

    void Start()
    {
        var actionMap = inputActionAsset.FindActionMap("UIControls");
        UpAction = actionMap.FindAction("Up");
        DownAction = actionMap.FindAction("Down");
        ClickAction = actionMap.FindAction("Click");

        UpAction.Enable();
        DownAction.Enable();
        ClickAction.Enable();

        UpAction.performed += OnUpPerformed;
        DownAction.performed += OnDownPerformed;
        ClickAction.performed += OnClickPerformed;
    }

    void OnDisable()
    {
        UpAction.Disable();
        DownAction.Disable();
        ClickAction.Disable();
    }


    private void OnDownPerformed(InputAction.CallbackContext context){
        if(gameObject.activeInHierarchy == false) return; //自身がヒエラルキー上で非アクティブなら処理を抜ける
        Debug.Log("Down");

        //Menumasterが見つからない場合は処理を抜ける
        if(GameObject.Find("MenuMaster") == null) return;

        if(currentMenuIndex == MenuTexts.Length - 1) currentMenuIndex = 0;
        else currentMenuIndex++;
        
        //CursorAllowTextのY座標をMenuTexts[currentMenuIndex]のY座標に合わせる
        CursorAllowText.transform.position = new Vector3(CursorAllowText.transform.position.x, MenuTexts[currentMenuIndex].transform.position.y, CursorAllowText.transform.position.z);
    }

    private void OnUpPerformed(InputAction.CallbackContext context){
        if(gameObject.activeInHierarchy == false) return; //自身がヒエラルキー上で非アクティブなら処理を抜ける
        Debug.Log("Up");

        //Menumasterが見つからない場合は処理を抜ける
        if(GameObject.Find("MenuMaster") == null) return;
        
        if(currentMenuIndex == 0) currentMenuIndex = MenuTexts.Length - 1;
        else currentMenuIndex--;

        //CursorAllowTextのY座標をMenuTexts[currentMenuIndex]のY座標に合わせる
        CursorAllowText.transform.position = new Vector3(CursorAllowText.transform.position.x, MenuTexts[currentMenuIndex].transform.position.y, CursorAllowText.transform.position.z);
    }

    private void OnClickPerformed(InputAction.CallbackContext context){
        if(gameObject.activeInHierarchy == false) return; //自身がヒエラルキー上で非アクティブなら処理を抜ける
        Debug.Log("Click");
        GameObject menuMaster = GameObject.Find("MenuMaster");
        if (menuMaster == null) {
            Debug.LogError("MenuMasterが見つかりません。");
            return;
        }
        MasterMenu masterMenu = menuMaster.GetComponent<MasterMenu>();
        if (masterMenu == null) {
            Debug.LogError("MasterMenuコンポーネントがMenuMasterにアタッチされていません。");
            return;
        }

        switch (currentMenuIndex)
        {
            case 0:
                CursorAnimator.SetTrigger("OnClicked");
                masterMenu.CloseMainMenu();
                break;
            case 1:
                CursorAnimator.SetTrigger("OnClicked");
                masterMenu.StartCoroutine("OpenOptionMenu");
                break;
            case 2:
                CursorAnimator.SetTrigger("OnClicked");
                masterMenu.StartCoroutine("OpenItemMenu");
                break;
            case 3:
                CursorAnimator.SetTrigger("OnClicked");
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;//エディタ上のゲームプレイ終了
                #else
                    Application.Quit();//ビルドしたアプリ上のゲームプレイ終了
                #endif
                break;
        }
    }
}

