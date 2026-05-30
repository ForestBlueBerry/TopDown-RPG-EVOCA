using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/EnemyAbilities/GoblinAttack")]
public class GoblinAttackAB : EnemyAbilities
{
    [Header("KnockbackResistance")]
    public float strenghtResist;
    public float knockbackTimer;
    public override void Activate(GameObject parent, Vector2 vector2, EnemySFX sfx)
    {
        
        Vector2 fullDirect = parent.transform.position + ((Vector3)vector2 * offset);
        Collider2D[] hits = Physics2D.OverlapCircleAll(fullDirect, range);
        
        if (hits.Length == 0 ) return;
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject== parent) continue;

            if ( hit.TryGetComponent(out IDamageable d))
            {
                sfx.AttackImpactClip(setAbilitySFXEnemy.attackImpact_Clip);
                d.TakeDamage(damage);
                if (hit.TryGetComponent(out IKnockbackResistance r)) r.ApplyKnockback(vector2, strenghtResist, knockbackTimer);
            }
        }
    }
}
