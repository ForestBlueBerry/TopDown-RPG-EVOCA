using Unity.Jobs;
using UnityEngine;
using UnityEngine.UI;
public class OpenHealthBarBoss : MonoBehaviour
{
    public Image animationAppearanceUIboss;
    public GameObject hpBarObject;
    public Animator bossAnimator;
    public EnemyHealth enemyHealth;

    private void Start()
    {
        enemyHealth.OnUpdateHpBossBar += UpdateHPUI;
    }

   private void UpdateHPUI(float currentHealth, float maxHealth) 
    {
        if (enemyHealth == null) return;

        float hpPercent = (float)currentHealth / maxHealth;
        hpPercent = Mathf.Clamp01(hpPercent);
        float finalValue = 1 - hpPercent; 
        bossAnimator.SetFloat("Hpequal", finalValue);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            hpBarObject.SetActive(true);
            bossAnimator.SetTrigger("ShowCurrentHp");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            hpBarObject.SetActive(false); 
        }
    }

    private void OnDestroy()
    {
        if (enemyHealth != null) enemyHealth.OnUpdateHpBossBar -= UpdateHPUI;
    }
}
