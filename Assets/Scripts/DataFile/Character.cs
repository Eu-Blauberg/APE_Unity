using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
[CreateAssetMenu(fileName = "Character", menuName = "CreateCharacter")]
public class Character : ScriptableObject
{
    public int life = 3;
    public float speedRatio = 1;
    private float speedDownTime;
    private float sensitivity = 100f;

    public Character(Character character)
    {
        this.life = character.life;
        this.speedRatio = character.speedRatio;
        this.speedDownTime = character.speedDownTime;
        this.sensitivity = character.sensitivity;
    }
}
