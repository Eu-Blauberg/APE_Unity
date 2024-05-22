using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class ControlOptionMenu : MonoBehaviour
{
    [SerializeField] Animator CursorAnimator;
    [SerializeField] GameObject CursorAllowText;
    [SerializeField] GameObject[] MenuTexts;
    [SerializeField] Slider SensiSlider;
    [SerializeField] Slider VolSlider;
    [SerializeField] InputActionAsset inputActionAsset;
    [SerializeField] PlayerData playerData;

    private GameInputs gameInputs;
    private MasterMenu masterMenu;
    private int currentMenuIndex;
    
    void OnEnable()
    {
        currentMenuIndex = 0;

        //CursorAllowTextのY座標をMenuTexts[currentMenuIndex]のY座標に合わせる
        CursorAllowText.transform.position = new Vector3(CursorAllowText.transform.position.x, MenuTexts[currentMenuIndex].transform.position.y, CursorAllowText.transform.position.z);
    }

    void Start()
    {
        /*
        InputActionAssetからアクションマップを取得し、その中からアクションを取得する
        */
        gameInputs = new GameInputs();
        if(gameInputs == null) Debug.Log("GameInputs is null");

        gameInputs.UIControls.Down.performed += OnDownPerformed;
        gameInputs.UIControls.Up.performed += OnUpPerformed;
        gameInputs.UIControls.Click.performed += OnClickPerformed;

        gameInputs.Enable();

        masterMenu = GameObject.Find("MasterMenu").GetComponent<MasterMenu>();

        //スライダーの初期値を設定
        SensiSlider.value = playerData.sensivity;
    }

    //非アクティブ時に呼び出される
    private void OnDestroy()
    {
        gameInputs?.Dispose();
    }

    //方向キーの長押しを検知して各オプションを変更できるようにする
    void Update()
    {
        // Rightボタンが押されているか確認
        if (gameInputs.UIControls.Right.ReadValue<float>() > 0)
        {
            var PressRight = gameInputs.UIControls.Right.GetTimeoutCompletionPercentage();
            if (PressRight >= 1.0f)
            {
                switch(currentMenuIndex)
                {
                    case 0:
                        SensiSlider.value += 5.0f;
                        playerData.sensivity = SensiSlider.value;
                        break;
                    case 1:
                        VolSlider.value += 0.1f;
                        AudioListener.volume = VolSlider.value;
                        break;
                    case 2:
                        break;
                }
            }
        }
        
        // Leftボタンが押されているか確認
        if (gameInputs.UIControls.Left.ReadValue<float>() > 0)
        {
            var PressLeft = gameInputs.UIControls.Left.GetTimeoutCompletionPercentage();
            if (PressLeft >= 1.0f)
            {
                switch(currentMenuIndex)
                {
                    case 0:
                        SensiSlider.value -= 5.0f;
                        playerData.sensivity = SensiSlider.value;
                        break;
                    case 1:
                        VolSlider.value -= 0.1f;
                        AudioListener.volume = VolSlider.value;
                        break;
                    case 2:
                        break;
                }
            }
        }
    }

    private void OnDownPerformed(InputAction.CallbackContext context){
        SoundManager.Instance.PlaySE(SESoundData.SE.Cursor);
        if(gameObject.activeInHierarchy == false) return; //自身がヒエラルキー上で非アクティブなら処理を抜ける
        Debug.Log("Down");

        if(currentMenuIndex == 2) currentMenuIndex = 0;
        else currentMenuIndex++;

        //CursorAllowTextのY座標をMenuTexts[currentMenuIndex]のY座標に合わせる
        CursorAllowText.transform.position = new Vector3(CursorAllowText.transform.position.x, MenuTexts[currentMenuIndex].transform.position.y, CursorAllowText.transform.position.z);
    }

    private void OnUpPerformed(InputAction.CallbackContext context){
        SoundManager.Instance.PlaySE(SESoundData.SE.Cursor);
        if(gameObject.activeInHierarchy == false) return; //自身がヒエラルキー上で非アクティブなら処理を抜ける
        Debug.Log("Up");

        if(currentMenuIndex == 0) currentMenuIndex = 2;
        else currentMenuIndex--;

        //CursorAllowTextのY座標をMenuTexts[currentMenuIndex]のY座標に合わせる
        CursorAllowText.transform.position = new Vector3(CursorAllowText.transform.position.x, MenuTexts[currentMenuIndex].transform.position.y, CursorAllowText.transform.position.z);
    }

    private void OnClickPerformed(InputAction.CallbackContext context){
        SoundManager.Instance.PlaySE(SESoundData.SE.Click);
        if(gameObject.activeInHierarchy == false) return; //自身がヒエラルキー上で非アクティブなら処理を抜ける
        Debug.Log("Click");
        switch (currentMenuIndex)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                CursorAnimator.SetTrigger("OnClicked");
                masterMenu.CloseOptionMenu();
                break;
        }
    }
}

