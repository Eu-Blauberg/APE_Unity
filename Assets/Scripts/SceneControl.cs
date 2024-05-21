using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SceneControl : MonoBehaviour
{

    private InputAction _pressAnyKeyAction =�@new InputAction(type: InputActionType.PassThrough, binding: "*/<Button>", interactions: "Press");

    private void OnEnable() => _pressAnyKeyAction.Enable();
    private void OnDisable() => _pressAnyKeyAction.Disable();


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // �������͂������
        if (_pressAnyKeyAction.triggered)
        {
            // ���݂̃V�[�����^�C�g���������Ƃ��̓v���C��ʂɈړ�
            if (SceneManager.GetActiveScene().name == "GameTitle")
            {
                SceneManager.LoadScene("MazeCreateScene");
            }

            // ���݂̃V�[�����I����ʂ������Ƃ��̓^�C�g����ʂɈړ�
            if (SceneManager.GetActiveScene().name == "GameOver" || SceneManager.GetActiveScene().name == "GameClear")
            {
                SceneManager.LoadScene("GameTitle");
            }
        }
    }
}