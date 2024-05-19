using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectGameObject : MonoBehaviour
{
    /*
    シーン遷移でオブジェクトが破棄されないようにする
    */
    // Start is called before the first frame update
    void Start()
    {
        //画面遷移してもオブジェクトが壊れないようにする
        DontDestroyOnLoad (this);
    }
}
