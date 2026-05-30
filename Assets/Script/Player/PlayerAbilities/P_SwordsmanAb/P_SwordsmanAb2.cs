using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/PlayerAbilities/AbSwordsmen2Extra")]
public class P_SwordsmanAb2 : PlayerAbilities
{
    public GameObject prefabAb;

    public override void Activate(GameObject parent, Vector2 mousePos)
    {
        float damagemodifier = 0f;
        if (parent.TryGetComponent(out PlayerAttack playerAttack)) damagemodifier = playerAttack.damageModifier;

        Vector2 dir = (mousePos - (Vector2)parent.transform.position).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;


        GameObject abilka = GameObject.Instantiate(prefabAb, (Vector2)parent.transform.position + dir * offset, Quaternion.Euler(0, 0, angle+90));
        Collider2D[] hits = Physics2D.OverlapCircleAll((Vector2)parent.transform.position + dir * offset, range, layerAttack);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                if (hit.TryGetComponent(out EnemyHealth health)) health.SetLastAttacker(parent);
                if (hit.TryGetComponent(out IDamageable d)) {
                    playerAttack.playerSFX.AttackImpactClip(setAbilitySFX.attackImpact_Clip);
                    d.TakeDamage(damage + damagemodifier);
                        };
            }
        }
        Destroy(abilka, 0.5f);
    }
}
