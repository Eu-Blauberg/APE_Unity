using UnityEngine;

public class Ghost : MonoBehaviour
{
    private float       speed = 3.0f; //移動速度
    private float       changeInterval = 4.0f; //方向を変える間隔
    private Vector3     minBounds; //移動範囲の最小値
    private Vector3     maxBounds; //移動範囲の最大値
    private GameObject  player; //プレイヤーオブジェクトの参照
    private Character   PlayerInfo; //プレイヤーオブジェクトのCharacterコンポネントを取得
    private float       followDistance = 10.0f; //プレイヤーを追従する距離
    private float       WakableLimitDistance = 20.0f;
    private Vector3     targetDirection; //現在の移動方向
    private float       timer; //次に方向を変えるまでのタイマー
    private bool        isFollowing; //プレイヤー追従フラグ
    private int         mazeSize;
    private float       RouteScale;
    private bool        isAudioPlaying = false; // オーディオが再生中かどうか
    

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
            if(!isAudioPlaying){
                //プレイヤー追従中はBGMを変更
                SoundManager.Instance.PlayBGM(BGMSoundData.BGM.Following);
                isAudioPlaying = true;
            }
        }
        else{
            isFollowing = false;
            timer += Time.deltaTime;
            //指定した時間が経過したら方向を変更
            if (timer > changeInterval){
                ChangeDirection();
            }

            if(isAudioPlaying){
                //プレイヤー追従中でない場合はBGMを変更
                SoundManager.Instance.PlayBGM(BGMSoundData.BGM.Main);
                isAudioPlaying = false;
            }
        }
        MoveObject();
    }

    //ランダムな方向に方向転換
    private void ChangeDirection()
    {
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);
        targetDirection = new Vector3(randomX, 0, randomZ).normalized;
        timer = 0; //タイマーリセット
    }

    //オブジェクト移動
    private void MoveObject(){
        if (isFollowing){ //距離がfollowDistance以下
            //プレイヤーに向かって移動
            targetDirection = (player.transform.position - transform.position).normalized;
        }
        transform.Translate(targetDirection * speed * Time.deltaTime, Space.World);

        //オブジェクトが範囲外に出ないように制限
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minBounds.x, maxBounds.x);
        pos.z = Mathf.Clamp(pos.z, minBounds.z, maxBounds.z); 
        //制限している
        
        transform.position = pos;
    }

    private void MobWalking(){
        if(followDistance < WakableLimitDistance) {

        }
    }

    public void SetPlayer(GameObject Character){
        player = Character;
        //PlayerInfo = player.GetComponent<Character();
    }

    public void SetBounds(int mazeSize, float RouteScale){
        this.mazeSize = mazeSize;
        this.RouteScale = RouteScale;
        Vector3 min = new Vector3(1*RouteScale,1*RouteScale,1*RouteScale);
        Vector3 max = new Vector3((mazeSize-2)*RouteScale, 1*RouteScale, (mazeSize-2)*RouteScale);
        minBounds = min;
        maxBounds = max;
    }

    public void SpownSiteDecisionMaker(){
        
    }
        public int[] Return_xy(){

        int[] initial_xy = new int[2];

        //初期位置を仮決定
        int initial_x = GiveSmallerThanMaxValue(mazeSize);
        int initial_y = GiveSmallerThanMaxValue(mazeSize);

        //初期位置について，例外を解決
        if(initial_x <= 2) initial_x += 2;
        if(initial_x >= mazeSize-2) initial_x -= 2;
        if(initial_y <= 2) initial_y += 2;
        if(initial_y >= mazeSize-2) initial_y -= 2;

        initial_xy[0] = initial_y;
        initial_xy[1] = initial_x;
        return initial_xy;
    }

    private int GiveSmallerThanMaxValue(int maxValue){
        System.Random r = new();
        int RandomValue = r.Next(maxValue);
        return RandomValue;
    }
}
