using UnityEngine;
using System;

[System.Serializable]
[CreateAssetMenu(fileName = "PlayerData", menuName = "CreatePlayerData")]
public class PlayerData : ScriptableObject
{
    public int life;
    public float speed;
    public float sensivity;
    public Vector3 _position;

    public PlayerData(int life, float speed, float sensivity, Vector3 _position)
    {
        this.life = life;
        this.speed = speed;
        this.sensivity = sensivity;
        this._position = _position;
    }
}