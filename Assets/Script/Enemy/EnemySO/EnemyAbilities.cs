using UnityEngine;


public abstract class EnemyAbilities : ScriptableObject
{
    public string nameAbility;
    [Header("EnemyAttack")]
    public float cooldown;
    [Header("dBA - The begining of the attack with current distance")]
    public float distanceBeforeAttack;
    public float attackDurationStun;
    public float offset;
    [Header("Settings Projectile")]
    public float range;
    public float speed;
    public float damage;
    
    public abstract void Activate(GameObject parent,Vector2 vector2,EnemySFX sfx);
    public SetAbilitySFXEnemy setAbilitySFXEnemy;
}
[System.Serializable]
public struct SetAbilitySFXEnemy
{
    public AudioClip attackLaunch_Clip;
    public AudioClip attackImpact_Clip;
    [Header("For projectile")]
    public AudioClip attackLoop_Clip;
}