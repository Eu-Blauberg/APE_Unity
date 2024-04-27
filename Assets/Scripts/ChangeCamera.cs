using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    [SerializeField] Camera camera1;
    [SerializeField] Camera camera3;
    
    // Start is called before the first frame update
    void Start()
    {
        camera1.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            camera1.gameObject.SetActive(!camera1.gameObject.activeSelf);
            camera3.gameObject.SetActive(!camera3.gameObject.activeSelf);
        }
    }
}
