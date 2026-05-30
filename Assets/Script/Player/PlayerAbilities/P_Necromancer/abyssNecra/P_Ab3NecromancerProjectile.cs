using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Timeline;
using UnityEngine.U2D;

public class P_Ab3NecromancerProjectile : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 startpos;
    private float damage;
    private float range;
    private float strenghtMagnit;
    private float timerMagnitism;
    private Animator animator;
    private float timeWork;
    private float tickAttack;

    private Dictionary<IDamageable,float>NextTimeToDamage = new Dictionary<IDamageable,float>();

    private AudioSource audioSource;
    public AudioClip clipsLoop;
    public AudioClip clipsEnd;
    private bool isFrozen;

    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        circleCollider = GetComponent<CircleCollider2D>();
    }
    void Update()
    {
        FrozenSFXTimeScale();
        if (((Vector2)transform.position-startpos).sqrMagnitude > range* range)rb.linearVelocity =Vector2.zero;
        if (Time.time > timeWork) animator.SetTrigger("End");
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Projectile") || collision.CompareTag("Obstacle")) return;
       
        if ( collision.CompareTag("Enemy") && collision.TryGetComponent(out IDamageable d) && collision.TryGetComponent(out IHasMagnitusm m))
        {
            m.onMagnitusm(transform.position, timerMagnitism, strenghtMagnit);
            if (!NextTimeToDamage.ContainsKey(d)||Time.time>= NextTimeToDamage[d])
            {
                if (collision.TryGetComponent(out EnemyHealth health))health.SetLastAttacker(this.gameObject);
                d.TakeDamage(damage);
                NextTimeToDamage[d] = Time.time+tickAttack;
            }
        }
    }

    public void Launch(Vector2 direct, float damage, float range, float speed,float strenghtMagnit,float timerMagnitism,float timeWork,float tickAttack)
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clipsLoop;
        audioSource.Play();
        startpos = transform.position;
        this.damage = damage;
        this.range = range;
        this.strenghtMagnit = strenghtMagnit;
        this.timerMagnitism = timerMagnitism;
        this.timeWork = Time.time+timeWork;
        this.tickAttack = tickAttack;
        rb.linearVelocity = direct * speed;
    }

    public void xDestroy()
    {
        circleCollider.enabled = false;
        spriteRenderer.enabled = false;
        //animator.enabled = false;
        audioSource.Stop();
        audioSource.PlayOneShot(clipsEnd);
        Destroy(this.gameObject,clipsEnd.length);
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
        }
        else if (isFrozen)
        {
            audioSource.UnPause();
            isFrozen = false;
        }
    }

}
