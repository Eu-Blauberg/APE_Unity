using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCompass : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform player;

    void Update()
    {
        // ターゲットの方向を取得
        Vector3 targetDir = target.position - player.position;
        // ターゲットの方向を正規化(単位ベクトルに変換)
        targetDir.Normalize();
        
        //オブジェクトを回転させる
        float angle = Mathf.Atan2(targetDir.x, targetDir.z) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(0, angle, 0);
    }
}
