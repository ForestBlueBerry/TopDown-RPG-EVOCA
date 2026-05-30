using System;
using System.Collections;
using UnityEngine;

public class SnailSpitProjectile : MonoBehaviour
{

    [HideInInspector] public float speed;
    [HideInInspector] public float damage;
    [HideInInspector] public Vector2 direct;

    private bool isDead = false;
    private Animator animator;
    private Vector2 startPos;
    private Rigidbody2D rb;
    private int id;
    private float sqrRange;

    private AudioClip impactClip;
    private AudioClip loopClip;
    private AudioSource audioSource;

    private bool isFrozen;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        FrozenSFXTimeScale();
        if ((startPos - (Vector2)transform.position).sqrMagnitude >= sqrRange) {
            Explode();
        }
    }
    public void Launch(Vector2 direct, float speed, float range,float damage,int id,AudioClip impactclip, AudioClip loopclip)
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();

        startPos = transform.position;
      
        this.impactClip = impactclip;
        this.loopClip = loopclip;

        this.direct = direct;
        this.speed = speed;
        this.sqrRange  = range * range;
        this.damage = damage;
        this.id = id;
        LoopClip();
        rb.linearVelocity = this.direct * this.speed;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.GetInstanceID() == id) return;

        if (!collision.CompareTag("Projectile"))
        {
            var damageable = collision.GetComponent<IDamageable>();

            if (damageable != null) {
                isDead = true;
                rb.simulated = false;
                damageable.TakeDamage(this.damage);
                ImpactClip();
                Explode();
            }
        }
    }
    public void Explode()
    {
        animator.SetTrigger("Explode");
    }

    public void Destroy()  // using by animator 
    {
        Destroy(gameObject,0.1f);
    }

    public void LoopClip()
    {
        audioSource.clip = this.loopClip;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void ImpactClip()
    {
        if (audioSource == null) return;
        audioSource.Stop();
        audioSource.PlayOneShot(impactClip);
    }

    private void FrozenSFXTimeScale()
    {
        if (Time.timeScale == 0f)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
                isFrozen = true;
            }
        }else if (isFrozen)
        {
                audioSource.UnPause();
                isFrozen = false;
        }
    }
}

