using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControlOptionMenu : MonoBehaviour
{
    //アニメーターを自作クラスで管理する
    [System.Serializable] public class AnimatorManager{
        public Animator OnCursorAnimator;
        public Animator OnClickAnimator;
        public string boolName;
    }

    //インスペクターからアニメーターを管理するリストを作成
    [SerializeField] List<AnimatorManager> animatorManagers;
    [SerializeField] Slider SensiSlider;
    [SerializeField] Slider VolSlider;

    public InputActionAsset inputActionAsset;
    private InputAction UpAction;
    private InputAction DownAction;
    private InputAction ClickAction;
    private InputAction RightAction;
    private InputAction LeftAction;

    private int currentMenuIndex;

    
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
        RightAction = actionMap.FindAction("Right");
        LeftAction = actionMap.FindAction("Left");

        UpAction.Enable();
        DownAction.Enable();
        ClickAction.Enable();
        RightAction.Enable();
        LeftAction.Enable();

        UpAction.performed += OnUpPerformed;
        DownAction.performed += OnDownPerformed;
        ClickAction.performed += OnClickPerformed;

        SensiSlider.value = GameObject.Find("Character").GetComponent<PlayerLook>().GetSensitivity();
    }

    void OnDisable()
    {
        UpAction.Disable();
        DownAction.Disable();
        ClickAction.Disable();
        RightAction.Disable();
        LeftAction.Disable();
    }

    //方向キーの長押しを検知して各オプションを変更できるようにする
    void Update()
    {
        // Rightボタンが押されているか確認
        if (RightAction.ReadValue<float>() > 0)
        {
            var PressRight = RightAction.GetTimeoutCompletionPercentage();
            if (PressRight >= 1.0f)
            {
                switch(currentMenuIndex)
                {
                    case 0:
                        SensiSlider.value += 5.0f;
                        GameObject.Find("Character").GetComponent<PlayerLook>().SetSensitivity(SensiSlider.value);
                        break;
                    case 1:
                        VolSlider.value += 5.0f;
                        //音量の設定を追加
                        break;
                    case 2:
                        break;
                }
            }
        }

        // Leftボタンが押されているか確認
        if (LeftAction.ReadValue<float>() > 0)
        {
            var PressLeft = LeftAction.GetTimeoutCompletionPercentage();
            if (PressLeft >= 1.0f)
            {
                switch(currentMenuIndex)
                {
                    case 0:
                        SensiSlider.value -= 5.0f;
                        GameObject.Find("Character").GetComponent<PlayerLook>().SetSensitivity(SensiSlider.value);
                        break;
                    case 1:
                        VolSlider.value -= 5.0f;
                        //音量の設定を追加
                        break;
                    case 2:
                        break;
                }
            }
        }
    }

    private void OnDownPerformed(InputAction.CallbackContext context){
        if(gameObject.activeInHierarchy == false) return; //自身がヒエラルキー上で非アクティブなら処理を抜ける
        Debug.Log("Down");

        if(currentMenuIndex == 2) currentMenuIndex = 0;
        else currentMenuIndex++;

        for (int i = 0; i < 3; i++)
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

        if(currentMenuIndex == 0) currentMenuIndex = 2;
        else currentMenuIndex--;

        for (int i = 0; i < 3; i++)
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
                break;
            case 1:
                break;
            case 2:
                animatorManagers[currentMenuIndex].OnClickAnimator.SetTrigger("OnClick");
                GameObject.Find("MenuMaster").GetComponent<MasterMenu>().StartCoroutine("CloseOptionMenu");
                break;
        }
    }
}

