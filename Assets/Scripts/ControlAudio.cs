using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlAudio : MonoBehaviour
{
    private AudioSource BGM;
    private AudioSource PlayerSE;
    private string BGMPath = "Audio/BGM/";
    private string SEPath = "Audio/SE/";

    void Start()
    {
        BGM = GameObject.FindWithTag("EventSystem").GetComponent<AudioSource>();
        BGM.clip = Resources.Load<AudioClip>(BGMPath + "MainBGM");
        BGM.loop = true;
        BGM.Play();

        //PlayerSEはPlayerMove.csで設定
    }

    public void PlayBGM(string BGMName)
    {
        BGM.clip = Resources.Load<AudioClip>(BGMPath + BGMName);
        BGM.Play();
    }

    public void PlaySE(string SEName)
    {
        PlayerSE.clip = Resources.Load<AudioClip>(SEPath + SEName);
        PlayerSE.Play();
    }

    public void StopSE()
    {
        PlayerSE.Stop();
    }

    public void StopBGM()
    {
        BGM.Stop();
    }

    public void SetCharacterObject(GameObject character)
    {
        PlayerSE = character.GetComponent<AudioSource>();
    }
}
