
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(menuName = "Scriptable Objects/EnemyAbilities/SlimeAttackSuicide")]
public class AbSlimeAttackSuicide : EnemyAbilities
{
    public override void Activate(GameObject parent, Vector2 vector2, EnemySFX sfx)
    {
        Vector2 dirattack = (Vector2)parent.transform.position + (vector2 * offset);
        Collider2D[] hit = Physics2D.OverlapCircleAll(dirattack, range);
        sfx.AttackImpactClip(setAbilitySFXEnemy.attackImpact_Clip);
        foreach (var hits in hit)
        {
            if (hits.gameObject != parent.gameObject && hits.TryGetComponent(out IDamageable victim))
            {
               
                victim.TakeDamage(damage);
            }
        }
        Destroy(parent.gameObject, setAbilitySFXEnemy.attackImpact_Clip.length);

    }
}
