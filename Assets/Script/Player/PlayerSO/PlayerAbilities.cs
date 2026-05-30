using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerAbilities : ScriptableObject
{
    public float cooldown;
    public float damage;
    public float range;
    public float speed;
   [Header("For mage else 0")]
    public float manaPerAttack;
   [Header("0-center")]
    public float offset;
    public bool executeAnimator;
    public LayerMask layerAttack;
    public abstract void Activate(GameObject parent, Vector2 mousePos);
    public SetAbilitySFX setAbilitySFX;
}

[System.Serializable]
public struct SetAbilitySFX
{
    public AudioClip attackLaunch_Clip;
    public AudioClip attackImpact_Clip;
    [Header("For projectile")]
    public AudioClip attackLoop_Clip;
}