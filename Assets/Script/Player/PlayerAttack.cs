using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private Vector2 mousePos;
    public PlayerAbilities[] playerAbilities ;
    private float[] cooldown;
    private PlayerController PlayerController;
    private float lastExecuteTime;
    private PlayerHealth playerHealth;


    public float nextHit;
    public bool canAttack = true;
    public Action<int> OnTriggerAttack;
    public Action<bool> OnSignAttentionMana;


    public Action<float> OnDamageUIBook;
    public Action<float> OnCooldownUIBook;

    public float cooldownModifier = 0f;
    public float damageModifier = 0f;

    public PlayerSFX playerSFX;
    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        PlayerController = GetComponent<PlayerController>();
        if(playerAbilities != null)
        {
            cooldown = new float[playerAbilities.Length];
        }
        playerSFX = GetComponent<PlayerSFX>();
    }
    void Update()
    {
        if (Time.timeScale == 0f) return;
        Vector2 mousePixelPos = Mouse.current.position.ReadValue();
     
        mousePos = Camera.main.ScreenToWorldPoint(mousePixelPos);

        if (Mouse.current.leftButton.wasPressedThisFrame && !EventSystem.current.IsPointerOverGameObject()) UseAttack(0);
        if (Mouse.current.rightButton.wasPressedThisFrame && !EventSystem.current.IsPointerOverGameObject()) UseAttack(1);
        if (Keyboard.current.qKey.wasPressedThisFrame && !EventSystem.current.IsPointerOverGameObject()) UseAttack(2);
    }

    public void UseAttack(int index)
    {
        if (playerAbilities == null || index >= playerAbilities.Length || playerAbilities[index] == null ) return;

        if (playerHealth.currentMana < playerAbilities[index].manaPerAttack) {
            OnSignAttentionMana?.Invoke(true);
            return;
        }
    

        if (canAttack && Time.time > cooldown[index]-cooldownModifier) {//was change 
            playerHealth.currentMana -= playerAbilities[index].manaPerAttack;
            playerHealth.OnShowManaforUI?.Invoke(playerHealth.currentMana,playerHealth.maxMana);
            canAttack = false;
            PlayerController.directAnimator = (mousePos - (Vector2)transform.position).normalized;
            OnTriggerAttack?.Invoke(index);

            if (playerAbilities[index].executeAnimator) return;

            playerAbilities[index].Activate(this.gameObject, mousePos);
            cooldown[index]  = Time.time + (playerAbilities[index].cooldown - cooldownModifier);
        }
    }


    public void LoadAbilities(PlayerAbilities[] playerAbilities)
    {
        if(playerAbilities != null && playerAbilities.Length > 0)
        {
            this.playerAbilities = playerAbilities;
            this.cooldown = new float[playerAbilities.Length];
            OnDamageUIBook?.Invoke(playerAbilities[0].damage);
            OnCooldownUIBook?.Invoke(playerAbilities[0].cooldown);
            damageModifier = 0f;
            cooldownModifier = 0f;
        }
    }

    public void ExecuteAbility(int index) {
        if (Time.time - lastExecuteTime < 0.2f) return;
        lastExecuteTime = Time.time;
        if (playerAbilities == null || index >= playerAbilities.Length || playerAbilities[index] == null) return;
        playerSFX.AttackLaunchClip(playerAbilities[index].setAbilitySFX.attackLaunch_Clip);
        playerAbilities[index].Activate(this.gameObject, mousePos);
        cooldown[index] = Time.time + playerAbilities[index].cooldown;
    }

    public void OnDrawGizmos()
    {
        Vector3 directesa = (mousePos - (Vector2)transform.position).normalized;
        Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(transform.position + directesa * playerAbilities[0].offset, playerAbilities[0].range);
    }

    public void UpDamage(float updamage)
    {
        damageModifier += updamage;
        OnDamageUIBook?.Invoke(playerAbilities[0].damage + damageModifier);
    }
    public void DownCooldown(float downCooldown)
    {
            cooldownModifier += downCooldown;
            OnCooldownUIBook?.Invoke(playerAbilities[0].cooldown - cooldownModifier);
    }
    public void ResetAttackState()
    {
        canAttack = true;
    }
}
