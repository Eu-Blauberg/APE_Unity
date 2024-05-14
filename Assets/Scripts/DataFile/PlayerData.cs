using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData: MonoBehaviour
{
    public int life = 3;
    public float speedRatio = 1;
    private float speedDownTime;

    [SerializeField] private ItemDataBase itemDataBase;
    private ControlGameDisplay controlGameDisplay;

    void Start()
    {
        controlGameDisplay = GameObject.Find("GameDisplayManager").GetComponent<ControlGameDisplay>();
    }

    public void ReduceLife()
    {
        //インベントリにItemNameがバリアとなっているアイテムがある場合
        if(itemDataBase.items.Exists(item => item.itemName == "バリア"))
        {
            //バリアを1つ削除
            itemDataBase.RemoveItem(itemDataBase.items.Find(item => item.itemName == "バリア"));

            //UIにアイテムを反映
            controlGameDisplay.UpdateInventoryUI();
        }
        else
        {
            life --;
            //一定時間スピードを下げる
            //SpeedDownCoroutine();

            //UIにライフを反映
            controlGameDisplay.UpdateLifeUI(life);
        }
        
        if(life <= 0)
        {
            Debug.Log("GameOver");
            /*
            ゲームオーバー処理を追加したい
            */
        }
    }

    //非MonoBehaviourクラスでStartCoroutineを使えないので外部クラスを使ってコルーチンを呼び出す
    private void SpeedDownCoroutine()
    {
        IEnumerator coroutine = SpeedDown();
        var myMono = new MyMonoBehaviour(); //MyMonoBehaviourのインスタンスを生成
        myMono.CallStartCoroutine(coroutine); //MyMonoBehaviourのコルーチンを呼び出す
    }

    private IEnumerator SpeedDown()
    {
        speedRatio /= 2;
        yield return new WaitForSeconds(speedDownTime);
        speedRatio *= 2;
    }
}

//MonoBehaviourを継承していないクラスでコルーチンを使うためのクラス
public class MyMonoBehaviour : MonoBehaviour
{
    public void CallStartCoroutine(IEnumerator iEnumerator)
    {
        StartCoroutine(iEnumerator); //ここで実際にMonoBehaviour.StartCoroutine()を呼ぶ
    }
}
