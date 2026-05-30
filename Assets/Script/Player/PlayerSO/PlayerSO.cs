using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "Scriptable Objects/PlayerSO")]
public class PlayerSO : ScriptableObject
{
    public Sprite icon;
    public RuntimeAnimatorController runtimeAnimatorController;

    public float speedWalk;
    public float speedRun;
    public float maxHealth;
    public float maxMana;
    [Header("The Capsule collider")]
    public Vector2 colliderOffset;
    public Vector2 colliderSize;
    public CapsuleDirection2D capsuleDirection;

    public PlayerAbilities[] abilities;
    public RuntimeAnimatorController iconUI;
    [Header("Require pick on bool!")]
    public setSizeIconAndBar setSizeIcon;
    public SetSFX setSFX;
    public int idCharacter;
}
[System.Serializable]
public struct setSizeIconAndBar
{
    public bool needSetSizeCanvasIcon;
    public Vector2 size;
    public float x;
    public float y;
    [Header("Require pick on bool! This is set high to health bar")]
    public bool needSetSizeHealthBarMini;
    public float healthy;
}
[System.Serializable]
public struct SetSFX
{
    public AudioClip walk_Clip;
    public AudioClip run_Clip;

    public AudioClip death_Clip;
    public AudioClip hurt_Clip;
}