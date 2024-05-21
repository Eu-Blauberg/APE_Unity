using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SceneControl : MonoBehaviour
{

    private InputAction _pressAnyKeyAction =　new InputAction(type: InputActionType.PassThrough, binding: "*/<Button>", interactions: "Press");

    private void OnEnable() => _pressAnyKeyAction.Enable();
    private void OnDisable() => _pressAnyKeyAction.Disable();


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 何か入力があれば
        if (_pressAnyKeyAction.triggered)
        {
            // 現在のシーンがタイトルだったときはプレイ画面に移動
            if (SceneManager.GetActiveScene().name == "GameTitle")
            {
                SceneManager.LoadScene("MazeCreateScene");
            }

            // 現在のシーンが終了画面だったときはタイトル画面に移動
            if (SceneManager.GetActiveScene().name == "GameOver" || SceneManager.GetActiveScene().name == "GameClear")
            {
                SceneManager.LoadScene("GameTitle");
            }
        }
    }
}