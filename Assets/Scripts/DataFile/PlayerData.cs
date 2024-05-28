using UnityEngine;
using System;

[System.Serializable]
[CreateAssetMenu(fileName = "PlayerData", menuName = "CreatePlayerData")]
public class PlayerData : ScriptableObject
{
    public int life; // 体力
    public float speed; // 移動速度
    public float sensivity; // 視点感度
    public Vector3 _position; // プレイヤーの位置

    public PlayerData(int life, float speed, float sensivity, Vector3 _position)
    {
        this.life = life;
        this.speed = speed;
        this.sensivity = sensivity;
        this._position = _position;
    }
}