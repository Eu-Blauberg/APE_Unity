using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameLifeCycleContoroler : MonoBehaviour
{
    private GameObject      Terminal;
    private GameObject      Hero;
    private GameObject      TerminalPrefab;
    private GameObject      HeroPrefab;
    private GameObject      GhostPrefab;
    private GameObject[]    GhostObj = new GameObject[16];
    private GameObject      BagagesPrefab;
    private GameObject[]    BagagesObj = new GameObject[16];
    private MazeCreater     mazeCreater;
    private Character       character;
    private Ghost[]         ghost = new Ghost[16];
    private Bagages[]       bagages = new Bagages[16];      
    private const int       Goal_x = -48;
    private const int       Goal_y = -10;
    private const int       Goal_z = 0;
    private const int       Relay_x = -50;
    private const int       Relay_y = -2;
    private const int       Relay_z = 2;
    private float           RouteScale = 0f;
    

    // Start is called before the first frame update
    void Start()
    {
        GenTerminal();
        TerminalStarter();
        mazeCreater.mazeCreate();
        //GenGhost();
        //GenBagages();
        character.SetInitialData(mazeCreater.GetInitialSpownCoordinate_x(), mazeCreater.GetInitialSpownCoordinate_y(), mazeCreater.GetRouteScale());
    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.Find("Terminal(Clone)") == false) {
            Restart();
            GenTerminal();
        }
    }

    private void TerminalStarter(){
        // 迷路生成のコンストラクタ
        Terminal.AddComponent<MazeCreater>();
        mazeCreater = Terminal.GetComponent<MazeCreater>();
        GenCharacter();

    }

    private void Restart(){
        character.SwitchActivator(false);
        mazeCreater.MazeReCreate();
        if(mazeCreater.GetFloorNumber() % 2 != 0) character.SetInitialData(mazeCreater.GetInitialSpownCoordinate_x(), mazeCreater.GetInitialSpownCoordinate_y(), mazeCreater.GetRouteScale());
        else character.transform.position = new Vector3(Relay_x, Relay_y, Relay_z);
        //DelBagages();
        //DelGhosts();
        //GenBagages();
        //GenGhost();
        character.SwitchActivator(true);
    }

    

    private void GenTerminal(){
        Debug.Log("Generate Terminal");
        if(TerminalPrefab == null) TerminalPrefab = (GameObject)Resources.Load("Terminal");
        Terminal = Instantiate(TerminalPrefab);
    }

    private void GenCharacter(){
        if(HeroPrefab == null) HeroPrefab = (GameObject)Resources.Load("Character");
        Hero = Instantiate(HeroPrefab);
        Hero.AddComponent<Character>();
        character = Hero.GetComponent<Character>();
        character.SetInstans(Hero);
    }

    private void GenGhost(){
        if(GhostPrefab == null) GhostPrefab = (GameObject)Resources.Load("LittleGhost_L");
        for(int n = 0; n < mazeCreater.GetFloorNumber()+2; n++) {
            GhostObj[n] = Instantiate(GhostPrefab);
            GhostObj[n].AddComponent<Ghost>();
            ghost[n] = GhostObj[n].GetComponent<Ghost>();
            if(Hero != null) ghost[n].SetPlayer(Hero);
            ghost[n].SetBounds(mazeCreater.GetMazeScale(), mazeCreater.GetRouteScale());
            int[] initial_xy = new int[2];
            initial_xy = mazeCreater.Return_Secure_xy();
            GhostObj[n].transform.position = new Vector3(initial_xy[0]*RouteScale, 1*RouteScale, initial_xy[1]*RouteScale);
        }
    }

    private void GenBagages(){
        if(BagagesPrefab == null) BagagesPrefab = (GameObject)Resources.Load("Bag.2_White");
        if(RouteScale == 0f) RouteScale = mazeCreater.GetRouteScale();
        for(int n = 0; n < mazeCreater.GetFloorNumber()+5; n++) {
            BagagesObj[n] = Instantiate(BagagesPrefab);
            int[] initial_xy = new int[2];
            initial_xy = mazeCreater.Return_Secure_xy();
            BagagesObj[n].transform.position = new Vector3(initial_xy[0]*RouteScale, 1*RouteScale, initial_xy[1]*RouteScale);
        }
    }

    private void DelBagages(){
        for(int n = 0; n < BagagesObj.ToArray().GetLength(0); n++) {
            Debug.Log(n);
            BagagesObj[n] = null;
            bagages[n] = null;
        }
    }

    private void DelGhosts(){
        for(int n = 0; n < GhostObj.ToArray().GetLength(0); n++) {
            GhostObj[n] = null;
            ghost[n] = null;
        }
    }
}
