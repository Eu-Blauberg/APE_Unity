using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLifeCycleContoroler : MonoBehaviour
{
    GameObject Terminal;
    GameObject Hero;
    MazeCreater mazeCreater;
    Character character;

    // Start is called before the first frame update
    void Start()
    {
        GenTerminal();
        TerminalStarter();
        mazeCreater.mazeCreate();
        character.SetInitialData(mazeCreater.GetInitialSpownCoordinate_x(), mazeCreater.GetInitialSpownCoordinate_y(), mazeCreater.GetRouteScale());
    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.Find("Terminal(Clone)") == false) {
            Debug.Log("terminal is not found");
            Restart();
            GenTerminal();
        }
    }

    private void TerminalStarter(){
        Terminal.AddComponent<MazeCreater>();
        mazeCreater = Terminal.GetComponent<MazeCreater>();

        Hero = Instantiate((GameObject)Resources.Load("Character"));
        Hero.AddComponent<Character>();
        character = Hero.GetComponent<Character>();
        if(character != true) Debug.Log("character is not attached");

        if(Hero == true) Debug.Log("obj is true");

        character.SetInstans(Hero);
    }

    private void Restart(){
        character.SwitchActivator(false);
        mazeCreater.MazeReCreate();
        character.SetInitialData(mazeCreater.GetInitialSpownCoordinate_x(), mazeCreater.GetInitialSpownCoordinate_y(), mazeCreater.GetRouteScale());
        character.SwitchActivator(true);
    }

    private void GenTerminal(){
        GameObject terpre = (GameObject)Resources.Load("Terminal");
        Terminal = Instantiate(terpre);
    }
}
