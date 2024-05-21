using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ControlMiniMap : MonoBehaviour
{
    [SerializeField] private Camera miniMapCamera;
    [SerializeField] ItemDataBase itemDataBase;
    [SerializeField] InputActionAsset inputActionAsset;
    [SerializeField] private Text noticeText;

    private GameInputs gameInputs;
    private ControlGameDisplay controlGameDisplay;

    void Start()
    {
        controlGameDisplay = GameObject.FindWithTag("GameDisplayMaster").GetComponent<ControlGameDisplay>();

        gameInputs = new GameInputs();
        if(gameInputs == null) Debug.Log("GameInputs is null");

        gameInputs.PlayerControls.Item.performed += OnItemPerformed;

        gameInputs.Enable();
    }

    void OnItemPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if(Time.timeScale == 0) return;

        if(itemDataBase.GetItemNum(itemDataBase.GetItemByName("千里眼の薬")) > 0)
        {
            itemDataBase.RemoveItem(itemDataBase.GetItemByName("千里眼の薬"));
            controlGameDisplay.UpdateInventoryUI();
            StartCoroutine(DisplayNoticeText("千里眼の薬を使った"));
            //コルーチンを開始
            StartCoroutine(ExpandFieldOfView());
        }
    }

    //カメラの有効視野を広げる
    private IEnumerator ExpandFieldOfView()
    {
        //カメラの有効視野を広げる
        miniMapCamera.fieldOfView = 90;

        //1秒待つ
        yield return new WaitForSeconds(3.0f);

        //カメラの有効視野を元に戻す
        miniMapCamera.fieldOfView = 60;
    }

    //アイテムを使った時のテキスト表示
    public IEnumerator DisplayNoticeText(string message)
    {
        noticeText.text = message;
        yield return new WaitForSeconds(3.0f);
        noticeText.text = "";
    }
}
