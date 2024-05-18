using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public InputActionAsset inputActionAsset;
    public PlayerData playerData;
    private InputAction moveAction;
    private Vector2 moveInput;


    // Start is called before the first frame update
    void Start()
    {
        // InputActionから「PlayerControls」マップと「Move」アクションを取得
        var actionMap = inputActionAsset.FindActionMap("PlayerControls");
        moveAction = actionMap.FindAction("Move");

        // アクションの有効化
        moveAction.Enable();

        // アクションにコールバックを設定
        moveAction.performed += OnMovePerformed;
        moveAction.canceled += OnMoveCanceled;
        Application.targetFrameRate = 60; // ← FPS を 60 に設定
    }

    private void OnMovePerformed(InputAction.CallbackContext context){
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context){
        moveInput = Vector2.zero;
    }

    void FixedUpdate(){
        if(Time.timeScale == 0) return;
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y) * playerData.speed * Time.deltaTime;
        transform.Translate(move);

        playerData._position = transform.position;

        /*
        キーボードの入力によってプレイヤーを移動させる処理
        */
        if (Input.GetKey(KeyCode.Space))
        {
            //上矢印キーが押された場合
            if (Input.GetKey(KeyCode.UpArrow))
            {
                //プレイヤーを前進させる
                transform.position += transform.forward * playerData.speed * Time.deltaTime;
            }

            //下矢印キーが押された場合
            if (Input.GetKey(KeyCode.DownArrow))
            {
                //プレイヤーを後退させる
                transform.position -= transform.forward * playerData.speed * Time.deltaTime;
            }

            //左矢印キーが押された場合
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                //プレイヤーを左に移動させる
                transform.position -= transform.right * playerData.speed * Time.deltaTime;
            }

            //右矢印キーが押された場合
            if (Input.GetKey(KeyCode.RightArrow))
            {
                //プレイヤーを右に移動させる
                transform.position += transform.right * playerData.speed * Time.deltaTime;
            }
        }
    }

}