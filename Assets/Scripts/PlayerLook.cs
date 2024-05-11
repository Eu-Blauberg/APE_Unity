using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    public Transform cameraTransform; // カメラのTransform
    public float lookSpeed = 200f; // 回転速度
    private Vector2 lookInput;
    private float xRotation = 0f; // カメラのX軸（上下）回転を制御するための変数

    public InputActionAsset inputActionAsset; // Input Actionsアセットへの参照
    private InputAction lookAction; // 視点移動用のアクション

    void Start()
    {
        var actionMap = inputActionAsset.FindActionMap("PlayerControls");
        lookAction = actionMap.FindAction("Look");
        lookAction.Enable();
        lookAction.performed += OnLookPerformed;
        lookAction.canceled += OnLookCanceled;
    }

    private void OnLookPerformed(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    private void OnLookCanceled(InputAction.CallbackContext context)
    {
        lookInput = Vector2.zero;
    }

    void FixedUpdate()
    {
        // プレイヤーを左右に回転
        float mouseX = lookInput.x * lookSpeed * Time.deltaTime;
        transform.Rotate(0, mouseX, 0);

        // カメラを上下に回転（X軸）
        float mouseY = lookInput.y * lookSpeed * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void OnDestroy()
    {
        lookAction.Disable();
    }

    public float GetSensitivity()
    {
        return lookSpeed;
    }

    public void SetSensitivity(float value)
    {
        lookSpeed = value;
    }
}

