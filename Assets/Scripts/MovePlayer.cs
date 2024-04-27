using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * 0.1f;
        }
        if(Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * 0.1f;
        }
        if(Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * 0.1f;
        }
        if(Input.GetKey(KeyCode.A))
        {
            transform.position -= transform.right * 0.1f;
        }
    }
}
