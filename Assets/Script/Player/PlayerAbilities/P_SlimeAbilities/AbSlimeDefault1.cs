using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/PlayerAbilities/AbSlimeDefault1")]
public class AbSlimeDefault1 : PlayerAbilities
{
    public override void Activate(GameObject parent, Vector2 mousePos)
    {
        float damagemodifier = 0f;
        if (parent.TryGetComponent(out PlayerAttack playerAttack)) damagemodifier = playerAttack.damageModifier;
       
        Collider2D[] hits = Physics2D.OverlapCircleAll(parent.transform.position ,range,layerAttack);
    
       foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                if (hit.TryGetComponent(out EnemyHealth health)) health.SetLastAttacker(parent);
                if (hit.TryGetComponent(out IDamageable d)) {
                    playerAttack.playerSFX.AttackImpactClip(setAbilitySFX.attackImpact_Clip);
                    d.TakeDamage(damage + damagemodifier);

                } 
            }
        }
    }
}
