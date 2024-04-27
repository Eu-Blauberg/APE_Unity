using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] GameObject player;

    [SerializeField] float se = 1.0f;//感度

    private void Start()
    {
    }
    void Update()
    {
        //------カメラの回転------

        // マウスの移動量を取得
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        // X方向に一定量移動していれば横回転
        if (Mathf.Abs(mx) > 0.01f)
        {
            // 回転軸はワールド座標のY軸
            player.transform.RotateAround(player.transform.position, Vector3.up, mx * se);
            
        }

        // Y方向に一定量移動していれば縦回転
        if (Mathf.Abs(my) > 0.01f)
        {
            // 回転軸はカメラ自身のX軸
            transform.RotateAround(player.transform.position, transform.right, -my * se);
        }
    }
}
