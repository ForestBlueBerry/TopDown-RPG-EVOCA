using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(menuName = "Scriptable Objects/EnemyAbilities/SlimeAttack")]
public class AbSlimeAttack : EnemyAbilities
{
    public override void Activate(GameObject parent, Vector2 vector2, EnemySFX sfx)
    {
        int id =parent.GetInstanceID();
       Vector2 dirattack =(Vector2)parent.transform.position + (vector2* offset);
        Collider2D[] hit = Physics2D.OverlapCircleAll(dirattack, range);
       foreach (var hits in hit)
        {
            if(hits.gameObject.GetInstanceID() != id && hits.TryGetComponent(out IDamageable victim))
            {
                sfx.AttackImpactClip(setAbilitySFXEnemy.attackImpact_Clip);
                victim.TakeDamage(damage);
                
            }
        }
      
    }
}
