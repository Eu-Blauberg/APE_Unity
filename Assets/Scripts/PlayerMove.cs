using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{

    public float speed = 5.0f;
    public InputActionAsset inputActionAsset;
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
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y) * speed * Time.deltaTime;
        transform.Translate(move);
    }
}