using UnityEngine;

public class HpBarBossEnable : MonoBehaviour
{
    public EnemyHealth enemyHealth;
    private void OnEnable()
    {
        if(enemyHealth == null) return;
        enemyHealth.OnUpdateHpBossBar?.Invoke(enemyHealth.currentHealth,enemyHealth.maxHealth);
    }
}
