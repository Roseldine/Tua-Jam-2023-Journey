
using UnityEngine;

[CreateAssetMenu(menuName = "Game / Settings / Player Settings")]
public class PlayerSettingsSO : ScriptableObject
{
    [Header("Movement")]
    public float speedHorizontal    = 5;
    public float speedForwards      = 5;
    public float jumpHeight         = 10;
    public float gravity            = -15;

    [Header("Look")]
    public float lookSpeed      = 15;
    public float lookMultiplier = 5;
    
    [Header("Collision")]
    public float collisionKnockbackSpeed    = 5;
    public float collisionKnockbackHeight   = 1.5f;
    public float collisionKnockbackDistance = 2;

    [Header("Speed Gauge")]
    public int distanceDecimals = 2;
    public float minSpeedMultiplier = 1;
    public float maxSpeedMultiplier = 2;
    public float maxBoost = 10;

    [Header("Dragon Distance")]
    public float[] distances = { 5, 3, 1 };
    public float dragonSpeed = 15;
}