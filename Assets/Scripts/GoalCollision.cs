using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class GoalCollision : MonoBehaviour
{
    //GameObject MainCharacter;
    GameObject Terminal;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("collision script start");
        SetEntity();
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.name == "Character(Clone)"){
            Debug.Log("collision Here!!");
            // prepare UI to make Decision about forward or not !!
            // END OF UI CONFIG
            if(Terminal == true) Destroy(Terminal);
            
        }
    }
    

    public void SetEntity(){
        //MainCharacter = GameObject.Find("Character(Clone)");
        Terminal = GameObject.Find("Terminal(Clone)");
    }
}
