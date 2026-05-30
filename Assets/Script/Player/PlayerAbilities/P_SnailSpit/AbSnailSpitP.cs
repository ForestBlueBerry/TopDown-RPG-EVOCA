using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/PlayerAbilities/AbSnailSpitP")]
public class AbSnailSpitP : PlayerAbilities
{
    public GameObject prefabAbility;

    public override void Activate(GameObject parent, Vector2 mousePos)
    {
        float damagemodifier = 0f;
        if (parent.TryGetComponent(out PlayerAttack playerAttack)) damagemodifier = playerAttack.damageModifier;

        Vector2 direct = (mousePos - (Vector2)parent.transform.position).normalized;
        GameObject spit = Instantiate(prefabAbility, parent.transform.position +(Vector3)direct *offset, Quaternion.identity);
        if (spit.TryGetComponent(out SnailSpitProjectileP s))
        {
            s.loopclip = setAbilitySFX.attackLoop_Clip;
            s.impactclip = setAbilitySFX.attackImpact_Clip;
            s.Launch(direct, speed, range,damage + damagemodifier);
        }
    }
}
