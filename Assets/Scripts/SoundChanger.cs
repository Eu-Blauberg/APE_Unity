using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundChanger : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            SoundManager.Instance.PlayBGM(BGMSoundData.BGM.Main);
        }
    }
}
