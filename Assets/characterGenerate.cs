using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterGenerate : MonoBehaviour
{
    [SerializeField]
    GameObject MainCharacter;

    public characterGenerate(int x, int y, int z){
        MainCharacter.transform.position = new Vector3(x,y,z);
    }
    // Start is called before the first frame update    
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("up")) // ↑なら前(Z 方向)に 0.1 だけ進む
        {
        transform.position += transform.forward * 0.1f;
        }
        if (Input.GetKey("down")) // ↓なら-Z 方向に 0.1 だけ進む
        {
        transform.position -= transform.forward * 0.1f;
        }
        if (Input.GetKey ("right")) // ←なら Y 軸に 3 度回転する
        {
        transform.Rotate(0f,3.0f,0f);
        }
        if (Input.GetKey ("left")) // →なら Y 軸に-3 度回転する
        {
        transform.Rotate(0f, -3.0f, 0f);
        }
    }
}
