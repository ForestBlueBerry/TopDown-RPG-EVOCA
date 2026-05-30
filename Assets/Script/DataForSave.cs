using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataForSave
{
    public int idCharacter;

    public float maxHealth;
    public float maxMana;

    public float cooldownModifier;
    public float damageModifier;

    public float speedRun;

    public float currentExp;

    public List<BottleElement> bottlekey;
    public List<int> bottlevalue;

    public Vector3 position;

    public int currentMutationPage;

    public int curtask;

    public float playtime;

    public string saveTime;

    public float completerewardexp;

    public void SaveModifier(PlayerHealth health, PlayerAttack attack, PlayerController controller, PlayerInventory inventory, FreshDataUI freshUI)
    {
        if (controller != null && controller.so != null)
        {
            idCharacter = controller.so.idCharacter;
            this.speedRun = controller.speedRun;
            position = controller.transform.position;
        }

        if (health != null)
        {
            this.maxHealth = health.maxHealth;
            this.maxMana = health.maxMana;
        }

        if (attack != null)
        {
            this.cooldownModifier = attack.cooldownModifier;
            this.damageModifier = attack.damageModifier;
        }

        if (inventory != null)
        {
            this.currentExp = inventory.currentExp;
            bottlekey = inventory.SaveStorageBottleKey();
            bottlevalue = inventory.SaveStorageBottleValue();
        }
        if (freshUI != null)
        {
            currentMutationPage = freshUI.currentMutationPage;
            curtask = freshUI.curtask;
        }
    }

    public void LoadModifier(PlayerHealth health, PlayerAttack attack, PlayerController controller, PlayerInventory inventory, FreshDataUI freshUI, QuestTask questTask)
    {
        if (health != null)
        {
            health.maxHealth = this.maxHealth;
            health.maxMana = this.maxMana;
        }

        if (attack != null)
        {
            attack.cooldownModifier = this.cooldownModifier;
            attack.damageModifier = this.damageModifier;
        }

        if (controller != null)
        {
            controller.speedRun = this.speedRun;
            controller.transform.position = position;
        }

        if (inventory != null)
        {
            inventory.currentExp = this.currentExp;
            inventory.LoadStorage(bottlekey, bottlevalue);
        }
        if (freshUI != null)
        {
            freshUI.currentMutationPage = this.currentMutationPage;
            freshUI.curtask = this.curtask;
        }

        //if (questTask != null)
        //{
        //    questTask.LoadTasks(curtask);
        //}
    }
}
