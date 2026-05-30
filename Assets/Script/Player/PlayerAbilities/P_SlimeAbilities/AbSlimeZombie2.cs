using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/PlayerAbilities/AbSlimeZombie2")]
public class AbSlimeZombie2 : PlayerAbilities
{
    public float strenghtKnockback;
    public override void Activate(GameObject parent, Vector2 mousePos)
    {
        float damagemodifier = 0f;
        if (parent.TryGetComponent(out PlayerAttack playerAttack)) damagemodifier = playerAttack.damageModifier;
 

        Vector2 direct = (mousePos - (Vector2)parent.transform.position).normalized;
        Collider2D[] hits = Physics2D.OverlapCircleAll(parent.transform.position + (Vector3)direct * offset, range,layerAttack);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                if (hit.TryGetComponent(out EnemyHealth health)) health.SetLastAttacker(parent);
                if (hit.TryGetComponent(out IDamageable d)) {
                    playerAttack.playerSFX.AttackImpactClip(setAbilitySFX.attackImpact_Clip);
                    d.TakeDamage(damage + damagemodifier);
                }
                if (strenghtKnockback > 0){
                    if (hit.TryGetComponent(out IKnockbackResistance t)) t.ApplyKnockback(direct, strenghtKnockback, 0.2f);
                };
            }
        }
    }
}
