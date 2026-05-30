using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour,IDamageable
{
    [HideInInspector] public float currentHealth;
    public float maxHealth = 5;
    public EnemySO enemySO;
    private EnemyController enemyController;

    public Action<float, float> OnUpdateHpBossBar;


    public HealthBarShaderController healthBar;

    public GameObject lastAttacker;
    private float lastAttackerTime = 0f;
    private EnemySFX enemySFX;
    private EnemyAttack enemyAttack;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        enemySFX = GetComponent<EnemySFX>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        enemyAttack = GetComponent<EnemyAttack>();
        maxHealth = enemyController.enemySO.maxHealth;
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (lastAttacker != null) {
            lastAttackerTime -= Time.deltaTime;
            if (lastAttackerTime <= 0f ) lastAttacker = null;   
        } 
    }

    public void SetLastAttacker(GameObject attacker)
    {
        lastAttacker = attacker;
        lastAttackerTime = 5f;
    }

    void IDamageable.TakeDamage(float damage)
    {
        if (currentHealth <= 0) return;
        enemySFX.HurtClip();
      currentHealth -= damage;
        if (enemyController.isBoss) OnUpdateHpBossBar?.Invoke(currentHealth,maxHealth) ;
        healthBar.SetHealth(currentHealth, maxHealth);
        DamageNumberPool.Instance.OnShowDamage(this.gameObject.transform.position , damage,Color.yellow);
        if (currentHealth > 0) {
            enemyController.animator.SetTrigger("isHurt");
        }
        else
        {
            ThrowLoots();
            enemyController.animator.ResetTrigger("isHurt");
            enemyController.animator.ResetTrigger("isAttack");
            enemySFX.DeathClip();
            enemyController.animator.SetTrigger("isDeath");
            enemyAttack.enabled = false;
            enemyController.colider.enabled = false;
            enemyController.enabled = false;
            enemyController.rb.simulated = false;
            enemyController.agent.isStopped = true;
            Destroy(gameObject, 1.5f);
        }
    }

    private void ThrowLoots()
    {
        if (lastAttacker != null) {
            if (enemyController.enemySO.bottle != BottleElement.none)
            {
                PlayerInventory.Instance.GetBottle(enemyController.enemySO.bottle);
                DamageNumberPool.Instance.OnShowElement(this.gameObject.transform.position, Color.aquamarine);
            }
            PlayerInventory.Instance.GetExp(5);
            DamageNumberPool.Instance.OnShowExp(this.gameObject.transform.position,5, Color.green);
        }
    }



    public void DisableSuicideSlime()
    {
        currentHealth = 0;
        enemyAttack.enabled = false;
        enemyController.colider.enabled = false;
        spriteRenderer.enabled = false;
        animator.enabled = false;
        healthBar.gameObject.SetActive(false);
        enemyController.enabled = false;
    }
}
