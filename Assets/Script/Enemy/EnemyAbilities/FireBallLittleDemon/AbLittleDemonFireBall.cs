using UnityEngine;
[CreateAssetMenu(menuName = "Scriptable Objects/EnemyAbilities/DemonFireBall")]
public class AbLittleDemonFireBall : EnemyAbilities
{
    public GameObject projectile;
   
    public override void Activate(GameObject parent, Vector2 vector2, EnemySFX sfx)
    {
        int id = parent.GetInstanceID();

        Vector2 startpos = parent.transform.position + (Vector3)vector2*offset;
     

        float angel =Mathf.Atan2(vector2.y, vector2.x)*Mathf.Rad2Deg;

        GameObject fireball = Instantiate(projectile,startpos,Quaternion.Euler(0,0,angel));
        if(fireball.TryGetComponent(out LittleDemonFireBallProjectile s))
        {
            s.Launch(vector2, speed,range,damage, id,setAbilitySFXEnemy.attackImpact_Clip,setAbilitySFXEnemy.attackLoop_Clip);
        }
    }
}
