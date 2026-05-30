using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Scriptable Objects/EnemySO")]
public class EnemySO : ScriptableObject
{
    public RuntimeAnimatorController animator;
   
    public GameObject enemyPrefab;
    public float speedMove;
    public float speedRun;

    public float detectionRadius;
    public float maxHealth;
    public float stoppingDistance;
    public EnemyAbilities abilities;

    [Header("The Capsule Collider Settings")]
    public Vector2 colliderOffset;
    public Vector2 colliderSize ; 
    public CapsuleDirection2D capsuleDirection;
    public EnemyName enemyName;
    [Header("The Element of the mob")]
    public BottleElement bottle;


    [Header("ATTENTION!")]
    public bool boss;

    public SetSFXEnemy setSFXEnemy;
}

public enum EnemyName{ 
    none,
    snail,
    goblin,
    monsterplant,
    skeleton,
    slime,
    littledemon,
    boss
};

public enum BottleElement
{
    none,
    fleshDefaultSlimeBottle,
    goblinGreeFleshBottle,
    fireBottle,
    blueFleshZombieSlime,
    redFleshDemon,
    lightningBottle,
    greyFleshPossesed,
    bonesBottle,
    monsterPlantBottle,
    godStarBottle,
    explosionCultBottle,
    frozenBottle,
    necromancerBottle,
    witchDocBottle,
    vampiresBottle
}

[System.Serializable]
public struct SetSFXEnemy
{
    public AudioClip walk_Clip;
    public AudioClip run_Clip;

    public AudioClip death_Clip;
    public AudioClip hurt_Clip;
}