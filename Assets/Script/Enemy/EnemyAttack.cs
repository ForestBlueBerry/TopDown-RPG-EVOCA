using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private float cooldown;
    private float distanceBeforeAttack;
    private float attackDurationStop;
    private float attackOffset;
    private float lastAttackTime;
    private EnemyController enemyController;
    private bool boss;
    private int lastFrameShot = -1;
    private EnemySFX enemySFX;
    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        enemySFX  = GetComponent<EnemySFX>();
        Setup();
    }
    void Update()
    {
        if (!enemyController.enabled) return;

        if (enemyController.canSeeForAttack && enemyController.distanceClosesPointVictimSqr <= (distanceBeforeAttack * distanceBeforeAttack) && Time.time >= lastAttackTime + cooldown)
        {
            if (boss)
            {
                enemyController.StopBeforeAttack(attackDurationStop);
                enemyController.animator.SetInteger("BossAttack", Random.Range(0, 3));
                enemySFX.AttackLaunchClip(enemyController.enemySO.abilities.setAbilitySFXEnemy.attackLaunch_Clip);//SFX
                enemyController.animator.SetTrigger("isAttack");
                lastAttackTime = Time.time;
                return;
            }
            enemyController.StopBeforeAttack(attackDurationStop);
            enemySFX.AttackLaunchClip(enemyController.enemySO.abilities.setAbilitySFXEnemy.attackLaunch_Clip);  //SFX
            enemyController.animator.SetTrigger("isAttack");
            lastAttackTime = Time.time;
        }
    }
    public void Setup()
    {
        if (enemyController.enemySO.abilities != null)
        {
            var abilka = enemyController.enemySO.abilities;
            boss = enemyController.enemySO.boss;
            cooldown = abilka.cooldown;
            distanceBeforeAttack = abilka.distanceBeforeAttack;
            attackDurationStop = abilka.attackDurationStun;
            attackOffset = abilka.offset;
        }
    }

    public void ExecuteAbility()
    {
        if (Time.frameCount == lastFrameShot) return;
        lastFrameShot = Time.frameCount;
        if (enemyController == null || enemyController.victim == null) return;

        Vector2 dirAttack = (enemyController.victim.position - transform.position).normalized;

        if (enemyController.enemySO.abilities != null) enemyController.enemySO.abilities.Activate(this.gameObject, dirAttack, enemySFX);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        if (enemyController.victim != null)
        {
            Vector2 dirAttack2 = (enemyController.victim.position - transform.position).normalized;
            Vector2 attackCenter = (Vector2)transform.position + (dirAttack2 * attackOffset);
            Gizmos.DrawWireSphere(attackCenter, enemyController.enemySO.abilities.range);
        }
    }
}
