using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ControlItemWindow : MonoBehaviour
{
    [System.Serializable] public class AnimatorManager{
        public Animator OnCursorAnimator;
        public Animator OnClickAnimator;
        public string boolName;
    }

    //インスペクターからアニメーターを管理するリストを作成
    [SerializeField] List<AnimatorManager> animatorManagers;

    //アイテムウィンドウの各要素を格納する変数
    [SerializeField] UnityEngine.UI.Image itemImage;
    [SerializeField] UnityEngine.UI.Text itemName;
    [SerializeField] UnityEngine.UI.Text itemDescription;

    private InputActionAsset inputActionAsset;
    private InputAction UpAction;
    private InputAction DownAction;
    private InputAction RightAction;
    private InputAction LeftAction;
    private InputAction ClickAction;

    //プレイヤーデータを格納する変数
    private PlayerData playerData;
    private Inventry inv = new Inventry();

    private int currentItemIndex = 0;
    private bool isDown;//戻るボタン選択中かどうかのフラグ

    void OnEnable()
    {
        currentItemIndex = 0;
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

        //右矢印のアニメーションを再生
        StartCoroutine(OnClickController(animatorManagers[0]));

        //アイテムリストの最後の要素に到達したら最初の要素に戻る
        if(currentItemIndex == inv.inventry.Count - 1) currentItemIndex = 0;
        else currentItemIndex++;
    }

    private void OnLeftPerformed(InputAction.CallbackContext context){
        if(isDown) return; //戻るボタン選択中なら処理を抜ける
        Debug.Log("Left");

        //左矢印のアニメーションを再生
        StartCoroutine(OnClickController(animatorManagers[1]));

        //アイテムリストの最初の要素に到達したら最後の要素に戻る
        if(currentItemIndex == 0) currentItemIndex = inv.inventry.Count - 1;
        else currentItemIndex--;  
    }
        
    private void OnDownPerformed(InputAction.CallbackContext context){
        Debug.Log("Down");

        isDown = true;

        animatorManagers[2].OnCursorAnimator.SetBool("OnCursor", true);
    }

    private void OnUpPerformed(InputAction.CallbackContext context){
        Debug.Log("Up");

        animatorManagers[2].OnCursorAnimator.SetBool("OnCursor", false);
        animatorManagers[2].OnCursorAnimator.SetTrigger("Reset");

        isDown = false;
    }

    private void OnClickPerformed(InputAction.CallbackContext context){
        if(!isDown) return; //戻るボタン選択中なら処理を抜ける

        Debug.Log("Click");

        animatorManagers[2].OnCursorAnimator.SetTrigger("Reset");
        animatorManagers[2].OnClickAnimator.SetTrigger("OnClick");

        Destroy(transform.parent.gameObject);
    }

    private IEnumerator OnClickController(AnimatorManager animatorManagers){
        animatorManagers.OnClickAnimator.SetTrigger("OnClick");
        yield return new WaitForSecondsRealtime(0.3f);
        animatorManagers.OnClickAnimator.SetTrigger("Reset");
    }
}
