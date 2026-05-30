using UnityEngine;
using UnityEngine.Audio;

public class P_Necromancer1FlameProjectile : MonoBehaviour 
{
    private Rigidbody2D rb;
    private float damage;
    private float range;
    private bool isDead = false;

    private Vector2 startPos;

    public AudioClip loopclip;
    public AudioClip impactclip;
    private AudioSource audioSource;

    private bool isFrozen;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;
    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
    }
    void Update()
    {
        FrozenSFXTimeScale();
        if (((Vector2)transform.position - startPos).sqrMagnitude > range * range) DestroyAfterSFX(); 
    }

    public void Launch(Vector2 direct, float damage, float range, float speed)
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = loopclip;
        audioSource.Play();
        startPos = transform.position;

        this.damage = damage;
        this.range = range;

        rb.linearVelocity = direct * speed;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Projectile")) return;
        if (collision.CompareTag("Obstacle")) { rb.linearVelocity = Vector2.zero; DestroyAfterSFX(); };
        if (!isDead && collision.CompareTag("Enemy"))
        {
            if (collision.TryGetComponent(out EnemyHealth health)) health.SetLastAttacker(this.gameObject);
            DestroyAfterSFX();
            collision.GetComponent<IDamageable>()?.TakeDamage(damage);
            rb.linearVelocity = Vector2.zero;
        }
    }

    public void DestroyL()
    {
        Destroy(this.gameObject);
    }
    public void DestroyAfterSFX()
    {
        if (!isDead) {
            isDead = true;
            audioSource.Stop();
            audioSource.PlayOneShot(impactclip);
            rb.linearVelocity = Vector2.zero;
            circleCollider.enabled = false;
            spriteRenderer.enabled = false;
            animator.enabled = false;
            Destroy(this.gameObject, impactclip.length);
        }
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
