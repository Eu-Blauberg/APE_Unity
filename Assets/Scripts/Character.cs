using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    private GameObject instantedMainCharacter;
    // Start is called before the first frame update
    private int initial_y;
    private int initial_x;
    private float RouteScale;

    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey("up")) // ↑なら前(Z 方向)に 0.1 だけ進む
        {
        instantedMainCharacter.transform.position += instantedMainCharacter.transform.forward * 0.1f;
        }
        if (Input.GetKey("down")) // ↓なら-Z 方向に 0.1 だけ進む
        {
        instantedMainCharacter.transform.position -= instantedMainCharacter.transform.forward * 0.1f;
        }
        if (Input.GetKey ("right")) // ←なら Y 軸に 3 度回転する
        {
        instantedMainCharacter.transform.Rotate(0f,3.0f,0f);
        }
        if (Input.GetKey ("left")) // →なら Y 軸に-3 度回転する
        {
        instantedMainCharacter.transform.Rotate(0f, -3.0f, 0f);
        }
    }
    
    public void SetInitialData(int initial_y, int initial_x, float RouteScale){
        this.initial_x = initial_x;
        this.initial_y = initial_y;
        this.RouteScale = RouteScale;
        instantedMainCharacter.transform.position = new Vector3(initial_y*RouteScale,1*RouteScale,initial_x*RouteScale);
    }

    public void SetInstans(GameObject character){
        instantedMainCharacter = character;
    }

    public void SwitchActivator(bool state){
        instantedMainCharacter.gameObject.SetActive(state);
    }
}
