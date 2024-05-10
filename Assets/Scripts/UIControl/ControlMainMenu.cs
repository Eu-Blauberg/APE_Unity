using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlMainMenu : MonoBehaviour
{
    //アニメーターを自作クラスで管理する
    [System.Serializable] public class AnimatorManager{
        public Animator OnCursorAnimator;
        public Animator OnClickAnimator;
        public string boolName;
    }
    //インスペクターからアニメーターを管理するリストを作成
    [SerializeField] List<AnimatorManager> animatorManagers;

    public InputActionAsset inputActionAsset;
    private InputAction UpAction;
    private InputAction DownAction;
    private InputAction ClickAction;

    private int currentMenuIndex;
    private int menuIndexNum = 3;

    void OnEnable()
    {
        currentMenuIndex = 0;

        animatorManagers[currentMenuIndex].OnCursorAnimator.SetBool("OnCursor", true);
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

        if(currentMenuIndex == (menuIndexNum - 1)) currentMenuIndex = 0;
        else currentMenuIndex++;

        for (int i = 0; i < menuIndexNum; i++)
        {
            if(i == currentMenuIndex){
                animatorManagers[i].OnCursorAnimator.SetBool("OnCursor", true);
            }else{
                animatorManagers[i].OnCursorAnimator.SetBool("OnCursor", false);
                animatorManagers[i].OnCursorAnimator.SetTrigger("Reset");
            }
        }
    }

    private void OnUpPerformed(InputAction.CallbackContext context){
        if(gameObject.activeInHierarchy == false) return; //自身がヒエラルキー上で非アクティブなら処理を抜ける
        Debug.Log("Up");
        if(currentMenuIndex == 0) currentMenuIndex = (menuIndexNum - 1);
        else currentMenuIndex--;

        for (int i = 0; i < menuIndexNum; i++)
        {
            if(i == currentMenuIndex){
                animatorManagers[i].OnCursorAnimator.SetBool("OnCursor", true);
            }else{
                animatorManagers[i].OnCursorAnimator.SetBool("OnCursor", false);
                animatorManagers[i].OnCursorAnimator.SetTrigger("Reset");
            }
        }
    }

    private void OnClickPerformed(InputAction.CallbackContext context){
        if(gameObject.activeInHierarchy == false) return; //自身がヒエラルキー上で非アクティブなら処理を抜ける
        Debug.Log("Click");
        switch (currentMenuIndex)
        {
            case 0:
                animatorManagers[currentMenuIndex].OnClickAnimator.SetTrigger("OnClick");
                GameObject.Find("MenuMaster").GetComponent<MasterMenu>().CloseMainMenu();
                break;
            case 1:
                animatorManagers[currentMenuIndex].OnClickAnimator.SetTrigger("OnClick");
                GameObject.Find("MenuMaster").GetComponent<MasterMenu>().OpenOptionMenu();
                break;
            case 2:
                animatorManagers[currentMenuIndex].OnClickAnimator.SetTrigger("OnClick");
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;//エディタ上のゲームプレイ終了
                #else
                    Application.Quit();//ビルドしたアプリ上のゲームプレイ終了
                #endif
                break;
        }
    }
}

