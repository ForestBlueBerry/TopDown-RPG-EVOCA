using System;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private Animator animator;
    private PlayerController controller;
    private PlayerAttack playerAttack;
    private PlayerHealth playerHealth;

 
    void Awake()
    {
        animator = GetComponent<Animator>();
        controller =GetComponent<PlayerController>();
        playerAttack = GetComponent<PlayerAttack>();
        playerHealth = GetComponent<PlayerHealth>();
        if (playerAttack != null)
        {
            playerAttack.OnTriggerAttack += OnTriggerAttack;
        }

        if (playerHealth != null)
        {
            playerHealth.OnTriggerHurt += OnTriggerHurt;
            playerHealth.OnTriggerDeath += OnTriggerDeath;
            playerHealth.OnResetDeath += OnTriggerRecoverAfterDeath;
        }

    }

    
    void Update()
    {
        if (animator.GetBool("Death")) return;
        SetAnimation();
    }

    public void SetupAnimator(PlayerSO playerSO)
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        if (animator != null && playerSO != null && playerSO.runtimeAnimatorController != null)
        {
            animator.runtimeAnimatorController = playerSO.runtimeAnimatorController;
        }
    }
    private void SetAnimation()
    {
        if (animator == null || animator.runtimeAnimatorController == null || controller == null) return;
        animator.SetFloat("X", controller.directAnimator.x);
        animator.SetFloat("Y", controller.directAnimator.y);
        animator.SetBool("IsRun", controller.isRunning);
        animator.SetBool("IsWalk", controller.isWalking);
        animator.SetBool("canBeInterrupted", playerAttack.canAttack);
    }
    public void ResetAttack()
    {
        playerAttack.canAttack = true;
        animator.SetInteger("AttackIndex", 0);
    }

    private void OnTriggerAttack(int index)
    {
        animator.SetInteger("AttackIndex", index);
        animator.SetTrigger("IsAttack");
    }

    private void OnDestroy()
    {
        if (playerAttack != null) playerAttack.OnTriggerAttack -= OnTriggerAttack;
        if (playerHealth != null)
        {
            playerHealth.OnTriggerHurt -= OnTriggerHurt;
            playerHealth.OnTriggerDeath -= OnTriggerDeath;
            playerHealth.OnResetDeath -= OnTriggerRecoverAfterDeath;
        }
    }


    private void OnTriggerHurt()
    {
        animator.SetTrigger("IsHurt");
    }
    private void OnTriggerDeath()
    {
        animator.ResetTrigger("IsAttack");
        animator.ResetTrigger("IsHurt");

        animator.SetBool("Death", true);
        animator.SetTrigger("IsDeath");

    }

    private void OnTriggerRecoverAfterDeath()
    {
        animator.SetBool("Death", false);

        animator.ResetTrigger("IsAttack");
        animator.ResetTrigger("IsHurt");
        animator.ResetTrigger("IsDeath");

        animator.SetInteger("AttackIndex", 0);
        animator.SetFloat("X", 0);
        animator.SetFloat("Y", 0);
        animator.SetBool("IsRun", false);
        animator.SetBool("IsWalk", false);
        animator.SetBool("canBeInterrupted", true);
    }
}
