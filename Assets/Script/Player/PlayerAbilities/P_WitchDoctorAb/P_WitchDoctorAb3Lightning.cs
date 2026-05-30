using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/PlayerAbilities/WitchDoctorAb3Lightning")]
public class P_WitchDoctorAb3Lightning : PlayerAbilities
{
    public GameObject abilkaPrefab;
    public override void Activate(GameObject parent, Vector2 mousePos)
    {
        float damagemodifier = 0f;
        if (parent.TryGetComponent(out PlayerAttack playerAttack)) damagemodifier = playerAttack.damageModifier;
        Vector2 dir = (mousePos - (Vector2)parent.transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        GameObject ab = GameObject.Instantiate(abilkaPrefab, (Vector2)parent.transform.position + dir * offset, Quaternion.Euler(0, 0, angle + 90));
        if (ab.TryGetComponent(out P_Necromancer1FlameProjectile q))
        {
            q.loopclip = setAbilitySFX.attackLoop_Clip;
            q.impactclip = setAbilitySFX.attackImpact_Clip;
            q.Launch(dir, damage+damagemodifier, range, speed);
        }
    }
}


