using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MazeCreater : MonoBehaviour
{   
    // 各ブロックの親要素のロード
    private GameObject WallBlock;
    private GameObject FloorBlock;
    private GameObject StairBlock;

    private int mazeSize = 21;
    private const int mazeStagnationStepsLimit = 4;
    private const float RouteScale = 3;


    private byte[][] mazeMap;
    private byte[][] MazeFullyBinaryMap;
    private int initial_y;
    private int initial_x;

    private int goal_y;
    private int goal_x;
    private int floorNumber = 1;

    // Start is called before the first frame update
    
    public void mazeCreate()
    {
        WallBlock = Instantiate((GameObject)Resources.Load("wall"));
        FloorBlock = Instantiate((GameObject)Resources.Load("floor"));
        StairBlock = Instantiate((GameObject)Resources.Load("stair"));
        //全体のマップをバイナリで保存するための二次元配列
        mazeMap = new byte[mazeSize][];
        //一行分の0詰めされた配列
        byte[] mazeMapEachLen = Enumerable.Repeat<byte>(0,mazeSize).ToArray();
        //大きさがmazeSizeの正方行列を作成
        for (int i = 0; i < mazeSize; i++) mazeMap[i] = mazeMapEachLen.Clone() as byte[];
        //仮想的なバイナリマップを取得
        MazeFullyBinaryMap = ReturnVirtualBinaryMap(mazeMap);
        Debug.Log("MazeFullyBinaryMap is completed");
        //実際に3D空間にブロックを配置する
        Fix3DMaze(MazeFullyBinaryMap);
    }
    

    // Update is called once per frame
    void FixedUpdate(){
        
    }

    //マップを配列によってバイナリで生成します
    // 1 : 道
    // 0 : 壁
    private byte[][] ReturnVirtualBinaryMap(byte[][] mazeEmptyMap){

        int LoopMakerKey = 5;
        int MakeLoopingPointTimes = 0;
        int RouteAchiveTimes = 0;
        int RouteTryTimes = 0;
        int RouteStagnationTimes;
        int direction;
        int[] Passed_xy = new int[2];
        int[] LoopingCandicate_xy = new int[2];

        // 初期位置の決定と代入
        int[] initial_xy = Return_xy();
        initial_y = initial_xy[0];
        initial_x = initial_xy[1];

        Debug.Log("Create Maze-GoalDecide mainbody... Start");
        // ゴール地点の決定と導入
        while(true){
            int[] Goal_xy = Return_xy();
            goal_y = Goal_xy[0];
            goal_x = Goal_xy[1];
            if(initial_x == goal_x && initial_y == goal_y) {
                Debug.Log("一致しているためゴールが決定しない -> continue");
                continue;
            }
            Debug.Log("ゴールが決定したため抜けます -> break");
            break;
        }
        Debug.Log("Create Maze-GoalDecide mainbody... End");

        // 引数のMAPをこの関数内で保持するためのコピーを生成
        byte[][] mazeBinaryMap = mazeEmptyMap.Clone() as byte[][];

        // 初期位置を決定
        mazeBinaryMap[initial_y][initial_x] = 1;

        // 明示的に探索させるセル情報へ代入
        int digingYcell = initial_y;
        int digingXcell = initial_x;

        Debug.Log("Create Maze mainbody... Start");
        while(RouteAchiveTimes < mazeSize*mazeSize*5){
            RouteTryTimes++;
            direction = GiveSmallerThanMaxValue(4);

            RouteStagnationTimes = RouteTryTimes - RouteAchiveTimes;

            if(RouteStagnationTimes % mazeStagnationStepsLimit == 0) {
                while(true){
                    Passed_xy = Return_xy();
                    digingYcell = Passed_xy[0];
                    digingXcell = Passed_xy[1];
                    if(mazeBinaryMap[digingYcell][digingXcell] != 1) continue;
                    break;
                }
            }

            switch(direction){
                case 0:
                if(digingYcell - 2 < 0 || mazeBinaryMap[digingYcell-2][digingXcell] != 0) break;
                if(mazeBinaryMap[digingYcell-1][digingXcell+1] != 0 || mazeBinaryMap[digingYcell-1][digingXcell-1] != 0) break;
                digingYcell -= 1;
                mazeBinaryMap[digingYcell][digingXcell] = 1;
                RouteAchiveTimes++;
                break;

                case 1:
                if(digingXcell - 2 < 0 || mazeBinaryMap[digingYcell][digingXcell-2] != 0) break;
                if(mazeBinaryMap[digingYcell-1][digingXcell-1] != 0 || mazeBinaryMap[digingYcell+1][digingXcell-1] != 0) break;
                digingXcell -= 1;
                mazeBinaryMap[digingYcell][digingXcell] = 1;
                RouteAchiveTimes++;
                break;

                case 2:
                if(digingYcell + 2 > mazeSize-1 || mazeBinaryMap[digingYcell+2][digingXcell] != 0) break;
                if(mazeBinaryMap[digingYcell+1][digingXcell-1] != 0 || mazeBinaryMap[digingYcell+1][digingXcell+1] != 0) break;
                digingYcell += 1;
                mazeBinaryMap[digingYcell][digingXcell] = 1;
                RouteAchiveTimes++;
                break;

                case 3:
                if(digingXcell + 2 > mazeSize-1 || mazeBinaryMap[digingYcell][digingXcell+2] != 0) break;
                if(mazeBinaryMap[digingYcell-1][digingXcell+1] != 0 || mazeBinaryMap[digingYcell+1][digingXcell+1] != 0) break;
                digingXcell += 1;
                mazeBinaryMap[digingYcell][digingXcell] = 1;
                RouteAchiveTimes++;
                break;

                default:
                break;
            }
        }
        Debug.Log("Create Maze mainbody... End");

        Debug.Log("Create Maze-Loop mainbody... Start");
        // 迷路にループ構造を生成
        while(mazeSize/LoopMakerKey > MakeLoopingPointTimes){
            LoopingCandicate_xy = Return_xy();
            digingYcell = LoopingCandicate_xy[0];
            digingXcell = LoopingCandicate_xy[1];
            if(mazeBinaryMap[digingYcell][digingXcell]!=0) continue;
            mazeBinaryMap[digingYcell][digingXcell] = 1;
            MakeLoopingPointTimes++;
        }
        Debug.Log("Create Maze-Loop mainbody... End");

        // ゴール地点を生成
        for(int i = -1; i < 2; i++){
            for(int j = -1; j < 2; j++){
                mazeBinaryMap[goal_y+i][goal_x+j] = 1;
            }
        }
        
        return mazeBinaryMap;
    }

    public int[] Return_xy(){

        int[] initial_xy = new int[2];
        int primarily_x;
        int primarily_y;

        //初期位置を仮決定
        primarily_x = GiveSmallerThanMaxValue(mazeSize);
        primarily_y = GiveSmallerThanMaxValue(mazeSize);

        //初期位置について，例外を解決
        if(primarily_x <= 2) primarily_x += 2;
        if(primarily_x >= mazeSize-2) primarily_x -= 2;
        if(primarily_y <= 2) primarily_y += 2;
        if(primarily_y >= mazeSize-2) primarily_y -= 2;

        initial_xy[0] = primarily_y;
        initial_xy[1] = primarily_x;

        return initial_xy;
    }

    public int[] Return_Secure_xy(){
        
        int[] initial_xy = new int[2];
        int primarily_x;
        int primarily_y;

        while(true){
            //初期位置を仮決定
            primarily_x = GiveSmallerThanMaxValue(mazeSize);
            primarily_y = GiveSmallerThanMaxValue(mazeSize);

            //初期位置について，例外を解決
            if(primarily_x <= 2) primarily_x += 2;
            if(primarily_x >= mazeSize-2) primarily_x -= 2;
            if(primarily_y <= 2) primarily_y += 2;
            if(primarily_y >= mazeSize-2) primarily_y -= 2;

            if(MazeFullyBinaryMap == null) Debug.Log("null");
            else Debug.Log("no - null");
            if(MazeFullyBinaryMap[primarily_y][primarily_x] == 1) break;
        }
        initial_xy[0] = primarily_y;
        initial_xy[1] = primarily_x;

        return initial_xy;
    }

    private int GiveSmallerThanMaxValue(int maxValue){
        System.Random r = new();
        int RandomValue = r.Next(maxValue);
        return RandomValue;
    }

    private void ShowBinaryMap(int[][] MazeBinaryMap){
        string message = "";
        for(int i = 0; i < mazeSize; i++){
            for(int j = 0; j < mazeSize; j++){
                message += MazeBinaryMap[i][j].ToString();
            }
            message += "\n";
        }Debug.Log(message);
    }

    private void Fix3DMaze(byte[][] mazeFullyBinaryMap){
        GameObject wall;
        GameObject floor;
        GameObject Stair;
        //GameObject bakedfloor;
        GameObject GoalJudgeSpace;
        // ブロックのロード
        GameObject wallPrefab = (GameObject)Resources.Load("WallCube");
        GameObject FloorPrefab = (GameObject)Resources.Load("FloorCube");
        GameObject StairPrefab = (GameObject)Resources.Load("BelowStair");
        GameObject BakedFloorPrefab = (GameObject)Resources.Load("FloorCube");

        GameObject GoalJudgeSpacePrefab = (GameObject)Resources.Load("Goaljudgement");
        //bakedfloor = Instantiate(BakedFloorPrefab);

        //bakedfloor.transform.position

        //床・穴・壁を生成する
        for(int m = 0; m < mazeSize; m++){
            for(int n = 0; n < mazeSize; n++){
                // ゴールの座標上は，別途設置処理
                if(m == goal_y && n == goal_x){
                    Stair = Instantiate(StairPrefab);
                    Stair.transform.position = new Vector3(m*RouteScale, 0, n*RouteScale);
                    Stair.transform.localScale = new Vector3(RouteScale*0.667f,RouteScale*0.33f,RouteScale*0.33f);
                    Stair.transform.parent = StairBlock.transform;

                    GoalJudgeSpace = Instantiate(GoalJudgeSpacePrefab);
                    GoalJudgeSpace.transform.position = new Vector3(m*RouteScale, 1*RouteScale, n*RouteScale);
                    GoalJudgeSpace.transform.localScale = new Vector3(RouteScale,RouteScale,RouteScale);
                    GoalJudgeSpace.transform.parent = StairBlock.transform;
                    
                }
                // 迷路の道を生成
                else{
                    // 壁の設置を行うのは，BinaryMapが0の時だけ
                    if(mazeFullyBinaryMap[m][n] == 0){
                        wall = Instantiate(wallPrefab);
                        wall.transform.position = new Vector3(m*RouteScale, 1*RouteScale, n*RouteScale);
                        wall.transform.localScale = new Vector3(RouteScale,RouteScale,RouteScale);
                        wall.transform.parent = WallBlock.transform;
                    }
                    // 床の設置を行うのは，BinaryMapが1の時だけ
                    else{
                        floor = Instantiate(FloorPrefab);
                        floor.transform.position = new Vector3(m*RouteScale, 0, n*RouteScale);
                        floor.transform.localScale = new Vector3(RouteScale,RouteScale,RouteScale);
                        floor.transform.parent = FloorBlock.transform;
                    }
                }

            }
        }
    }

    // ここで，該当階層踏破後のパラメータの更新を行う．
    private void MazeParamsUpdater(){
        if(floorNumber % 2 != 0) mazeSize += 2;
        floorNumber += 1;
        // 必要に応じて書き足していく
    }

    // 迷路の初期化処理=迷路の除去
    private void MazeInitialize(){
        foreach (Transform child in WallBlock.transform) Destroy(child.gameObject);  
        foreach (Transform child in FloorBlock.transform) Destroy(child.gameObject); 
        foreach (Transform child in StairBlock.transform) Destroy(child.gameObject);
    }
    
    
    public void MazeReCreate(){
        Debug.Log("MazeRecreate is running");
        Debug.Log(floorNumber);
        // 迷路の初期化処理を開始
        MazeParamsUpdater();
        Debug.Log("params updated");
        if(floorNumber % 2 != 0){
            Debug.Log("Remaze");
            MazeInitialize();
            mazeMap = new byte[mazeSize][];
            byte[] mazeMapEachLen = Enumerable.Repeat<byte>(0,mazeSize).ToArray();
            for (int i = 0; i < mazeSize; i++) mazeMap[i] = mazeMapEachLen.Clone() as byte[];
            byte[][] MazeFullyBinaryMap = ReturnVirtualBinaryMap(mazeMap);
            // 迷路の初期化処理を終了
            Fix3DMaze(MazeFullyBinaryMap);
            Debug.Log("MazeRecreate is completed");
        }
        else Debug.Log("No Create phase...");
    }

    public int GetInitialSpownCoordinate_x(){
        return initial_x;
    }

    public int GetInitialSpownCoordinate_y(){
        return initial_y;
    }

    public float GetRouteScale(){
        return RouteScale;
    }

    public int GetMazeScale(){
        return mazeSize;
    }

    public int GetFloorNumber(){
        return floorNumber;
    }
    
}
