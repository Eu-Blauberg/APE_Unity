using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SceneControl : MonoBehaviour
{

    [SerializeField] ItemDataBase itemDataBase;

    private InputAction _pressAnyKeyAction = new InputAction(type: InputActionType.PassThrough, binding: "*/<Button>", interactions: "Press");

    private void OnEnable() => _pressAnyKeyAction.Enable();
    private void OnDisable() => _pressAnyKeyAction.Disable();


    // Start is called before the first frame update
    void Start()
    {
        itemDataBase.CleanItem();
        
        if (SceneManager.GetActiveScene().name == "GameTitle")
        {
            SoundManager.Instance.PlayBGM(BGMSoundData.BGM.Title);
        }

        if (SceneManager.GetActiveScene().name == "MazeCreateScene")
        {
            SoundManager.Instance.PlayBGM(BGMSoundData.BGM.Main);
        }

        if (SceneManager.GetActiveScene().name == "GameOver")
        {
            SoundManager.Instance.PlayBGM(BGMSoundData.BGM.GameOver);
        }

        if (SceneManager.GetActiveScene().name == "GameClear")
        {
            SoundManager.Instance.PlaySE(SESoundData.SE.GameClear);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_pressAnyKeyAction.triggered)
        {
            if (SceneManager.GetActiveScene().name == "GameTitle")
            {
                SceneManager.LoadScene("MazeCreateScene");
            }

            if (SceneManager.GetActiveScene().name == "GameOver" || SceneManager.GetActiveScene().name == "GameClear")
            {
                SceneManager.LoadScene("GameTitle");
            }
        }
    }
}