using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlCompass : MonoBehaviour
{
    [SerializeField] ItemDataBase itemDataBase;
    private Transform target;
    private Transform player;

    private Vector3 targetDir;
    private GameObject CompassAllow;
    private bool isCompass = false;

    void Start()
    {
        Vector3 localGoalPoint = GameObject.FindWithTag("Goal").transform.position;
        
        target = GameObject.FindWithTag("Goal").GetComponent<Transform>();
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();

        CompassAllow = transform.GetChild(0).gameObject;

        CompassAllow.SetActive(false);
    }

    void Update()
    {
        //コンパスを一つ以上持っている場合に一度だけ処理を行う
        if(itemDataBase.GetItemNum(itemDataBase.GetItemByName("コンパス")) > 0)
        {
            if(!isCompass)
            {
                CompassAllow.SetActive(true);
                isCompass = true;
            }
        }else return;

        // ターゲットの方向を取得
        targetDir = target.position - player.position;
        // ターゲットの方向を正規化(単位ベクトルに変換)
        targetDir.Normalize();

        //オブジェクトを回転させる
        float angle = Mathf.Atan2(targetDir.x, targetDir.z) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(0, angle, 0);
    }
}
