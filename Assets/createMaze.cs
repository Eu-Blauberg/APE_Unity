using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEditor.VersionControl;
using UnityEngine;

public class mazeCreater : MonoBehaviour
{   
    private int mazeSize = 21;
    private int mazeStagnationStepsLimit = 4;

    // Start is called before the first frame update
    void Start()
    {
        //全体のマップをバイナリで保存するための二次元配列
        int[][] mazeMap = new int[mazeSize][];
        //一行分の0詰めされた配列
        int[] mazeMapEachLen = Enumerable.Repeat(0,mazeSize).ToArray();
        //大きさがmazeSizeの正方行列を作成
        for (int i = 0; i < mazeSize; i++) mazeMap[i] = mazeMapEachLen.Clone() as int[];
        //仮想的なバイナリマップを取得
        int[][] MazeFullyBinaryMap = ReturnVirtualBinaryMap(mazeMap);
        //実際に3D空間にブロックを配置する
        Fix3DMaze(MazeFullyBinaryMap);

        Debug.Log("処理完了");
    }

    // Update is called once per frame
    void Update(){

    }

    //マップを配列によってバイナリで生成します
    // 1 : 道
    // 0 : 壁
    private int[][] ReturnVirtualBinaryMap(int[][] mazeEmptyMap){

        int RouteAchiveTimes = 0;
        int RouteTryTimes = 0;
        int RouteStagnationTimes = 0;
        int direction = 0;
        int[] Passed_xy = new int[2];

        //初期位置の決定と代入
        int[] initial_xy = Return_xy();
        int initial_y = initial_xy[0];
        int initial_x = initial_xy[1];

        // 引数のMAPをこの関数内で保持するためのコピーを生成
        int[][] mazeBinaryMap = mazeEmptyMap.Clone() as int[][];

        // 初期位置を決定
        mazeBinaryMap[initial_y][initial_x] = 1;

        // 明示的に探索させるセル情報へ代入
        int digingYcell = initial_y;
        int digingXcell = initial_x;

        while(RouteAchiveTimes < mazeSize*mazeSize){
            RouteTryTimes++;
            ShowBinaryMap(mazeBinaryMap);
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
        return mazeBinaryMap;
    }

    private int[] Return_xy(){

        int[] initial_xy = new int[2];

        //初期位置を仮決定
        int initial_x = GiveSmallerThanMaxValue(mazeSize);
        int initial_y = GiveSmallerThanMaxValue(mazeSize);

        //初期位置について，例外を解決
        if(initial_x == 0) initial_x += 1;
        if(initial_x == mazeSize-1) initial_x -= 1;
        if(initial_y == 0) initial_y += 1;
        if(initial_y == mazeSize-1) initial_y -= 1;

        initial_xy[0] = initial_y;
        initial_xy[1] = initial_x;
        return initial_xy;
    }

    private int GiveSmallerThanMaxValue(int maxValue){
        System.Random r = new();
        int RandomValue = r.Next(maxValue);
        //Debug.Log(R.ToString());
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

    private void Fix3DMaze(int[][] mazeFullyBinaryMap){
        GameObject wall;
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.transform.position = new Vector3((mazeSize-1)/2, 0, (mazeSize-1)/2);
        floor.transform.localScale = new Vector3(mazeSize,1,mazeSize);

        for(int m = 0; m < mazeSize; m++){
            for(int n = 0; n < mazeSize; n++){
                if(mazeFullyBinaryMap[m][n] == 1) continue;
                wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                wall.transform.position = new Vector3(m, 1, n);
            }
        }
    }
}
