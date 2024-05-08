using UnityEngine;

public class ghost2 : MonoBehaviour
{
    public float speed = 5.0f; //移動速度
    public float changeInterval = 2.0f; //方向を変える間隔
    public Vector3 minBounds; //移動範囲の最小値
    public Vector3 maxBounds; //移動範囲の最大値
    public GameObject player; //プレイヤーオブジェクトの参照
    public float followDistance = 5.0f; //プレイヤーを追従する距離
    private Vector3 targetDirection; //現在の移動方向
    private float timer; //次に方向を変えるまでのタイマー
    private bool isFollowing; //プレイヤー追従フラグ

    void Start()
    {
        ChangeDirection();
    }

    void FixedUpdate()
    {
        //プレイヤーとの距離を計算
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        //プレイヤーとの距離がfollowDistance以下なら追従
        if (distanceToPlayer <= followDistance){
            isFollowing = true;
        }
        else{
            isFollowing = false;
            timer += Time.deltaTime;
            //指定した時間が経過したら方向を変更
            if (timer > changeInterval){
                ChangeDirection();
            }
        }
        MoveObject();
    }

    //ランダムな方向に方向転換
    void ChangeDirection()
    {
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);
        targetDirection = new Vector3(randomX, 0, randomZ).normalized;
        timer = 0; //タイマーリセット
    }

    //オブジェクト移動
    void MoveObject()
    {
        if (isFollowing){ //距離がfollowDistance以下
            //プレイヤーに向かって移動
            targetDirection = (player.transform.position - transform.position).normalized;
        }
        transform.Translate(targetDirection * speed * Time.deltaTime, Space.World);
        //オブジェクトが範囲外に出ないように制限
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minBounds.x, maxBounds.x);
        pos.z = Mathf.Clamp(pos.z, minBounds.z, maxBounds.z); 
        transform.position = pos;
    }
}
