using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IKnockbackResistance
{
    private float x ;
    private float y ;
    public Rigidbody2D rb;
    
    private float knockbackTimer;
    private float currentSpeed;
    private PlayerHealth health;
    private PlayerAttack playerAttack;
    public float speedRun;
    private float speedWalk;
    [HideInInspector]
    public CapsuleCollider2D capsuleCollider;
    private Vector2 direct;
    private PlayerVisual playerVisual;
    public PlayerSO so;
    [HideInInspector]
    public Vector2 directAnimator;
    [HideInInspector]
    public bool isRunning = false;
    [HideInInspector]
    public bool isWalking = false;
    [HideInInspector]
    public Action<string>OnSpeedRunUIBook;
    private PlayerSFX playerSFX;

    private bool lastWalking;
    private bool lastRunning;
    void Start()
    {
        capsuleCollider=GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        playerAttack = GetComponent<PlayerAttack>();
        health = GetComponent<PlayerHealth>();
        playerVisual = GetComponent<PlayerVisual>();
        playerSFX = GetComponent<PlayerSFX>();
        if (SaveManager.Instance.currentMode == SaveManager.GameMode.NewGame) SetupSO(so);
    }

    void Update()
    {
        FrozenTimeScaleSFX();
        if (Time.timeScale == 0f) return;
        Timer();
        GetInput();
    }
    void FixedUpdate()
    {
        if(knockbackTimer > 0) return;
        rb.linearVelocity = direct * currentSpeed; 

    }

    private void GetInput(){
       
        x = 0;
        y=0;
        currentSpeed = speedWalk;
        isRunning =false;

        if (Keyboard.current.wKey.isPressed) y =  1;
        if(Keyboard.current.sKey.isPressed) y = -1;
        if (Keyboard.current.aKey.isPressed) x = -1;
        if( Keyboard.current.dKey.isPressed) x = 1;
        direct=new Vector2(x,y).normalized;
     
        if (direct != Vector2.zero && playerAttack.canAttack) directAnimator = direct;

        if (Keyboard.current.leftShiftKey.isPressed) {
            isWalking = false;
            if (direct != Vector2.zero) {
                isRunning = true;
                currentSpeed = speedRun;
            }
        }else{ isWalking = direct != Vector2.zero;}

        if (isWalking != lastWalking)
        {
            playerSFX.WalkClip(isWalking);
            lastWalking = isWalking;
        }

        if (isRunning != lastRunning)
        {
            playerSFX.RunClip(isRunning);
            lastRunning = isRunning;
        }
    }
 
    
    private void Timer()
    {
        if (knockbackTimer > 0) knockbackTimer -= Time.deltaTime;
    }

    public void SetupSO(PlayerSO playerSO)
    {
        so = playerSO;
        speedRun = playerSO.speedRun;
        speedWalk = playerSO.speedWalk;
        OnSpeedRunUIBook?.Invoke(speedRun.ToString("F1"));

        capsuleCollider.offset = playerSO.colliderOffset;
        capsuleCollider.size = playerSO.colliderSize;
        capsuleCollider.direction = playerSO.capsuleDirection;

        playerAttack.LoadAbilities(playerSO.abilities);
        health.SetupPlayerHealthAndMana(playerSO);
        playerVisual.SetupAnimator(playerSO);
        playerSFX.SetupSFXController(playerSO);
    }
    void IKnockbackResistance.ApplyKnockback(Vector2 forceDirection, float strengthResist, float knockbackTimer)
    {
        this.knockbackTimer = knockbackTimer;
        rb.AddForce(forceDirection * strengthResist, ForceMode2D.Impulse);
    }

    public void UpSpeedRun(float speedRun)
    {
        this.speedRun += speedRun;
        OnSpeedRunUIBook?.Invoke(this.speedRun.ToString("F1"));
    }
    private void FrozenTimeScaleSFX()
    {
        if (Time.timeScale == 0f)
        {
            if (playerSFX.move_Source.isPlaying)
            {
                playerSFX.move_Source.Stop();
            }
        }
    }


    private void OnDisable()
    {
        directAnimator = Vector2.zero; 
        isRunning = false;
        isWalking = false;
        rb.linearVelocity = Vector2.zero;
    }

}
