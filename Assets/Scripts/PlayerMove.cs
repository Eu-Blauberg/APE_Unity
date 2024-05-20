using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public InputActionAsset inputActionAsset;
    public PlayerData playerData;
    private InputAction moveAction;
    private InputAction dashAction;
    private Vector2 moveInput;

    private ControlAudio controlAudio;

    //ダッシュ時のスピード倍率
    private float dashSpeedRate = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        controlAudio = GameObject.FindWithTag("AudioMaster").GetComponent<ControlAudio>();

        controlAudio.SetCharacterObject(gameObject);

        // InputActionから「PlayerControls」マップと「Move」アクションを取得
        var actionMap = inputActionAsset.FindActionMap("PlayerControls");
        moveAction = actionMap.FindAction("Move");
        dashAction = actionMap.FindAction("Dash");

        // アクションの有効化
        moveAction.Enable();
        dashAction.Enable();

        // アクションにコールバックを設定
        moveAction.performed += OnMovePerformed;
        moveAction.canceled += OnMoveCanceled;
        dashAction.performed += OnDashPerformed;
        dashAction.canceled += OnDashCanceled;
        Application.targetFrameRate = 60; // ← FPS を 60 に設定
    }

    private void OnMovePerformed(InputAction.CallbackContext context){
        moveInput = context.ReadValue<Vector2>();
        controlAudio.PlaySE("WakingSE");
    }

    private void OnMoveCanceled(InputAction.CallbackContext context){
        moveInput = Vector2.zero;
        controlAudio.StopSE();
    }

    private void OnDashPerformed(InputAction.CallbackContext context){
        playerData.speed = playerData.speed * dashSpeedRate;
        controlAudio.PlaySE("DashSE");
    }

    private void OnDashCanceled(InputAction.CallbackContext context){
        playerData.speed = playerData.speed / dashSpeedRate;
        controlAudio.StopSE();
    }

    void FixedUpdate(){
        if(Time.timeScale == 0) return;
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y) * playerData.speed * Time.deltaTime;
        transform.Translate(move);

        playerData._position = transform.position;
    }

}