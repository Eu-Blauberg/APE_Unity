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
    
    private GameInputs gameInputs;
    private MasterMenu masterMenu;

    private int currentMenuIndex;

    //アクティブ時に呼び出される
    void OnEnable()
    {
        //初期状態でカーソルが最初のメニューに合わせる
        currentMenuIndex = 0;

        //CursorAllowTextのY座標をMenuTexts[currentMenuIndex]のY座標に合わせる
        CursorAllowText.transform.position = new Vector3(CursorAllowText.transform.position.x, MenuTexts[currentMenuIndex].transform.position.y, CursorAllowText.transform.position.z);
    }

    void Start()
    {
        gameInputs = new GameInputs();
        if(gameInputs == null) Debug.Log("GameInputs is null");

        gameInputs.UIControls.Down.performed += OnDownPerformed;
        gameInputs.UIControls.Up.performed += OnUpPerformed;
        gameInputs.UIControls.Click.performed += OnClickPerformed;

        gameInputs.Enable();

        masterMenu = GameObject.Find("MasterMenu").GetComponent<MasterMenu>();
    }

    //非アクティブ時に呼び出される
    private void OnDestroy()
    {
        gameInputs?.Dispose();
    }


    private void OnDownPerformed(InputAction.CallbackContext context){
        if(gameObject.activeInHierarchy == false) return; //自身がヒエラルキー上で非アクティブなら処理を抜ける
        Debug.Log("Down");

        if(currentMenuIndex == MenuTexts.Length - 1) currentMenuIndex = 0;
        else currentMenuIndex++;
        
        //CursorAllowTextのY座標をMenuTexts[currentMenuIndex]のY座標に合わせる
        CursorAllowText.transform.position = new Vector3(CursorAllowText.transform.position.x, MenuTexts[currentMenuIndex].transform.position.y, CursorAllowText.transform.position.z);
    }

    private void OnUpPerformed(InputAction.CallbackContext context){
        if(gameObject.activeInHierarchy == false) return; //自身がヒエラルキー上で非アクティブなら処理を抜ける
        Debug.Log("Up");
        
        if(currentMenuIndex == 0) currentMenuIndex = MenuTexts.Length - 1;
        else currentMenuIndex--;

        //CursorAllowTextのY座標をMenuTexts[currentMenuIndex]のY座標に合わせる
        CursorAllowText.transform.position = new Vector3(CursorAllowText.transform.position.x, MenuTexts[currentMenuIndex].transform.position.y, CursorAllowText.transform.position.z);
    }

    private void OnClickPerformed(InputAction.CallbackContext context){
        if(gameObject.activeInHierarchy == false) return; //自身がヒエラルキー上で非アクティブなら処理を抜ける
        Debug.Log("Click");

        switch (currentMenuIndex)
        {
            case 0:
                CursorAnimator.SetTrigger("OnClicked");
                masterMenu.CloseMainMenu();
                break;
            case 1:
                CursorAnimator.SetTrigger("OnClicked");
                masterMenu.OpenOptionMenu();
                break;
            case 2:
                CursorAnimator.SetTrigger("OnClicked");
                masterMenu.OpenItemMenu();
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

