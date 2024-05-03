using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEditor.VersionControl;
using UnityEngine;

public class mazeCreater : MonoBehaviour
{   
    [SerializeField]
    GameObject WallBlock;
    [SerializeField]
    GameObject FloorBlock;
    [SerializeField]
    GameObject StairBlock;
    [SerializeField]
    GameObject MainCharacter;

    private int mazeSize = 21;
    private int mazeStagnationStepsLimit = 4;
    private float RouteScale = 3;

    private byte[][] mazeMap;
    int initial_y;
    int initial_x;

    int goal_y;
    int goal_x;

    // Start is called before the first frame update
    void Start()
    {
        //全体のマップをバイナリで保存するための二次元配列
        mazeMap = new byte[mazeSize][];
        //一行分の0詰めされた配列
        byte[] mazeMapEachLen = Enumerable.Repeat<byte>(0,mazeSize).ToArray();
        //大きさがmazeSizeの正方行列を作成
        for (int i = 0; i < mazeSize; i++) mazeMap[i] = mazeMapEachLen.Clone() as byte[];
        //仮想的なバイナリマップを取得
        byte[][] MazeFullyBinaryMap = ReturnVirtualBinaryMap(mazeMap);
        //実際に3D空間にブロックを配置する
        Fix3DMaze(MazeFullyBinaryMap);
        //Debug.Log(initial_y);
        //Debug.Log(initial_x);

        MainCharacter.transform.position = new Vector3(initial_y*RouteScale,1*RouteScale,initial_x*RouteScale);

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

        // ゴール地点の決定と導入
        while(true){
            int[] Goal_xy = Return_xy();
            if(initial_x == goal_x || initial_y == goal_y) continue;
            goal_y = Goal_xy[0];
            goal_x = Goal_xy[1];
            break;
        }

        // 引数のMAPをこの関数内で保持するためのコピーを生成
        byte[][] mazeBinaryMap = mazeEmptyMap.Clone() as byte[][];

        // 初期位置を決定
        mazeBinaryMap[initial_y][initial_x] = 1;

        // 明示的に探索させるセル情報へ代入
        int digingYcell = initial_y;
        int digingXcell = initial_x;

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

        // 迷路にループ構造を生成
        while(mazeSize/LoopMakerKey > MakeLoopingPointTimes){
            LoopingCandicate_xy = Return_xy();
            digingYcell = LoopingCandicate_xy[0];
            digingXcell = LoopingCandicate_xy[1];
            if(mazeBinaryMap[digingYcell][digingXcell]!=0) continue;
            mazeBinaryMap[digingYcell][digingXcell] = 1;
            MakeLoopingPointTimes++;
        }

        // ゴール地点を生成
        for(int i = -1; i < 2; i++){
            for(int j = -1; j < 2; j++){
                mazeBinaryMap[goal_y+i][goal_x+j] = 1;
            }
        }
        
        return mazeBinaryMap;
    }

    private int[] Return_xy(){

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
        GameObject StairPrefab = (GameObject)Resources.Load("BelowStair");
        GameObject wallPrefab = (GameObject)Resources.Load("WallCube");
        GameObject FloorPrefab = (GameObject)Resources.Load("FloorCube");

        //床・穴・壁を生成する
        for(int m = 0; m < mazeSize; m++){
            for(int n = 0; n < mazeSize; n++){
                // ゴールの座標上は，別途設置処理
                if(m == goal_y && n == goal_x){
                    Stair = Instantiate(StairPrefab);
                    Stair.transform.position = new Vector3(m*RouteScale, 0, n*RouteScale);
                    Stair.transform.localScale = new Vector3(RouteScale*0.667f,RouteScale*0.33f,RouteScale*0.33f);
                    Stair.transform.parent = StairBlock.transform;
                    continue;
                }
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
