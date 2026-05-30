using System;
using System.Collections;
using UnityEngine;


public class PlayerHealth : MonoBehaviour,IDamageable
{
    public float maxHealth;
    private float currentHealth;

    public float maxMana;
    public float currentMana;

    public Action OnTriggerHurt;
    public Action OnTriggerDeath;
    public Action<float,float> OnShowHPforUI;
    public Action<float, float> OnShowManaforUI;
   
    private PlayerController conroller;
    private PlayerAttack playerAttack;
    public Transform respawn;
    public Action OnResetDeath;

    private PlayerSFX playerSFX;

    void Start()
    {
        conroller = GetComponent<PlayerController>();
        playerAttack = GetComponent<PlayerAttack>();
        playerSFX = GetComponent<PlayerSFX>();
        SetupPlayerHealthAndMana(conroller.so);
        StartCoroutine(ManaAndHpReovery());
    }
    void IDamageable.TakeDamage(float damage)
    {
        currentHealth -= damage;
        int clampHp = (int)Mathf.Clamp(currentHealth, 0, 10000);
        OnShowHPforUI?.Invoke(clampHp, maxHealth);
        DamageNumberPool.Instance.OnShowDamage(this.gameObject.transform.position, damage, Color.darkRed);
        if (currentHealth > 0)
        {
            playerSFX.HurtClip();
            OnTriggerHurt?.Invoke();
           
        }
        else {
            OffControlleDeath();
            OnTriggerDeath?.Invoke();
        }
    }
    public void SetupPlayerHealthAndMana(PlayerSO playerSO)
    {
        maxHealth = playerSO.maxHealth;
        maxMana = playerSO.maxMana;

        currentMana = maxMana;
        currentHealth = maxHealth;

        OnShowHPforUI?.Invoke(currentHealth, maxHealth);
        OnShowManaforUI?.Invoke(currentMana, maxMana);
    }


    public void OffControlleDeath()
    {
        playerSFX.DeathClip();
        conroller.enabled = false;
        conroller.rb.linearVelocity = Vector2.zero;
        playerAttack.enabled = false;
        conroller.capsuleCollider.enabled = false;
    }


    public void FinishDeath()
    {
        transform.position = respawn.position;
        StartCoroutine(CooldownDeath());
    }


    IEnumerator ManaAndHpReovery()
    {
        while (true) {

            yield return new WaitForSecondsRealtime(5);
            if (currentMana < maxMana) { 
            int rec = Mathf.FloorToInt(maxMana*0.1f);
                currentMana += rec;
                currentMana = Mathf.Clamp(currentMana, 0, maxMana);
                OnShowManaforUI?.Invoke(currentMana, maxMana);
            }

            if (currentHealth < maxHealth) {
            int recHp = Mathf.FloorToInt(maxHealth * 0.1f);
                currentHealth += recHp;
                currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
                OnShowHPforUI?.Invoke(currentHealth, maxHealth);
            }
        }
    }

    public void UpMAXHp(float hp)
    {
        maxHealth += hp;
        OnShowHPforUI?.Invoke(currentHealth, maxHealth);
    }
    public void UpMAXmana(float mana)
    {
        maxMana+= mana;
        OnShowManaforUI?.Invoke(currentMana, maxMana);
    }


    IEnumerator CooldownDeath()
    {
        yield return new WaitForSeconds(3f);
        currentHealth = maxHealth;
      
        conroller.enabled = true;
        playerAttack.enabled = true;
        conroller.capsuleCollider.enabled = true;

        playerAttack.ResetAttackState();

        OnResetDeath?.Invoke();
        OnShowHPforUI?.Invoke(currentHealth, maxHealth);
    }

}
// Need the Event for Death and change bool in the playerAttack script and player.visual && player.controoller == enable.false 