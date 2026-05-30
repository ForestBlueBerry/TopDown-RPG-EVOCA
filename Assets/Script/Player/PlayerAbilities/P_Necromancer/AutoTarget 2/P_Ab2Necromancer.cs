using UnityEngine;
[CreateAssetMenu(menuName = "Scriptable Objects/PlayerAbilities/NecromancerAb2Target")]
public class P_Ab2Necromancer : PlayerAbilities
{
    public GameObject ability;
    public override void Activate(GameObject parent, Vector2 mousePos)
    {
        float damagemodifier = 0f;
        if (parent.TryGetComponent(out PlayerAttack playerAttack)) damagemodifier = playerAttack.damageModifier;

        Vector2 direct = (mousePos - (Vector2)parent.transform.position).normalized;

        GameObject ab = GameObject.Instantiate(ability, (Vector2)parent.transform.position+direct *offset,Quaternion.identity);

        if(ab.TryGetComponent(out P_Ab2NecromancerProjectile q))
        {
            q.clipImpact = setAbilitySFX.attackImpact_Clip;
            q.clipLoop = setAbilitySFX.attackLoop_Clip;
            q.Launch(direct, damage + damagemodifier, range, speed, layerAttack,parent.GetInstanceID());
        }
    }
}
