using UnityEngine;
[CreateAssetMenu(menuName = "Scriptable Objects/PlayerAbilities/NecromancerAb3Abyss")]
public class P_Ab3Necromancer : PlayerAbilities
{
    [Header("Settings magnit zone")]
    public GameObject abilka;
    public float strenghtMagnit;
    public float timerMagnitism ;
    public float timeWork;
    public float tickAttack;
    public override void Activate(GameObject parent, Vector2 mousePos)
    {
        float damagemodifier = 0f;
        if (parent.TryGetComponent(out PlayerAttack playerAttack)) damagemodifier = playerAttack.damageModifier;

        Vector2 dir = (mousePos- (Vector2)parent.transform.position).normalized;

        GameObject fa = GameObject.Instantiate(abilka,(Vector2)parent.transform.position+ dir*offset,Quaternion.identity);

        if (fa.TryGetComponent(out P_Ab3NecromancerProjectile f)) {
            f.clipsLoop = setAbilitySFX.attackLoop_Clip;
            f.clipsEnd = setAbilitySFX.attackImpact_Clip;
            f.Launch(dir, damage + damagemodifier, range, speed, strenghtMagnit, timerMagnitism, timeWork, tickAttack);
        }
    }
}
