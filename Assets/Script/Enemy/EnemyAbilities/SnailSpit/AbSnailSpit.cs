using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/EnemyAbilities/SnailSpit")]
public class AbSnailSpit : EnemyAbilities 
{
    public GameObject prefabAbility;
    public override void Activate(GameObject parent,Vector2 vector2, EnemySFX sfx)
    {
        int id = parent.GetInstanceID();
        GameObject spit= Instantiate(prefabAbility,parent.transform.position,Quaternion.identity);
        if(spit.TryGetComponent(out SnailSpitProjectile s))
        {
            s.Launch(vector2, speed,range,damage, id,setAbilitySFXEnemy.attackImpact_Clip, setAbilitySFXEnemy.attackLoop_Clip);
        }
    }
}
